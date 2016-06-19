using UnityEngine;
using System.Collections;
using System;

public class Room : MonoBehaviour
{
    [SerializeField] private IGame m_Game = null;

    private bool m_bConnected = false;
    private string m_strIP = "175.197.228.126";
    private string m_strAverage = "";
    private string m_strBest = "";
    private string m_strWorst = "";

    private void OnGUI()
    {
        if(m_bConnected == false)
        {
            m_strIP = GUILayout.TextField(m_strIP);

            if (GUILayout.Button("Connect"))
            {
                Network.Instance.ConnectToServer (m_strIP, 9110, OnConnected, OnRecvMessage);
            }
        }

        if (GUILayout.Button("average"))
        {
            double total = 0;
            double best = double.MaxValue;
            double worst = double.MinValue;

            foreach (TimeSpan span in BaeTest.listLatency)
            {
                if (span.TotalMilliseconds > worst)
                    worst = span.TotalMilliseconds;

                if (span.TotalMilliseconds < best)
                    best = span.TotalMilliseconds;

                total += span.TotalMilliseconds;
            }

            if (BaeTest.listLatency.Count == 0)
            {
                m_strAverage = m_strBest = m_strWorst = "";
            }
            else
            {
                m_strAverage = (total / BaeTest.listLatency.Count).ToString();
                m_strBest = best.ToString();
                m_strWorst = worst.ToString();
            }
        }

        GUILayout.TextField(m_strAverage);
        GUILayout.TextField(m_strBest);
        GUILayout.TextField(m_strWorst);
    }

    private void Start()
    {
        Application.targetFrameRate = 30;

        //Network.Instance.ConnectToServer ("175.197.288.126", 9110, OnConnected, OnRecvMessage);
    }

    private void OnConnected(bool bSuccess)
    {
        Debug.Log ("OnConnected! Success : " + bSuccess);

        if (bSuccess)
        {
            m_bConnected = true;
        }
    }

    private void OnRecvMessage(IMessage msg)
    {
        Debug.Log (msg.Serialize());

        m_Game.OnRecvMessage(msg);

        if (msg.GetID () == (ushort)Messages.TEST_MESSAGE_ID)
        {
            //  ...
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