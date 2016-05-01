using UnityEngine;
using System.Xml;
using System.IO;

public class XmlEditor : Singleton<XmlEditor>
{
	public XmlDocument LoadXml(string strFilePath)
	{
//		TextAsset textAsset = (TextAsset)Resources.Load (strFilePath, typeof(TextAsset));
//		if (textAsset == null)
//		{
//			Debug.LogError ("textAsset is null!, strFileName : " + strFilePath);
//			return null;
//		}

		XmlDocument xmlDoc = new XmlDocument();
		//xmlDoc.LoadXml (textAsset.text.Trim());
        xmlDoc.Load(strFilePath);

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
