using UnityEngine;
using UnityEngine.SceneManagement;

public class Entrance : MonoBehaviour
{
    private string m_strIP = "175.197.227.196";
    private int m_nPort = 9110;

    private void OnGUI()
    {
        GUI.Label(new Rect(new Vector2(Screen.width * 0.5f, Screen.height * 0.3f), new Vector2(200f, 100f)), "Welcom To Entrance");

        if (GUI.Button(new Rect(new Vector2(Screen.width * 0.4f, Screen.height * 0.5f), new Vector2(200f, 100f)), "Join Lobby"))
        {
            OnJoinLobbyClicked();
        }
    }

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

    private void OnJoinLobbyClicked()
    {
        JoinLobbyToS msgToS = new JoinLobbyToS ();
        msgToS.m_nPlayerNumber = 1;
        msgToS.m_nAuthKey = 0;

        Network.Instance.Send (msgToS);
    }
}
