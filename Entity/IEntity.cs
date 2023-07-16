using UnityEngine;
using System.Collections.Generic;

namespace GameFramework
{
    public interface IEntity
    {
        int EntityId { get; }
        Vector3 Position { get; set; }
        Vector3 Rotation { get; set; }
        Vector3 Velocity { get; set; }
        Vector3 AngularVelocity { get; set; }

        T AttachEntityComponent<T>(T entityComponent) where T : IEntityComponent;
        T DetachEntityComponent<T>(T entityComponent) where T : IEntityComponent;

        T GetEntityComponent<T>() where T : IEntityComponent;
        List<T> GetEntityComponents<T>() where T : IEntityComponent;
    }
}
