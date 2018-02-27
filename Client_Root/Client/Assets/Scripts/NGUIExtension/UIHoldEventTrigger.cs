using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIHoldEventTrigger : MonoBehaviour
{
    static public UIHoldEventTrigger current;

    public List<EventDelegate> onHold = new List<EventDelegate>();
    public List<EventDelegate> onPress = new List<EventDelegate>();
    public List<EventDelegate> onRelease = new List<EventDelegate>();

    public int touchID;

    private void OnPress(bool bPressed)
    {
        if (current != null) return;

        current = this;

        if (bPressed)
        {
            touchID = UICamera.currentTouchID;

            EventDelegate.Execute(onPress);

            StartCoroutine("ProcessHold");
        }
        else
        {
            EventDelegate.Execute(onRelease);

            StopCoroutine("ProcessHold");
        }

        current = null;
    }

    private IEnumerator ProcessHold()
    {
        while (true)
        {
            EventDelegate.Execute(onHold);

            yield return null;
        }
    }
}
