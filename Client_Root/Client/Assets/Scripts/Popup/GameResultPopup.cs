using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameResultPopup : PopupBase
{
    [SerializeField] UILabel[] m_RankInfos = null;
    [SerializeField] BasicButton m_BtnGoToLobby = null;

    private void Awake()
    {
        m_BtnGoToLobby.onClicked = OnGoToLobbyBtnClicked;
    }

    private void OnDestroy()
    {
        m_BtnGoToLobby.onClicked = null;
    }

    public void SetData(List<PlayerRankInfo> listPlayerRankInfo)
    {
        for (int i = 0; i < m_RankInfos.Length; ++i)
        {
            if (i < listPlayerRankInfo.Count)
            {
                m_RankInfos[i].text = string.Format("{0}위 PlayerIndex_{1} {2}m", listPlayerRankInfo[i].m_nRank, listPlayerRankInfo[i].m_nPlayerIndex, listPlayerRankInfo[i].m_fHeight);
            }
            else
            {
                m_RankInfos[i].gameObject.SetActive(false);
            }
        }
    }

#region Event Handler
    private void OnGoToLobbyBtnClicked()
    {
        //  Go to Lobby
        SceneManager.LoadScene("Lobby"); 
    }
#endregion
}