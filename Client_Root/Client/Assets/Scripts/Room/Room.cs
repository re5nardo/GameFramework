using UnityEngine;
using System.Collections;
using System;

public class Room : MonoBehaviour
{
    [SerializeField] private IGame m_Game = null;

    //  Temp
    private string m_strIP = "175.197.228.153";
    private int m_nPort = 9111;

    private void Start()
    {
        Application.targetFrameRate = 30;

        RoomNetwork.Instance.ConnectToServer(m_strIP, m_nPort, OnConnected, OnRecvMessage);
    }

    private void OnConnected(bool bSuccess)
    {
        Debug.Log("OnConnected! Success : " + bSuccess);
    }

    private void OnRecvMessage(IMessage iMsg)
    {
        //  Check it's message regarding game
        if(CheckGameMessage(iMsg))
            m_Game.OnRecvMessage(iMsg);

        //  other message
        if (iMsg.GetID () == TestMessage.MESSAGE_ID)
        {
            //  ...
        }
    }

    private bool CheckGameMessage(IMessage iMsg)
    {
        if (iMsg.GetID() == GameEventMoveToC.MESSAGE_ID)
        {
            return true;
        }

        return false;
    }

    private void OnDestroy()
    {
        RoomNetwork.Instance.RemoveConnectHandler(OnConnected);
        RoomNetwork.Instance.RemoveRecvMessageHandler(OnRecvMessage);
    }
}