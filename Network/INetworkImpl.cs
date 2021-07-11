using UnityEngine;
using System;

namespace GameFramework
{
    public interface INetworkImpl
    {
        Action<IMessage> OnMessage { get; set; }

        void Send(IMessage msg, int targetId, bool reliable = true, bool instant = false);
        void Send(IMessage msg, ulong targetId, bool reliable = true, bool instant = false);
        void SendToAll(IMessage msg, bool reliable = true, bool instant = false);
        void SendToNear(IMessage msg, Vector3 center, float radius, bool reliable = true, bool instant = false);
    }
}
