using UnityEngine;
using UnityEngine.SceneManagement;

public class Lobby : MonoBehaviour
{
    private void Start()
    {
        Network.Instance.AddRecvMessageHandler(OnRecvMessage);
    }

    private void OnRecvMessage(IMessage iMsg)
    {
        if (iMsg.GetID() == SelectNormalGameToC.MESSAGE_ID)
        {
            SelectNormalGameToC msg = (SelectNormalGameToC)iMsg;

            if (msg.m_nResult == 0)
            {
                //  Go to GameRoom
                SceneManager.LoadScene("BaeGameRoom2");
            }
            else
            {
                Debug.LogError("Select NormalGame Fail! m_nResult : " + msg.m_nResult);
            }
        }
    }

    private void OnDestroy()
    {
        if(Network.GetInstance() != null)
            Network.Instance.RemoveRecvMessageHandler(OnRecvMessage);
    }

#region Event Handler
	public void OnSingleGameBtnClicked()
    {
		SceneManager.LoadScene("BaeGameRoom_Single");
    }

	public void OnMultiGameBtnClicked()
    {
        SelectNormalGameToL msgToL = new SelectNormalGameToL ();

        Network.Instance.Send(msgToL);
    }
#endregion
}