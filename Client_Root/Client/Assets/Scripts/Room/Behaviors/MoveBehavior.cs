using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoveBehavior : IBehavior
{
    private ICharacter                  m_Character = null;
    private List<Node>                  m_listPath = null;
    private Dictionary<int, float>      m_dicDistance = new Dictionary<int, float>();   //  Node index and accumulated distance
    private float                       m_fDistanceToMove = 0f;
    private string                      m_strMoveClipName = "";
    private bool                        m_bContinue = false;

    public MoveBehavior(ICharacter Character, LinkedList<Node> listPath, string strMoveClipName, bool bContinue) : base(Character)
    {
        m_Character = Character;
        m_listPath = new List<Node>(listPath);

        for (int nIndex = 0; nIndex < m_listPath.Count; ++nIndex)
        {
            if (nIndex == 0)
            {
                m_dicDistance.Add(nIndex, 0f);
            }
            else
            {
                m_dicDistance.Add(nIndex, m_dicDistance[nIndex - 1] + Vector3.Distance(m_listPath[nIndex - 1].m_vec3Pos, m_listPath[nIndex].m_vec3Pos));
            }
        }

        m_fDistanceToMove = m_dicDistance[m_listPath.Count - 1];

        m_strMoveClipName = strMoveClipName;
        m_bContinue = bContinue;
    }

    protected override IEnumerator Body()
    {
        float fClipLength = m_Character.m_CharacterUI.GetAnimationClipLegth(m_strMoveClipName);
        float fElapsedTime = 0f;
        float fContinueTime = m_bContinue ? m_Character.m_CharacterUI.GetAnimationStateTime(m_strMoveClipName) : 0f;

        float fMovedDistance = 0f;
        int nPrev = 0;
        int nNext = 1;

        while (true)
        {
            m_Character.m_CharacterUI.SampleAnimation(m_strMoveClipName, ((fElapsedTime + fContinueTime) % fClipLength) / fClipLength);
            m_Character.m_CharacterUI.transform.LookAt(m_listPath[nNext].m_vec3Pos);

            if (fMovedDistance >= m_fDistanceToMove)
            {
                break;
            }

            while (fMovedDistance >= m_dicDistance[nNext])
            {
                ++nPrev;
                ++nNext;
            }

            float t = (fMovedDistance - m_dicDistance[nPrev]) / (m_dicDistance[nNext] - m_dicDistance[nPrev]);

            m_Character.SetPosition(Util.Math.Lerp(m_listPath[nPrev].m_vec3Pos, m_listPath[nNext].m_vec3Pos, t));

            yield return null;

            fElapsedTime += Time.deltaTime;
            fMovedDistance += m_Character.GetSpeed() * Time.deltaTime;
        }

        m_Character.SetPosition(m_listPath[m_listPath.Count - 1].m_vec3Pos);
    }
}
