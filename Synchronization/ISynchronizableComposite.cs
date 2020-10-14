using System.Collections.Generic;

namespace GameFramework
{
    public interface ISynchronizableComposite : ISynchronizable
    {
        List<ISynchronizable> Children { get; }

        void Add(ISynchronizable child);
        void Remove(ISynchronizable child);
    }
}
