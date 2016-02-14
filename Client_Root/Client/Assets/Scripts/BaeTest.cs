using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class BaeTest : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{	
		
		Network.Instance.ConnectToServer ("127.0.0.1", 9110, OnConnected, OnRecvMessage);
	}

	private float speed = 0.1F;
	public GameObject goCharacter;
	private Vector3 vec3Dest;

	// Update is called once per frame
	void Update ()
	{
		if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Began)
		{
			// Get movement of the finger since last frame
			vec3Dest = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

			ReqMove msg = new ReqMove ();
			msg.m_vec3Position = vec3Dest;

			Network.Instance.Send (msg);
		}

		if (Input.GetMouseButtonDown(0))
		{
			// Get movement of the finger since last frame
			vec3Dest = Camera.main.ScreenToWorldPoint(Input.mousePosition);

			ReqMove msg = new ReqMove ();
			msg.m_vec3Position = vec3Dest;

			Network.Instance.Send (msg);
		}



		// Move object across XY plane
		//transform.Translate(-touchDeltaPosition.x * speed, -touchDeltaPosition.y * speed, 0);

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
