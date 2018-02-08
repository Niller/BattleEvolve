using UnityEngine;

public static class Vector3Extensions
{
    public static Vector3 ConvertToGroundVector(this Vector3 vector3, float groundHeight = 0)
    {
        return new Vector3(vector3.x, groundHeight, vector3.z);
    }
}

