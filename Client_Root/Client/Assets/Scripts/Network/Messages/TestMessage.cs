using System.Collections.Generic;

public class TestMessage : IMessage
{
	private string 				m_strName;			//	json field name : name
	public int 				m_nAge;				//	json field name : age
	public List<int>			m_listFavoriteNumbers;		//	json field name : favoriteNumbers	
	public Dictionary<string, string> 	m_dicOptions;			//	json field name : options

	public TestMessage()
	{
		m_listFavoriteNumbers = new List<int> ();
		m_dicOptions = new Dictionary<string, string> ();
	}

	public void SetName(string strName)
	{
		m_strName = strName;
	}

	public void SetAge(int nAge)
	{
		m_nAge = nAge;
	}

	public ushort GetID()
	{
		return (ushort)Messages.TEST_MESSAGE_ID;
	}

	public string Serialize()
	{
		JSONObject jsonObj = new JSONObject (JSONObject.Type.OBJECT);
		jsonObj.AddField ("m_strName", m_strName);
		jsonObj.AddField ("m_nAge", m_nAge);
		JSONObject arrNumbers = new JSONObject (JSONObject.Type.ARRAY);
		foreach (int nNumber in m_listFavoriteNumbers)
		{
			arrNumbers.Add (nNumber);
		}
		jsonObj.AddField ("m_listFavoriteNumbers", arrNumbers);
		JSONObject dicOptions = new JSONObject (m_dicOptions);
		jsonObj.AddField ("m_dicOptions", dicOptions);
			
		return jsonObj.Print ();
	}

	public bool Deserialize(string strJson)
	{
		JSONObject jsonObj = new JSONObject (strJson);

		if (jsonObj.HasField ("m_strName") && jsonObj.GetField ("m_strName").IsString)
		{
			jsonObj.GetField (ref m_strName, "m_strName");
		} 
		else
		{
			return false;
		}
			
		if (jsonObj.HasField ("m_nAge") && jsonObj.GetField ("m_nAge").IsNumber)
		{
			jsonObj.GetField (ref m_nAge, "m_nAge");
		} 
		else
		{
			return false;
		}
			
		if (jsonObj.HasField ("m_listFavoriteNumbers") && jsonObj.GetField ("m_listFavoriteNumbers").IsArray)
		{
			foreach (JSONObject j in jsonObj.GetField("m_listFavoriteNumbers").list)
			{
				m_listFavoriteNumbers.Add ((int)j.i);
			}
		}
		else
		{
			return false;
		}

		if (jsonObj.HasField ("m_dicOptions") && jsonObj.GetField ("m_dicOptions").IsObject)
		{
			foreach (KeyValuePair<string, string> kv in jsonObj.GetField("m_dicOptions").ToDictionary())
			{
				m_dicOptions.Add (kv.Key, kv.Value);
			}
		}
		else
		{
			return false;
		}

		return true;
	}
}
