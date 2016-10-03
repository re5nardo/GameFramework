using UnityEngine;
using System.Collections.Generic;

public static class JSONHelper
{
    #region AddField
    public static void AddField(JSONObject jsonObject, string strFieldName, int value)
    {
        jsonObject.AddField(strFieldName, value);
    }

    public static void AddField(JSONObject jsonObject, string strFieldName, long value)
    {
        jsonObject.AddField(strFieldName, value);
    }

    public static void AddField(JSONObject jsonObject, string strFieldName, float value)
    {
        jsonObject.AddField(strFieldName, value);
    }

    public static void AddField(JSONObject jsonObject, string strFieldName, string value)
    {
        jsonObject.AddField(strFieldName, value);
    }

    public static void AddField(JSONObject jsonObject, string strFieldName, JSONObject value)
    {
        jsonObject.AddField(strFieldName, value);
    }

    public static void AddField<T>(JSONObject jsonObject, string strFieldName, List<T> value) where T : IJSONObjectable
    {
        JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);

        foreach(T data in value)
        {
            arr.Add(data.GetJSONObject());
        }

        arr.AddField(strFieldName, arr);

        jsonObject.AddField(strFieldName, arr);
    }

    public static void AddField(JSONObject jsonObject, string strFieldName, Dictionary<int, string> value)
    {
        JSONObject obj = new JSONObject(JSONObject.Type.OBJECT);

        obj.keys = new List<string>();
        obj.list = new List<JSONObject>();

        foreach(KeyValuePair<int, string> kv in value)
        {
            obj.keys.Add(kv.Key.ToString());
            obj.list.Add(JSONObject.CreateStringObject(kv.Value));
        }

        jsonObject.AddField(strFieldName, obj);
    }
    #endregion

    #region GetField
    public static bool GetField(JSONObject jsonObject, string strFieldName, ref int value)
    {
        if(!jsonObject.HasField(strFieldName))
        {
            Debug.LogWarning("JSONObject does not have field, field name : " + strFieldName);
            return false;
        }

        if(!jsonObject.GetField(strFieldName).IsNumber)
        {
            Debug.LogWarning("Data type is invalid! It's not number");
            return false;
        } 

        return jsonObject.GetField(ref value, strFieldName);
    }

    public static bool GetField(JSONObject jsonObject, string strFieldName, ref long value)
    {
        if(!jsonObject.HasField(strFieldName))
        {
            Debug.LogWarning("JSONObject does not have field, field name : " + strFieldName);
            return false;
        }

        if(!jsonObject.GetField(strFieldName).IsNumber)
        {
            Debug.LogWarning("Data type is invalid! It's not number");
            return false;
        } 

        return jsonObject.GetField(ref value, strFieldName);
    }

    public static bool GetField(JSONObject jsonObject, string strFieldName, ref ulong value)
    {
        if(!jsonObject.HasField(strFieldName))
        {
            Debug.LogWarning("JSONObject does not have field, field name : " + strFieldName);
            return false;
        }

        if(!jsonObject.GetField(strFieldName).IsString)
        {
            Debug.LogWarning("Data type is invalid! It's not string");
            return false;
        }

        string strValue = "";
        if(!jsonObject.GetField(ref strValue, strFieldName))
        {
            Debug.LogWarning("Failed to GetField");
            return false;
        }
        value = uint.Parse(strValue);

        return true;
    }

    public static bool GetField(JSONObject jsonObject, string strFieldName, ref float value)
    {
        if(!jsonObject.HasField(strFieldName))
        {
            Debug.LogWarning("JSONObject does not have field, field name : " + strFieldName);
            return false;
        }

        if(!jsonObject.GetField(strFieldName).IsNumber)
        {
            Debug.LogWarning("Data type is invalid! It's not number");
            return false;
        } 

        return jsonObject.GetField(ref value, strFieldName);
    }

    public static bool GetField(JSONObject jsonObject, string strFieldName, ref string value)
    {
        if(!jsonObject.HasField(strFieldName))
        {
            Debug.LogWarning("JSONObject does not have field, field name : " + strFieldName);
            return false;
        }

        if(!jsonObject.GetField(strFieldName).IsString)
        {
            Debug.LogWarning("Data type is invalid! It's not string");
            return false;
        } 

        return jsonObject.GetField(ref value, strFieldName);
    }

    public static bool GetField(JSONObject jsonObject, string strFieldName, JSONObject value)
    {
        if(!jsonObject.HasField(strFieldName))
        {
            Debug.LogWarning("JSONObject does not have field, field name : " + strFieldName);
            return false;
        }

        if(!jsonObject.GetField(strFieldName).IsObject)
        {
            Debug.LogWarning("Data type is invalid! It's not object");
            return false;
        } 

        value = jsonObject.GetField(strFieldName);

        return value != null ? true : false;
    }

    public static bool GetField<T>(JSONObject jsonObject, string strFieldName, List<T> listValue) where T : IJSONObjectable, new()
    {
        if(!jsonObject.HasField(strFieldName))
        {
            Debug.LogWarning("JSONObject does not have field, field name : " + strFieldName);
            return false;
        }

        if(!jsonObject.GetField(strFieldName).IsArray)
        {
            Debug.LogWarning("Data type is invalid! It's not array");
            return false;
        }
            
        foreach(JSONObject element in jsonObject.list)
        {
            T value = new T();
            value.SetByJSONObject(element);

            listValue.Add(value);
        }

        return true;
    }

    public static bool GetField(JSONObject jsonObject, string strFieldName, Dictionary<int, string> dicValue)
    {
        if(!jsonObject.HasField(strFieldName))
        {
            Debug.LogWarning("JSONObject does not have field, field name : " + strFieldName);
            return false;
        }

        if(!jsonObject.GetField(strFieldName).IsObject)
        {
            Debug.LogWarning("Data type is invalid! It's not object");
            return false;
        }

        JSONObject field = jsonObject.GetField(strFieldName);
        for (int i = 0; i < field.list.Count; i++)
        {
            dicValue.Add(int.Parse(field.keys[i]), field.list[i].str);      
        }

        return true;
    }
    #endregion
}
