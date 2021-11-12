using UnityEngine;

[System.Serializable]
public class Biome
{
    public Gradient gradient;
    [Range(0, 1)] public float startHeight;
    public Color tintColour;
    [Range(0, 1)] public float tintDecimal;
}
