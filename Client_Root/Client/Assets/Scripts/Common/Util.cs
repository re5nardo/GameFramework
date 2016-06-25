using UnityEngine;
using System.Collections;

namespace Util
{
    public class Math
    {
        public static Vector3 Lerp(Vector3 a, Vector3 b, float t)
        {
            float x = Mathf.Lerp(a.x, b.x, t);
            float y = Mathf.Lerp(a.y, b.y, t);
            float z = Mathf.Lerp(a.z, b.z, t);

            return new Vector3(x, y, z);
        }
    }
}