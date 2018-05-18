using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SingleGameResultPopup : PopupBase
{
	[SerializeField] private UILabel m_lbText = null;
	[SerializeField] private BasicButton m_BtnRetry = null;
	[SerializeField] private BasicButton m_BtnGoToLobby = null;

    private void Awake()
    {
		m_BtnRetry.onClicked = OnRetryBtnClicked;
        m_BtnGoToLobby.onClicked = OnGoToLobbyBtnClicked;
    }

    private void OnDestroy()
    {
		m_BtnRetry.onClicked = null;
        m_BtnGoToLobby.onClicked = null;
    }

	public void SetData(float fCurrentHeight, float fBestHeight)
    {
    	string strGrade = "";

		if(fCurrentHeight >= 1000)
		{
			strGrade = "당신의 피지컬은 페이커급 입니다.";
		}
		else if(fCurrentHeight >= 800)
		{
			strGrade = "당신의 피지컬은 상위 10% 입니다.";
		}
		else if(fCurrentHeight >= 600)
		{
			strGrade = "당신의 피지컬은 상위 35% 입니다.";
		}
		else if(fCurrentHeight >= 400)
		{
			strGrade = "당신의 피지컬은 하위 60% 입니다.";
		}
		else if(fCurrentHeight >= 200)
		{
			strGrade = "당신의 피지컬은 하위 80% 입니다.";
		}
		else
		{
			strGrade = "당신의 피지컬은 하위 98% 입니다.";
		}

		m_lbText.text = string.Format("[000000][ff0000]{0}[-]\n\n최종 높이 : {1}m\n\n최고 높이 : {2}m[-]", strGrade, fCurrentHeight, fBestHeight);
    }

#region Event Handler
	private void OnRetryBtnClicked()
    {
		Hide();

        //  Retry
        SceneManager.LoadScene("BaeGameRoom_Single"); 
    }

    private void OnGoToLobbyBtnClicked()
    {
    	Hide();

        //  Go to Lobby
        SceneManager.LoadScene("Lobby"); 
    }
#endregion
}