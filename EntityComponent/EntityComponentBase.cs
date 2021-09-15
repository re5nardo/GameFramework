using UnityEngine;
using System.Collections.Generic;
using System;

namespace GameFramework
{
    public class EntityComponentBase : IEntityComponent
    {
        public IEntity Entity { get; private set; }

        public virtual void OnAttached(IEntity entity)
        {
            Entity = entity;
        }

        public virtual void OnDetached()
        {
            Entity = null;
        }
    }
}
