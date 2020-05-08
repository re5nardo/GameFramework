using UnityEngine;
using System;

namespace GameFramework
{
    public interface INetworkImpl
    {
        Action<IMessage, object[]> onMessage { get; set; }

        void Send(IMessage msg, int nTargetID, bool bReliable = true, bool bInstant = false);
        void SendToAll(IMessage msg, bool bReliable = true, bool bInstant = false);
        void SendToNear(IMessage msg, Vector3 vec3Center, float fRadius, bool bReliable = true, bool bInstant = false);
    }
}
