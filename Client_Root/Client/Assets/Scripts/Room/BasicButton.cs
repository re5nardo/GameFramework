using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicButton : MonoBehaviour
{
    [HideInInspector] public DefaultHandler onClicked;

#region Event Handler
    public void OnButtonClicked()
    {
        if (onClicked != null)
        {
            onClicked();
        }
    }
#endregion
}