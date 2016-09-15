using UnityEngine;

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
               
            }
            else
            {
                Debug.LogError("Select NormalGame Fail! m_nResult : " + msg.m_nResult);
            }
        }
    }

    private void OnDestroy()
    {
        Network.Instance.RemoveRecvMessageHandler(OnRecvMessage);
    }

    #region Event Handler
    public void OnNormalGameBtnClicked()
    {
        SelectNormalGameToS msgToS = new SelectNormalGameToS ();

        Network.Instance.Send(msgToS);
    }
    #endregion
}
