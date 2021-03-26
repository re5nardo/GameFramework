using System;
using System.Collections.Generic;

namespace GameFramework
{
    public static class Extension
    {
        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action)
        {
            foreach (T item in enumerable)
            {
                action.Invoke(item);
            }
        }
    }
}
