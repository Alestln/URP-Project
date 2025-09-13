using UnityEngine;

public static class TransformHelper
{
    public static void UpdateRotation(Transform transform, Vector2 diretion)
    {
        if (diretion != Vector2.zero)
        {
            float angle = Vector2.SignedAngle(Vector2.up, diretion);
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }
    }
}