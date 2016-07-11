using UnityEngine;
using UnityEngine.SceneManagement;

public class Entrance : MonoBehaviour
{
    private string m_strIP = "175.197.227.196";
    private int m_nPort = 9110;

    private void Start()
    {
        Network.Instance.ConnectToServer (m_strIP, m_nPort, OnConnected, OnRecvMessage);
    }

    private void OnConnected(bool bSuccess)
    {
        Debug.Log ("OnConnected! Success : " + bSuccess);
    }

    private void OnRecvMessage(IMessage msg)
    {
        if (msg.GetID () == (ushort)Messages.Join_Lobby_ToC)
        {
            OnJoinLobbyToC((JoinLobbyToC)msg);
        }
    }

    private void OnJoinLobbyToC(JoinLobbyToC msg)
    {
        if (msg.m_nResult == 0)
        {
            //  Go To Lobby
            SceneManager.LoadScene("Lobby");
        }
        else
        {
            Debug.LogError("Join Lobby Fail! m_nResult : " + msg.m_nResult);
        }
    }

#region Event Handler
    public void OnJoinLobbyBtnClicked()
    {
        JoinLobbyToS msgToS = new JoinLobbyToS ();
        msgToS.m_nPlayerNumber = 1;
        msgToS.m_nAuthKey = 0;

        Network.Instance.Send (msgToS);
    }
#endregion
}
