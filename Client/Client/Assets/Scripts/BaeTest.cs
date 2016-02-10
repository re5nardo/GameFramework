using UnityEngine;
using System.Collections;

public class BaeTest : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{	
		Network.Instance.ConnectToServer (OnConnected);
	}
	
	// Update is called once per frame
	void Update ()
	{
		
	}

	private void OnConnected(bool bSuccess)
	{
		if (bSuccess)
		{
			TestMessage testMsg = new TestMessage ();
			testMsg.SetName ("Bae");
			testMsg.SetAge (34);
			testMsg.m_listFavoriteNumbers.Add (7);
			testMsg.m_dicOptions.Add ("햄스터", "햄메돌");

			Network.Instance.Send (testMsg);
		}
	}
}
