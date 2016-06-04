using UnityEngine;
using System.Collections;

public class Room : MonoBehaviour
{
    [SerializeField] private IGame m_Game = null;

    private bool m_bConnected = false;
    private string m_strIP = "175.197.228.126";
    private string m_strState = "";

    private void OnGUI()
    {
        if (m_bConnected)
        {
            return;
        }
            
        m_strIP = GUILayout.TextField(m_strIP);

        if (GUILayout.Button("Connect"))
        {
            Network.Instance.ConnectToServer (m_strIP, 9110, OnConnected, OnRecvMessage);

            m_strState += "Try Connect\n";
        }

        GUILayout.TextArea(m_strState);
    }

    private void Start()
    {
        Application.targetFrameRate = 30;

        //Network.Instance.ConnectToServer ("175.197.288.126", 9110, OnConnected, OnRecvMessage);
    }

    private void OnConnected(bool bSuccess)
    {
        Debug.Log ("OnConnected! Success : " + bSuccess);
        m_strState += ("OnConnected! Success : " + bSuccess);

        if (bSuccess)
        {
            m_bConnected = true;
        }
    }

    private void OnRecvMessage(IMessage msg)
    {
        Debug.Log (msg.Serialize());

        m_Game.OnRecvMessage(msg);

        if (msg.GetID () == (ushort)Messages.REQ_MOVE_ID)
        {
//            ReqMove moveMsg = msg as ReqMove;
//            vec3Dest = new Vector3(moveMsg.m_vec3Position.x, moveMsg.m_vec3Position.y, 0f);
//
//            if (curCoroutine != null)
//            {
//                StopCoroutine (curCoroutine);
//            }
//            curCoroutine = StartCoroutine (MoveCoroutine ());
        }
    }

//    ReqMove msg = new ReqMove ();
//    msg.m_vec3Position = vec3Dest;
//
//    Network.Instance.Send (msg);
//
//    TestMessage testMsg = new TestMessage ();
//    testMsg.SetName ("배");
//    testMsg.SetAge (34);
//    testMsg.m_listFavoriteNumbers.Add (7);
//    testMsg.m_dicOptions.Add ("햄스터", "햄메돌");
//
//    Network.Instance.Send (testMsg);
}