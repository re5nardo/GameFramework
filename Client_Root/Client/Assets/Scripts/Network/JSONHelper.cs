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

    public static void AddField<T>(JSONObject jsonObject, string strFieldName, List<T> listValue) where T : IJSONObjectable
    {
        JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);

        foreach(T data in listValue)
        {
            arr.Add(data.GetJSONObject());
        }

        arr.AddField(strFieldName, arr);

        jsonObject.AddField(strFieldName, arr);
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

        listValue.Clear();
        foreach(JSONObject element in jsonObject.list)
        {
            T value = new T();
            value.SetByJSONObject(element);

            listValue.Add(value);
        }

        return true;
    }
    #endregion
}
