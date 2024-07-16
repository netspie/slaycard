using UnityEngine;

namespace Core.Unity.Math
{
    public static class VectorExtensions
    {
        public static Vector2 AddX(this Vector2 vector, float value) =>
            new Vector2(vector.x + value, vector.y);

        public static Vector2 AddY(this Vector2 vector, float value) =>
            new Vector2(vector.x, vector.y + value);

        public static Vector3 AddX(this Vector3 vector, float value) =>
            new Vector3(vector.x + value, vector.y, vector.z);

        public static Vector3 AddY(this Vector3 vector, float value) =>
            new Vector3(vector.x, vector.y + value, vector.z);

        public static Vector3 WithX(this Vector3 vector, float value) =>
            new Vector3(value, vector.y, vector.z);

        public static Vector3 WithY(this Vector3 vector, float value) =>
            new Vector3(vector.x, value, vector.z);
        
        public static Vector3 WithZ(this Vector3 vector, float value) =>
            new Vector3(vector.x, vector.y, value);

        public static Vector3 Multiply(this Vector3 source, Vector3 other) =>
            new Vector3(
                source.x * other.x,
                source.y * other.y,
                source.z * other.z);

        public static Vector2 ToVector2(this Vector3 vector) => vector;
    }

    public static class Vector3Extensions
    {
        public static bool IsApproximately(this Vector3 v1, Vector3 v2, float tolerance = 0.01f) =>
            Mathf.Abs(v1.x - v2.x) < tolerance &&
            Mathf.Abs(v1.y - v2.y) < tolerance &&
            Mathf.Abs(v1.z - v2.z) < tolerance;

        public static bool IsApproximately(this float v1, float v2, float tolerance = 0.01f) =>
            Mathf.Abs(v1 - v2) < tolerance;
    }
}
