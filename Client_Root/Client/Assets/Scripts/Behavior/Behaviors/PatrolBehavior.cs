using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PatrolBehavior : IBehavior
{
    private ICharacter                  m_Character = null;
    private Vector3                     m_vec3Start = Vector3.zero;
    private Vector3                     m_vec3Dest = Vector3.zero;
    private string                      m_strMoveClipName = "";

    public PatrolBehavior(ICharacter character, Vector3 vec3Start, Vector3 vec3Dest, string strMoveClipName) : base(character)
    {
        m_Character = character;
        m_vec3Start = vec3Start;
        m_vec3Dest = vec3Dest;
        m_strMoveClipName = strMoveClipName;
    }

    protected override IEnumerator Body()
    {
        bool bGoToDest = true;
        bool bFirstMove = true;
        LinkedList<Node> listPath = null;

        m_Character.SetPosition(m_vec3Start);

        while (true)
        {
            if (bGoToDest)
            {
                listPath = IGameRoom.Instance.GetMovePath(m_vec3Start, m_vec3Dest);
            }
            else
            {
                listPath = IGameRoom.Instance.GetMovePath(m_vec3Dest, m_vec3Start);
            }

            if (listPath == null)
            {
                Debug.LogError("Position is invalid!, m_vec3Start : " + m_vec3Start + ", m_vec3Dest : " + m_vec3Dest);
                yield break;
            }

            m_SubCoroutine = new MoveBehavior(m_Character, listPath, m_strMoveClipName, !bFirstMove, IGameRoom.Instance.GetElapsedTime()).Start();

            yield return m_SubCoroutine;

            bGoToDest = !bGoToDest;
            bFirstMove = false;
        }
    }
}
