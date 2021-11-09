using UnityEngine;

public class ShapeSettings : ScriptableObject
{
    public float radius = 1f;
    public NoiseLayer[] noiseLayers;

    [System.Serializable]
    public class NoiseLayer
    {
        public bool enabled = true;
        public bool useFirstLayerAsMask;
        public NoiseSettings noiseSettings;
    }
}

[System.Serializable]
public class NoiseSettings
{
    public float strenth = 1f;
    public float minValue = 1f;
    [Range(1, 8)]
    public int numberOfLayers = 1;
    public float baseRoughness = 1f;
    public float roughness = 2f;
    public float persistence = 0.5f;
    public Vector3 offset = Vector3.zero;
}