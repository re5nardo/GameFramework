using UnityEngine;

public delegate void DefaultHandler();
public delegate void BoolHandler(bool bValue);
public delegate void Vector3Handler(Vector3 vec3);
public delegate void MessageHandler(IMessage msg);

[System.Serializable]
public struct BehaviorAnimationInfo
{
    public string behaviorKey;
    public string clipName;
}