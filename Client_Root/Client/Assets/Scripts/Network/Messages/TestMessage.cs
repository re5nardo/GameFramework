using System.Collections.Generic;
using System.Text;

public class TestMessage : IMessage
{
    public const ushort MESSAGE_ID = 40000;

	private string 				        m_strName;			        //	json field name : name
	public int 				            m_nAge;				        //	json field name : age
	public List<int>			        m_listFavoriteNumbers;		//	json field name : favoriteNumbers	
	public Dictionary<string, string> 	m_dicOptions;			    //	json field name : options

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
        return MESSAGE_ID;
	}

	public byte[] Serialize()
	{
		JSONObject jsonObj = new JSONObject (JSONObject.Type.OBJECT);
		jsonObj.AddField ("name", m_strName);
		jsonObj.AddField ("age", m_nAge);
		JSONObject arrNumbers = new JSONObject (JSONObject.Type.ARRAY);
		foreach (int nNumber in m_listFavoriteNumbers)
		{
			arrNumbers.Add (nNumber);
		}
		jsonObj.AddField ("favoriteNumbers", arrNumbers);
		JSONObject dicOptions = new JSONObject (m_dicOptions);
		jsonObj.AddField ("options", dicOptions);

        return Encoding.Default.GetBytes(jsonObj.Print());
	}

    public bool Deserialize(byte[] bytes)
	{
        JSONObject jsonObj = new JSONObject (Encoding.Default.GetString(bytes));

		if (jsonObj.HasField ("name") && jsonObj.GetField ("name").IsString)
		{
			jsonObj.GetField (ref m_strName, "name");
		} 
		else
		{
			return false;
		}
			
		if (jsonObj.HasField ("age") && jsonObj.GetField ("age").IsNumber)
		{
			jsonObj.GetField (ref m_nAge, "age");
		} 
		else
		{
			return false;
		}
			
		if (jsonObj.HasField ("favoriteNumbers") && jsonObj.GetField ("favoriteNumbers").IsArray)
		{
			foreach (JSONObject j in jsonObj.GetField("favoriteNumbers").list)
			{
				m_listFavoriteNumbers.Add ((int)j.i);
			}
		}
		else
		{
			return false;
		}

		if (jsonObj.HasField ("options") && jsonObj.GetField ("options").IsObject)
		{
			foreach (KeyValuePair<string, string> kv in jsonObj.GetField("options").ToDictionary())
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
