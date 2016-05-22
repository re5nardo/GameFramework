using UnityEngine;
using System.Xml;
using System.IO;

public class XmlEditor : Singleton<XmlEditor>
{
	public XmlDocument LoadXml(string strFilePath)
	{
		XmlDocument xmlDoc = new XmlDocument();

        xmlDoc.Load(strFilePath);

		return xmlDoc;
	}

    public XmlDocument LoadXmlFromResources(string strFilePath)
    {
        TextAsset textXML = Resources.Load<TextAsset>(strFilePath);
        if (textXML == null)
        {
            Debug.LogError("textXML is null!, strFilePath : " + strFilePath);
            return null;
        }

        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml (textXML.text);
        //xmlDoc.LoadXml (textXML.text.Trim());

        return xmlDoc;
    }

	public void SaveXml(string strFilePath, XmlDocument xmlDoc)
	{
		if(strFilePath.Contains(Path.DirectorySeparatorChar.ToString()))
		{
			string strDirectory = strFilePath.Remove(strFilePath.LastIndexOf (Path.DirectorySeparatorChar));
			if (!Directory.Exists(strDirectory))
			{
				Directory.CreateDirectory(strDirectory);
			}
		}

		using (TextWriter textWriter = new StreamWriter(strFilePath, false, System.Text.Encoding.UTF8))
		{
			xmlDoc.Save(textWriter);
		}
	}
}
