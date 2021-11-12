using UnityEngine;

[System.Serializable]
public class WorldSettings
{
    public Biome[] biomes;
    public NoiseSettings noise;
    public float noiseOffset;
    public float noiseSrength;
    [Range(0,1)] public float blendAmount;
}