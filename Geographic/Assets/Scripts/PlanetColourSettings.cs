using UnityEngine;

[CreateAssetMenu(fileName = "PlanetColourSettings", menuName = "Settings/PlanetColourSettings")]
public class PlanetColourSettings : ScriptableObject
{
    public Material material;
    public WorldSettings worldSettings;
}
