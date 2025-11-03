using UnityEngine;

namespace OfficeFold.Utils
{
    public static class Vector3Extensions
    {
        public static Vector3 Add(this Vector3 v, float x = 0, float y = 0, float z = 0)
        {
            return new Vector3(v.x + x, v.y + y, v.z + z);
        }
    }
}