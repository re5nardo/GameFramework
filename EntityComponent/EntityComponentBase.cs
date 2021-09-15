using UnityEngine;
using System.Collections.Generic;
using System;

namespace GameFramework
{
    public class EntityComponentBase : IEntityComponent
    {
        public IEntity Entity { get; private set; }

        private Dictionary<Type, Action<ICommand>> commandHandlers = new Dictionary<Type, Action<ICommand>>();

        public virtual void OnCommand(ICommand command)
        {
            if (commandHandlers.TryGetValue(command.GetType(), out var handler))
            {
                handler?.Invoke(command);
            }
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
            if (commandHandlers.ContainsKey(type))
            {
                Debug.LogWarning("There is already handler! type : " + type);
                return;
            }

            commandHandlers.Add(type, handler);
        }

        protected void RemoveCommandHandler(Type type)
        {
            if (!commandHandlers.ContainsKey(type))
            {
                Debug.LogWarning("There is no handler! type : " + type);
                return;
            }

            commandHandlers.Remove(type);
        }
    }
}
