using System.Collections.Generic;
using UnityEngine;
using System;

namespace GameFramework
{
    public class MonoDataContainerImpl : MonoBehaviour, IDataContainer
    {
        public List<IDataComponent> DataComponentList { get; } = new List<IDataComponent>();

        public T Get<T>() where T : IDataComponent
        {
            var target = DataComponentList.Find(dataComponent => dataComponent.GetType() == typeof(T));
            if (target == null)
            {
                if (typeof(T).IsSubclassOf(typeof(MonoBehaviour)))
                {
                    target = gameObject.GetOrAddComponent(typeof(T)) as IDataComponent;
                }
                else
                {
                    target = (IDataComponent)Activator.CreateInstance(typeof(T));
                }

                DataComponentList.Add(target);
            }

            return (T)target;
        }

        public IDataComponent Get(Type type)
        {
            var target = DataComponentList.Find(dataComponent => dataComponent.GetType() == type);
            if (target == null)
            {
                if (type.IsSubclassOf(typeof(MonoBehaviour)))
                {
                    target = gameObject.GetOrAddComponent(type) as IDataComponent;
                }
                else
                {
                    target = (IDataComponent)Activator.CreateInstance(type);
                }

                DataComponentList.Add(target);
            }

            return target;
        }

        public void OnUpdate(IDataSource source)
        {
            source.DataHandlerTypeList?.ForEach(dataHandlerType =>
            {
                var dataComponent = Get(dataHandlerType.GetType());

                dataComponent.OnUpdate(source);
            });
        }
    }
}
