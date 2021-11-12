﻿using UnityEngine;

[CreateAssetMenu(fileName = "PlanetColourSettings", menuName = "Settings/PlanetColourSettings")]
public class PlanetColourSettings : ScriptableObject
{
    public Gradient gradient;
    public Material material;
}
