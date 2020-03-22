using UnityEngine;
using System.Collections.Generic;
using System;

namespace GameFramework
{
    public interface IEntity : ITickable
    {
        int EntityID { get; }
        Vector3 Position { get; set; }
        Vector3 Rotation { get; set; }
        Vector3 Velocity { get; set; }
        Vector3 AngularVelocity { get; set; }

        T AttachComponent<T>(T component) where T : IComponent;
        T DetachComponent<T>(T component) where T : IComponent;

        T GetComponent<T>() where T : IComponent;
        List<T> GetComponents<T>() where T : IComponent;

        void SendCommandToAll(ICommand command);
        void SendCommand(ICommand command, List<Type> cullings);
    }
}
