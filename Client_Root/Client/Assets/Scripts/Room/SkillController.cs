using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillController : MonoBehaviour
{
    private const int       SKILL_BUTTON_COUNT_MAX = 4;
    private const float     SKILL_BUTTON_INTERVAL = 170;

    public IntHandler onSkillButtonClicked = null;

    private List<SkillButton> m_listSkillButton = new List<SkillButton>();
    private Transform m_trSkillController = null;

    private void Awake()
    {
        m_trSkillController = transform;
    }

    public void SetSkills(List<int> listSkillID)
    {
        if (listSkillID.Count > SKILL_BUTTON_COUNT_MAX)
        {
            Debug.LogError("Count of SkillID is bigger than SKILL_BUTTON_COUNT_MAX!, SkillID Count : " + listSkillID.Count);
            return;
        }

        int nIndex = 0;
        for (; nIndex < m_listSkillButton.Count; ++nIndex)
        {
            if (nIndex < listSkillID.Count)
            {
                m_listSkillButton[nIndex].transform.localPosition = new Vector3(SKILL_BUTTON_INTERVAL * nIndex, 0, 0);
                m_listSkillButton[nIndex].Show();
                m_listSkillButton[nIndex].SetData(listSkillID[nIndex]);
            }
            else
            {
                m_listSkillButton[nIndex].Hide();
            }
        }

        for (; nIndex < listSkillID.Count; ++nIndex)
        {
            SkillButton skillButton = CreateSkillButton();
            skillButton.transform.localPosition = new Vector3(SKILL_BUTTON_INTERVAL * nIndex, 0, 0);
            skillButton.SetData(listSkillID[nIndex]);
        }
    }

    private SkillButton CreateSkillButton()
    {
        SkillButton skillButton = Instantiate(Resources.Load<GameObject>("UI/Skill Button")).GetComponent<SkillButton>();
        skillButton.transform.parent = transform;
        skillButton.transform.localPosition = Vector3.zero;
        skillButton.transform.localRotation = Quaternion.identity;
        skillButton.transform.localScale = Vector3.one;
        skillButton.onClicked += OnSkillButtonClicked;

        m_listSkillButton.Add(skillButton);

        return skillButton;
    }

    private void OnDestroy()
    {
        foreach(SkillButton skillButton in m_listSkillButton)
        {
            skillButton.onClicked -= OnSkillButtonClicked;
            Destroy(skillButton.gameObject);
        }
        m_listSkillButton.Clear();
    }

#region Event Handler
    private void OnSkillButtonClicked(int nSkillID)
    {
        if (onSkillButtonClicked != null)
            onSkillButtonClicked(nSkillID);

        GameInputSkillToR skillToR = new GameInputSkillToR();
        skillToR.m_nPlayerIndex = 0;
        skillToR.m_nSkillID = nSkillID;

        RoomNetwork.Instance.Send(skillToR);
    }
#endregion
}
