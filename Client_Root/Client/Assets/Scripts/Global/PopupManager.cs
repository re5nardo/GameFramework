using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : MonoSingleton<PopupManager>
{
	void Start ()
    {
        DontDestroyOnLoad(this);
	}
	
    public PopupBase ShowPopup(string strPopupName)
    {
        GameObject goPopup = ObjectPool.Instance.GetGameObject("Popup/" + strPopupName);
        GameObject goUIRoot = GameObject.Find("UI Root");

        goPopup.transform.SetParent(goUIRoot.transform);
        goPopup.transform.localScale = Vector3.one;

        PopupBase popup = goPopup.GetComponent<PopupBase>();

        popup.Show();

        return popup;
    }
}
