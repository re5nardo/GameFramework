using System.Collections.Generic;
using System;

namespace GameFramework
{
    public interface IDataSource
    {
        List<Type> DataHandlerTypeList { get; }
    }
}
