using UnityEngine;

public static class Extension
{
    public static Vector3 ToVector3(this string str)
    {
        if (str.StartsWith("(") && str.EndsWith(")"))
        {
            str = str.Substring(1, str.Length - 2);
        }

        string[] arrStr = str.Split(',');

        return new Vector3(float.Parse(arrStr[0]), float.Parse(arrStr[1]), float.Parse(arrStr[2]));
    }
}