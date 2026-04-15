using Unity.Mathematics;
using UnityEngine;

public static class ExtraMath {
    public static Vector3 ToVector3(this float3 f) {
        return new Vector3(f.x, f.y, f.z);
    }

    public static float3 ToFloat3(this Vector3 v) {
        return new float3(v.x, v.y, v.z);
    }
    public static float3 ToFloat3(this Vector2 v) {
        return new float3(v.x, v.y, 0);
    }
}