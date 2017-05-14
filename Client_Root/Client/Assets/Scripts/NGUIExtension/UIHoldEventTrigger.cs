using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIHoldEventTrigger : MonoBehaviour
{
    static public UIHoldEventTrigger current;

    public List<EventDelegate> onHold = new List<EventDelegate>();

    private void OnPress(bool bPressed)
    {
        if (current != null) return;

        current = this;

        if (bPressed)
        {
            StartCoroutine("ProcessHold");
        }
        else
        {
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
