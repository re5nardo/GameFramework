using UnityEngine;

public delegate void DefaultHandler();
public delegate void StringHandler(string str);
public delegate void BoolHandler(bool bValue);
public delegate void IntHandler(int n);
public delegate void FloatHandler(float f);
public delegate void Vector2Handler(Vector2 vec2);
public delegate void Vector3Handler(Vector3 vec3);
public delegate void MessageHandler(IMessage msg);
public delegate void CollisionHandler(Collision collision);
public delegate void ColliderHandler(Collider collider);
public delegate void GameItemHandler(GameItem gameItem);

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

public enum CoreState
{
    CoreState_Invincible = 0,
    CoreState_ChallengerDisturbing,
    CoreState_Faint,
};

public enum GameType
{
	GameType_Single = 0,
	GameType_Multi,
};