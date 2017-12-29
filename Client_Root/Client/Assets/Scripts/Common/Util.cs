using UnityEngine;
using System.Collections.Generic;

public class Util
{
    public class Math
    {
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            float x = Mathf.Lerp(a.x, b.x, t);
            float y = Mathf.Lerp(a.y, b.y, t);
            float z = Mathf.Lerp(a.z, b.z, t);

            return new Vector3(x, y, z);
        }
    }

    public static void Parse(string text, char delim, List<string> output)
    {
        string[] arrText = text.Split(delim);

        output.Clear();
        foreach (string strText in arrText)
        {
            output.Add(strText);
        }
    }

    public static void Parse(string text, char delim, List<int> output)
    {
        if (string.IsNullOrEmpty(text))
            return;

        List<string> temp = new List<string>();
        Parse(text, delim, temp);

        output.Clear();
        foreach (string strText in temp)
        {
            int result = 0;
            if(Convert(strText, ref result))
            {
                output.Add(result);
            }
        }
    }

    public static void Parse(string text, char delim, List<float> output)
    {
        if (string.IsNullOrEmpty(text))
            return;

        List<string> temp = new List<string>();
        Parse(text, delim, temp);

        output.Clear();
        foreach (string strText in temp)
        {
            float result = 0;
            if(Convert(strText, ref result))
            {
                output.Add(result);
            }
        }
    }

    public static void Parse(string text, char delim, List<double> output)
    {
        if (string.IsNullOrEmpty(text))
            return;

        List<string> temp = new List<string>();
        Parse(text, delim, temp);

        output.Clear();
        foreach (string strText in temp)
        {
            double result = 0;
            if(Convert(strText, ref result))
            {
                output.Add(result);
            }
        }
    }

    public static List<List<string>> ReadCSV(string strFilePath)
    {
        List<List<string>> listData = new List<List<string>>();

        TextAsset textAsset = (TextAsset)Resources.Load(strFilePath) as TextAsset;

        string text = System.Text.Encoding.UTF8.GetString(textAsset.bytes); //  unity 2017 can't read text with ansi encoding..
        string[] lines = text.Replace("\r", "").Split('\n');

        foreach(string line in lines)
        {
            if (line == "")
                continue;

            List<string> listWord = new List<string>();
            bool bInsideQuotes = false;
            int nWordStart = 0;

            for (int i = 0; i < line.Length; ++i)
            {
                bool bLast = i == line.Length - 1;

                if (line[i] == ',')
                {
                    if (bLast)
                    {
                        listWord.Add(line.Substring(nWordStart, i - nWordStart));
                        listWord.Add("");
                    }
                    else
                    {
                        if (!bInsideQuotes)
                        {
                            listWord.Add(line.Substring(nWordStart, i - nWordStart));
                            nWordStart = i + 1;
                        }
                    }
                }
                else if (line[i] == '"')
                {
                    if (bInsideQuotes)
                    {
                        if (bLast)
                        {
                            listWord.Add(line.Substring(nWordStart, i - nWordStart).Replace("\"\"", "\""));
                        }
                        else
                        {
                            if (line[i + 1] != '"')
                            {
                                listWord.Add(line.Substring(nWordStart, i - nWordStart).Replace("\"\"", "\""));
                                bInsideQuotes = false;

                                if (line[i + 1] == ',')
                                {
                                    if (i == line.Length - 2)
                                    {
                                        listWord.Add("");
                                    }
                                    else
                                    {
                                        ++i;
                                        nWordStart = i + 1;
                                    }
                                }
                            }
                            else
                            {
                                ++i;
                            }
                        }
                    }
                    else
                    {
                        nWordStart = i + 1;
                        bInsideQuotes = true;
                    }
                }
                else
                {
                    if (bLast)
                    {
                        listWord.Add(line.Substring(nWordStart, line.Length - nWordStart));
                    }
                }
            }

            listData.Add(listWord);
        }

        return listData;
    }

    public static bool Convert(string strText, ref int output)
    {
        if (string.IsNullOrEmpty(strText))
            return false;

        int result = 0;
        if (int.TryParse(strText.Replace(" ", ""), out result))
        {
            output = result;
            return true;
        }
        else
        {
            Debug.LogWarning("Can't convert to int type! strText : " + strText);
            return false;
        }
    }

    public static bool Convert(string strText, ref float output)
    {
        if (string.IsNullOrEmpty(strText))
            return false;
        
        float result = 0;
        if (float.TryParse(strText.Replace(" ", ""), out result))
        {
            output = result;
            return true;
        }
        else
        {
            Debug.LogWarning("Can't convert to float type! strText : " + strText);
            return false;
        }
    }

    public static bool Convert(string strText, ref double output)
    {
        if (string.IsNullOrEmpty(strText))
            return false;
        
        double result = 0;
        if (double.TryParse(strText.Replace(" ", ""), out result))
        {
            output = result;
            return true;
        }
        else
        {
            Debug.LogWarning("Can't convert to double type! strText : " + strText);
            return false;
        }
    }
}