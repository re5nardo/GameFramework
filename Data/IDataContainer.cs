using System.Collections.Generic;
using System;

namespace GameFramework
{
    public interface IDataContainer
    {
        List<IDataComponent> DataComponentList { get; }

        T Get<T>() where T : IDataComponent;
        IDataComponent Get(Type type);

        void OnUpdate(IDataSource source);
    }
}
