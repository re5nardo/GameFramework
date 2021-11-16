using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using GameFramework;

namespace Entity
{
	public class MonoEntityBase : MonoBehaviour, IEntity
	{
        private List<IEntityComponent> entityComponents = new List<IEntityComponent>();
       
        public int EntityID { get; protected set; } = -1;

        public virtual Vector3 Position { get; set; }
        public virtual Vector3 Rotation { get; set; }
        public virtual Vector3 Velocity { get; set; }
        public virtual Vector3 AngularVelocity { get; set; }

        public IEntityComponent AttachEntityComponent(string typeName)
        {
            return AttachEntityComponent(gameObject.AddComponent(Type.GetType(typeName)) as IEntityComponent);
        }

        public T AttachEntityComponent<T>() where T : Component, IEntityComponent
        {
            return AttachEntityComponent(gameObject.AddComponent<T>());
        }

        public T AttachEntityComponent<T>(T entityComponent) where T : IEntityComponent
        {
            entityComponents.Add(entityComponent);

            entityComponent.Attach(this);

            return entityComponent;
        }

        public T DetachEntityComponent<T>(T entityComponent) where T : IEntityComponent
        {
            entityComponents.Remove(entityComponent);

            entityComponent.Detach();

            return entityComponent;
        }

        public T GetEntityComponent<T>() where T : IEntityComponent
        {
            var found = entityComponents.Find(x => x is T);

            if (found == null)
                return default;

            return (T)found;
        }

        public List<T> GetEntityComponents<T>() where T : IEntityComponent
        {
            var found = entityComponents.FindAll(x => x is T);

            if (found == null)
                return null;

            return found.Cast<T>().ToList();
        }
    }
}
