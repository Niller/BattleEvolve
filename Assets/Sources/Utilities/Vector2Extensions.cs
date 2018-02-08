using UnityEngine;

public static class Vector2Extensions
{
    public static Vector3 ConvertToGroundVector3(this Vector2 vector2)
    {
        return new Vector3(vector2.x, 0, vector2.y);
    }
}

