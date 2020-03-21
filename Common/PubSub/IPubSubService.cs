using System;

namespace GameFramework
{
    public interface IPubSubService
    {
        void Publish(Enum key, params object[] param);
        void AddSubscriber(Enum key, ISubscriber subscriber);
        void RemoveSubscriber(Enum key, ISubscriber subscriber);
    }
}
