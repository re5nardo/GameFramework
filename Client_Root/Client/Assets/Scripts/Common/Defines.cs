using UnityEngine;

public delegate void DefaultHandler();
public delegate void StringHandler(string str);
public delegate void BoolHandler(bool bValue);
public delegate void IntHandler(int str);
public delegate void Vector2Handler(Vector2 vec2);
public delegate void Vector3Handler(Vector3 vec3);
public delegate void MessageHandler(IMessage msg);

[System.Serializable]
public struct BehaviorAnimationInfo
{
    public string behaviorKey;
    public string clipName;
}


public class GameObjectLayer
{
    public const int CHARACTER = 9;
}