using UnityEngine;
using System.Collections;
using System;

public class Room : MonoBehaviour
{
    [SerializeField] private IGame m_Game = null;

    private void Start()
    {
        Application.targetFrameRate = 30;

        Network.Instance.AddRecvMessageHandler(OnRecvMessage);
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
        Network.Instance.RemoveRecvMessageHandler(OnRecvMessage);
    }
}