using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings _settings;
    NoiseFilter[] _noiseFilter;

    public ShapeGenerator(ShapeSettings settings)
    {
        _settings = settings;
        _noiseFilter = new NoiseFilter[settings.noiseLayers.Length];

        for (int i = 0; i < _noiseFilter.Length; i++)
        {
            _noiseFilter[i] = new NoiseFilter(settings.noiseLayers[i].noiseSettings);
        }
    }

    public Vector3 CalculatePointFrom(Vector3 pointOnUnitSphere)
    {
        float elevation = 0;

        float layerValue = 0;

        if (_noiseFilter.Length > 0)
        {
            layerValue = _noiseFilter[0].Evaluate(pointOnUnitSphere);

            if (_settings.noiseLayers[0].enabled)
                elevation = layerValue;
        }

        for (int i = 1; i < _noiseFilter.Length; i++)
        {
            if (!_settings.noiseLayers[i].enabled)
                continue;

            float mask = _settings.noiseLayers[i].useFirstLayerAsMask ? layerValue : 1;
            elevation += _noiseFilter[i].Evaluate(pointOnUnitSphere) * mask;
        }

        return pointOnUnitSphere * _settings.radius * (1 + elevation);
    }
}