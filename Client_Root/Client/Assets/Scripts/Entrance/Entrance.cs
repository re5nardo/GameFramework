using UnityEngine;
using UnityEngine.SceneManagement;

public class Entrance : MonoBehaviour
{
    private string m_strIP = "175.197.228.153";
    private int m_nPort = 9110;

    private void Start()
    {
        Network.Instance.ConnectToServer(m_strIP, m_nPort, OnConnected, OnRecvMessage);
    }

    private void OnConnected(bool bSuccess)
    {
        Debug.Log("OnConnected! Success : " + bSuccess);
    }

    private void OnRecvMessage(IMessage iMsg)
    {
        if (iMsg.GetID() == JoinLobbyToC.MESSAGE_ID)
        {
            OnJoinLobbyToC((JoinLobbyToC)iMsg);
        }
    }

    private void OnJoinLobbyToC(JoinLobbyToC msg)
    {
        if (msg.m_nResult == 0)
        {
            //  Go to Lobby
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            Debug.LogError("Join Lobby Fail! m_nResult : " + msg.m_nResult);
        }
    }

    private void OnDestroy()
    {
        Network.Instance.RemoveConnectHandler(OnConnected);
        Network.Instance.RemoveRecvMessageHandler(OnRecvMessage);
    }

#region Event Handler
    public void OnJoinLobbyBtnClicked()
    {
        JoinLobbyToS msgToS = new JoinLobbyToS();
        msgToS.m_strPlayerKey = SystemInfo.deviceUniqueIdentifier;
        msgToS.m_nAuthKey = 0;

        Network.Instance.Send(msgToS);
    }
#endregion
}
