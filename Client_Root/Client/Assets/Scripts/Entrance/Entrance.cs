using UnityEngine;
using UnityEngine.SceneManagement;

public class Entrance : MonoBehaviour
{
    private string m_strIP = "172.30.1.18";
    private int m_nPort = 9110;

    private void Start()
    {
        MasterDataManager.Instance.DownloadMasterData("");

        Network.Instance.ConnectToServer(m_strIP, m_nPort, OnConnected, OnRecvMessage);
    }

    private void OnConnected(bool bResult)
    {
        Debug.Log("OnConnected! Result : " + bResult);
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
        if (Network.GetInstance() != null)
        {
            Network.Instance.RemoveConnectHandler(OnConnected);
            Network.Instance.RemoveRecvMessageHandler(OnRecvMessage);
        }
    }

#region Event Handler
    public void OnJoinLobbyBtnClicked()
    {
        JoinLobbyToL msgToL = new JoinLobbyToL();
        msgToL.m_strPlayerKey = SystemInfo.deviceUniqueIdentifier;
        msgToL.m_nAuthKey = 0;

        Network.Instance.Send(msgToL);
    }
#endregion
}
