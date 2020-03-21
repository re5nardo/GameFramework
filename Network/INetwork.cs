using UnityEngine;
using System;

namespace GameFramework
{
    public interface INetwork
    {
        INetworkImpl m_NetworkImpl { get; set; }
        Action<IMessage, object[]> onMessage { get; set; }

        void Send(IMessage msg, int nTargetUserID, bool bReliable = true, bool bInstant = false);
        void SendToAll(IMessage msg, bool bReliable = true, bool bInstant = false);
        void SendToNear(IMessage msg, Vector3 vec3Center, float fRadius, bool bReliable = true, bool bInstant = false);
    }
}
