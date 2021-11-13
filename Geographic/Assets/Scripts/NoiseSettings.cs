using UnityEngine;

[System.Serializable]
public class NoiseSettings
{
    public enum FilterType { Simple, Ridgid}
    public FilterType filterType;
    public float strenth = 1f;
    public float minValue = 1f;
    [Range(1, 8)]
    public int numberOfLayers = 1;
    public float baseRoughness = 1f;
    public float roughness = 2f;
    public float persistence = 0.5f;
    public Vector3 offset = Vector3.zero;
}
