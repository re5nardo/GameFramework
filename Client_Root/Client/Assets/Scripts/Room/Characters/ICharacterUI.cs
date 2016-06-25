using UnityEngine;
using System.Collections;

public class ICharacterUI : MonoBehaviour
{
    private Transform m_trCharacterUI = null;

    public float PlayAni(string strAniName)
    {
        return 0f;
    }

    public void StopAni()
    {
    }

    //  local pos
    public void SetPosition(Vector3 vec3Pos)
    {
        m_trCharacterUI.localPosition = vec3Pos;
    }
}
