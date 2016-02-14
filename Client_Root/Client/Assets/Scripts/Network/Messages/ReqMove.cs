using UnityEngine;
using System.Collections;

public class ReqMove : IMessage
{
	public Vector3 	m_vec3Position;

	//	json field name : pos_x, pos_y, pos_z

	public ushort GetID()
	{
		return (ushort)Messages.REQ_MOVE_ID;
	}

	public string Serialize()
	{
		JSONObject jsonObj = new JSONObject (JSONObject.Type.OBJECT);
		jsonObj.AddField ("pos_x", m_vec3Position.x);
		jsonObj.AddField ("pos_y", m_vec3Position.y);
		jsonObj.AddField ("pos_z", m_vec3Position.z);

		return jsonObj.Print () + '\0';		//	for server
	}

	public bool Deserialize(string strJson)
	{
		JSONObject jsonObj = new JSONObject (strJson);

		if (jsonObj.HasField ("pos_x") && jsonObj.GetField ("pos_x").IsNumber)
		{
			jsonObj.GetField (ref m_vec3Position.x, "pos_x");
		} 
		else
		{
			return false;
		}

		if (jsonObj.HasField ("pos_y") && jsonObj.GetField ("pos_y").IsNumber)
		{
			jsonObj.GetField (ref m_vec3Position.y, "pos_y");
		} 
		else
		{
			return false;
		}

		if (jsonObj.HasField ("pos_z") && jsonObj.GetField ("pos_z").IsNumber)
		{
			jsonObj.GetField (ref m_vec3Position.z, "pos_z");
		} 
		else
		{
			return false;
		}

		return true;
	}
}
