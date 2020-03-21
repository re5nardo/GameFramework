using System.Collections.Generic;
using System;

namespace GameFramework
{
    public class Util
    {
        public static bool IsSatisfy<T>(T target, List<Predicate<T>> conditions)
        {
            foreach (var condition in conditions)
            {
                if (condition(target) == false)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
