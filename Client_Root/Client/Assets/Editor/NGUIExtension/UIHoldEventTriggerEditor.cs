using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(UIHoldEventTrigger))]
public class UIHoldEventTriggerEditor : Editor
{
    UIHoldEventTrigger mTrigger;

    void OnEnable ()
    {
        mTrigger = target as UIHoldEventTrigger;
        EditorPrefs.SetBool("HET0", EventDelegate.IsValid(mTrigger.onHold));
    }

    public override void OnInspectorGUI ()
    {
        GUILayout.Space(3f);
        NGUIEditorTools.SetLabelWidth(80f);
        bool minimalistic = NGUISettings.minimalisticLook;
        DrawEvents("HET0", "On Hold", mTrigger.onHold, minimalistic);
    }

    void DrawEvents (string key, string text, List<EventDelegate> list, bool minimalistic)
    {
        if (!NGUIEditorTools.DrawHeader(text, key, false, minimalistic)) return;
        NGUIEditorTools.BeginContents();
        EventDelegateEditor.Field(mTrigger, list, null, null, minimalistic);
        NGUIEditorTools.EndContents();
    }
}
