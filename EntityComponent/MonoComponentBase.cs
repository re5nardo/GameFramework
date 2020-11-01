using UnityEngine;
using System.Collections.Generic;
using System;

namespace GameFramework
{
    public class MonoComponentBase : MonoBehaviour, IComponent
    {
        public IEntity Entity { get; private set; }

        private SimplePubSubService<Type, ICommand> commandHandler = new SimplePubSubService<Type, ICommand>();

        public void OnCommand(ICommand command)
        {
            commandHandler.Publish(command.GetType(), command);
        }

        public virtual void OnAttached(IEntity entity)
        {
            Entity = entity;
        }

        public virtual void OnDetached()
        {
            Entity = null;
        }

        protected void AddCommandHandler(Type type, Action<ICommand> handler)
        {
            commandHandler.AddSubscriber(type, handler);
        }

        protected void RemoveCommandHandler(Type type, Action<ICommand> handler)
        {
            commandHandler.RemoveSubscriber(type, handler);
        }

        public virtual void Initialize(params object[] param)
        {
        }
    }
}
