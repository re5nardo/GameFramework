using UnityEngine;
using UnityEngine.SceneManagement;

public class Entrance : MonoBehaviour
{
    private void Start()
    {
        MasterDataManager.Instance.DownloadMasterData("");

        Network.Instance.ConnectToServer(Config.Instance.GetLobbyServerIP(), Config.Instance.GetLobbyServerPort(), OnConnected, OnRecvMessage);
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
