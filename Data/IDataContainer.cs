using System.Collections.Generic;

namespace GameFramework
{
    public interface IDataContainer
    {
        List<IDataComponent> DataComponentList { get; }

        T Get<T>() where T : IDataComponent;
        void OnUpdate(IDataSource source);
    }
}
