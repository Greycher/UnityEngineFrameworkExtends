using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtendedMath
{
    public static float DistanceSquare(Vector3 a, Vector3 b)
    {
        var x = b.x - a.x;
        var y = b.y - a.y;
        return x * x + y * y;
    }
    
    public static float Normalize(float value, float minValue, float maxValue, float minNormalizedValue = 0, float maxNormalizedValue = 1)
    {
        return Mathf.Clamp(NormalizeUnclamped(value, maxValue, maxValue, minNormalizedValue, maxNormalizedValue), minNormalizedValue, maxNormalizedValue);
    }

    public static float NormalizeUnclamped(float value, float minValue, float maxValue, float minNormalizedValue = 0, float maxNormalizedValue = 1)
    {
        return (value - minValue) / (maxValue - minValue) * (maxNormalizedValue - minNormalizedValue) + minNormalizedValue;
    }

    public static Vector3 GetPointedWorldPosition(Vector2 screenPos, float distance)
    {
        var camera = Camera.main;
        return GetPointedWorldPosition(screenPos, camera, distance);
    }
    
    public static Vector3 GetPointedWorldPosition(Vector3 screenPos, Camera camera, float distance)
    {
        var ray = camera.ScreenPointToRay(screenPos);
        var plane = new Plane(camera.transform.forward, distance);
        plane.Raycast(ray, out float rayDistance);
        return ray.GetPoint(rayDistance);
    }
}
