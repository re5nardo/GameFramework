using UnityEngine;
using System.Collections;

public abstract class IBehaviorData
{
    public double m_dStartTime;

    public abstract ushort GetID ();
}

public class MoveBehaviorData : IBehaviorData
{
    public override ushort GetID()
    {
        return TestMessage.MESSAGE_ID;
    }

    public Vector3 m_vec3Start;
    public Vector3 m_vec3Dest;

}
