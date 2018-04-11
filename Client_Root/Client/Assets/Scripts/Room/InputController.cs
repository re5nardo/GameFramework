using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    [HideInInspector] public FloatHandler onReleased;

    private int m_nPressedTick = 0;

#region Event Handler
    public void OnPressed()
    {
        m_nPressedTick = BaeGameRoom2.Instance.GetCurrentTick();
    }

    public void OnReleased()
    {
        int nCurrentTick = BaeGameRoom2.Instance.GetCurrentTick();

        float fPressedTime = (nCurrentTick - m_nPressedTick) * BaeGameRoom2.Instance.GetTickInterval();

        if (onReleased != null)
        {
            onReleased(fPressedTime);
        }
    }
#endregion
}