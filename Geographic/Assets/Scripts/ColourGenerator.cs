using UnityEngine;

public class ColourGenerator
{
    PlanetColourSettings _settings;
    Texture2D _texture;
    const int _textureResolution = 50;
    NoiseFilter _biomdNoiseFilter;

    public void UpdateSettings(PlanetColourSettings settings)
    {
        this._settings = settings;
        _biomdNoiseFilter = new NoiseFilter(_settings.worldSettings.noise);

        if (_texture == null || _texture.height != _settings.worldSettings.biomes.Length)
        {
            _texture = new Texture2D(_textureResolution, _settings.worldSettings.biomes.Length, TextureFormat.RGBA32, false);
        }
    }

    public void UpdateElevation(MinMaxF elevationMinMax)
    {
        _settings.material.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public void UpdateColour()
    {
        Color[] colours = new Color[_texture.width * _texture.height];

        int i = 0;

        foreach(Biome biome in _settings.worldSettings.biomes)
        {
            Gradient gradient = biome.gradient;
            Color tintColour = biome.tintColour;
            float tintDecimal = biome.tintDecimal;

            for (int j = 0; j < _textureResolution; j++)
            {
                Color gradientColour = gradient.Evaluate(j / (_textureResolution - 1f));
                colours[i] = gradientColour * (1 - tintDecimal) + tintColour * tintDecimal;
                i++;
            }
        }



        _texture.SetPixels(colours);
        _texture.Apply();
        _settings.material.SetTexture("_texture", _texture);
    }

    public float BimeDecimalFromPoint(Vector3 pointOnUnitSphere)
    {
        WorldSettings worldSettings = _settings.worldSettings;
        Biome[] biomes = worldSettings.biomes;


        float heightDecimal = (pointOnUnitSphere.y + 1) / 2f + 0.0001f;
        heightDecimal += (_biomdNoiseFilter.Evaluate(pointOnUnitSphere) - worldSettings.noiseOffset) * worldSettings.noiseSrength;
        float biomeIndex = 0f;
        int mumBiomds = biomes.Length;
        float blendRange = worldSettings.blendAmount / 2f;

        for (int i = 0; i < biomes.Length; i++)
        {
            float dst = heightDecimal - biomes[i].startHeight;
            float weight = Mathf.InverseLerp(-blendRange, blendRange, dst);
            biomeIndex *= (1 - weight);
            biomeIndex += i * weight;
        }

        return biomeIndex / Mathf.Max(1, biomes.Length - 1);
    }
}