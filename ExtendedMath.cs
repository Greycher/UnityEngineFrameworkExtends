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
        return (value - minValue) / (maxValue - minValue) * (maxNormalizedValue - minNormalizedValue) + minNormalizedValue;
    }
}
