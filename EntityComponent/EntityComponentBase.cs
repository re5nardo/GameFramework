using UnityEngine;
using System.Collections.Generic;
using System;

namespace GameFramework
{
    public class EntityComponentBase : IEntityComponent
    {
        public IEntity Entity { get; private set; }

        public void Attach(IEntity entity)
        {
            Entity = entity;

            OnAttached(entity);
        }

        public void Detach()
        {
            OnDetached();

            Entity = null;
        }

        protected virtual void OnAttached(IEntity entity) { }
        protected virtual void OnDetached() { }
    }
}
