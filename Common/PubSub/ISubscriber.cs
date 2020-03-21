using System;

namespace GameFramework
{
    public interface ISubscriber
    {
        void OnMessage(Enum key, params object[] param);
    }
}
