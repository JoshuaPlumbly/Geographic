using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ShapeSettings", menuName = "Settings/ShapeSettings"), System.Serializable]
public class ShapeSettings : ScriptableObject, IShapeSettings
{
    [Range(2,6)] public int numberOfLayers= 2;
    public float radius = 1f;
    public float baseRoughness;
    public float roughness;
    public float persistence;
    public float minValue;
    public Vector3 offset = Vector3.zero;

    public Vector3 CalculatePointOnUnitSphere(Vector3 pointOnUnitSphere)
    {
        float noiseValue = 0f;
        float frequency = baseRoughness;
        float amplitude = 1f;

        for (int i = 0; i < numberOfLayers; i++)
        {
            float v = NoisePerlin.GetValue3D(pointOnUnitSphere + offset, frequency);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= roughness;
            amplitude *= persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - minValue);
        return pointOnUnitSphere * noiseValue * radius;
    }
}