using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShapeSettings", menuName = "Settings/ShapeSettings")]
public class ShapeSettings : ScriptableObject, IShapeSettings
{
    public float radius;

    public Vector3 CalculatePoint(Vector3 pointOnUnitSphere)
    {
        return pointOnUnitSphere * radius;
    }
}