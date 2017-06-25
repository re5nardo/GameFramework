using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillButton : MonoBehaviour
{
    [SerializeField] private UITexture m_texThumbnail = null;
    [SerializeField] private UISprite m_sprBlackCover = null;
    [SerializeField] private UILabel m_lbName = null;               //  Temp

    public IntHandler onClicked;
    public IntHandler onPressed;
    public IntHandler onReleased;

    private int m_nSkillID = -1;

    private MasterData.Skill m_MasterData_Skill = null;

    public void SetData(int nSkillID/*, object player*/)
    {
        m_nSkillID = nSkillID;

        if (nSkillID == -1)
        {
            //  Show empty button
            return;
        }

        m_lbName.text = m_nSkillID.ToString();

        MasterDataManager.Instance.GetData(nSkillID, ref m_MasterData_Skill);
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
        if (m_MasterData_Skill.m_strClassName != "FireSkill")
            return;

        if (onClicked != null)
            onClicked(m_nSkillID);
    }

    public void OnButtonPressed()
    {
        if (m_MasterData_Skill.m_strClassName != "ContinueSkill")
            return;

        if (onPressed != null)
            onPressed(m_nSkillID);
    }

    public void OnButtonReleased()
    {
        if (m_MasterData_Skill.m_strClassName != "ContinueSkill")
            return;

        if (onReleased != null)
            onReleased(m_nSkillID);
    }
#endregion
}
