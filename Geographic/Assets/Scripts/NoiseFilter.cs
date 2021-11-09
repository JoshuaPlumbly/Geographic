using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoiseFilter
{
    private NoiseSettings _settings;

    public NoiseFilter(NoiseSettings noiseSettings)
    {
        _settings = noiseSettings;
    }

    public float Evaluate(Vector3 point)
    {
        float noiseValue = 0;
        float frequency = _settings.baseRoughness;
        float amplitude = 1;

        for (int i = 0; i < _settings.numberOfLayers; i++)
        {
            float v = PerlinNoise.GetValue3D(point + _settings.offset, frequency);
            noiseValue += (v + 1) * 0.5f * amplitude;
            frequency *= _settings.roughness;
            amplitude *= _settings.persistence;
        }

        noiseValue = Mathf.Max(0, noiseValue - _settings.minValue);
        return noiseValue * _settings.strenth;
    }
}