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
		m_nPressedTick = IGameRoom.Instance.GetCurrentTick();
    }

    public void OnReleased()
    {
		int nCurrentTick = IGameRoom.Instance.GetCurrentTick();

		float fPressedTime = (nCurrentTick - m_nPressedTick) * IGameRoom.Instance.GetTickInterval();

        if (onReleased != null)
        {
            onReleased(fPressedTime);
        }
    }
#endregion
}