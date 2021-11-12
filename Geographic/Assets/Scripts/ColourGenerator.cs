using UnityEngine;

public class ColourGenerator
{
    PlanetColourSettings _settings;
    Texture2D _texture;
    const int _textureResolution = 50;

    public void UpdateSettings(PlanetColourSettings settings)
    {
        this._settings = settings;

        if (_texture==null)
        {
            _texture = new Texture2D(_textureResolution, 1);
        }
    }

    public void UpdateElevation(MinMaxF elevationMinMax)
    {
        _settings.material.SetVector("_elevationMinMax", new Vector4(elevationMinMax.Min, elevationMinMax.Max));
    }

    public void UpdateColour()
    {
        Color[] colours = new Color[_textureResolution];

        Gradient gradient = _settings.gradient;

        for (int i = 0; i < _textureResolution; i++)
        {
            colours[i] = gradient.Evaluate(i / (_textureResolution - 1f));
        }

        _texture.SetPixels(colours);
        _texture.Apply();
        _settings.material.SetTexture("_texture", _texture);
    }
}