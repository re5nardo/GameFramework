using UnityEngine;
using System.Collections.Generic;

public static class JSONHelper
{
    public static void AddField<T>(JSONObject jsonObject, string strFieldName, List<T> listData) where T : IJSONObjectable
    {
        JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);

        foreach(T data in listData)
        {
            arr.Add(data.GetJSONObject());
        }

        arr.AddField(strFieldName, arr);
    }
}
