using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class BaeTest : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{	
		StringBuilder 	CurMessage = new StringBuilder();

		Network.Instance.ConnectToServer ("127.0.0.1", 9110, OnConnected, OnRecvMessage);

		byte A = 65;
		byte B = 66;
		byte C = 67;

		CurMessage.Append ((char)A);
		CurMessage.Append ((char)B);
		CurMessage.Append ((char)C);

		string str = CurMessage.ToString();

		Debug.Log (CurMessage);
		Debug.Log (str);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKeyDown(KeyCode.Q))
		{
			TestMessage testMsg = new TestMessage ();
			testMsg.SetName ("배");
			testMsg.SetAge (34);
			testMsg.m_listFavoriteNumbers.Add (7);
			testMsg.m_dicOptions.Add ("햄스터", "햄메돌");

			Network.Instance.Send (testMsg);
		}
	}

	private void OnConnected(bool bSuccess)
	{
		Debug.Log ("OnConnected! Success : " + bSuccess);

		if (bSuccess)
		{
			
		}
	}

	private void OnRecvMessage(IMessage msg)
	{
		Debug.Log (msg.Serialize());
	}
}
