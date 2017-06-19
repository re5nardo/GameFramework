using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private UITexture m_texThumbnail = null;
    [SerializeField] private UISprite m_sprBlackCover = null;
    [SerializeField] private UILabel m_lbName = null;               //  Temp

    public IntHandler onClicked;

    private int m_nSkillID = -1;

    public void SetData(int nSkillID/*, object player*/)
    {
        m_nSkillID = nSkillID;

        if (nSkillID == -1)
        {
            //  Show empty button
            return;
        }

        m_lbName.text = m_nSkillID.ToString();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

	private void Update ()
    {
		
	}

#region Event Handler
    public void OnButtonClicked()
    {
        if (onClicked != null)
            onClicked(m_nSkillID);
    }
#endregion
}
