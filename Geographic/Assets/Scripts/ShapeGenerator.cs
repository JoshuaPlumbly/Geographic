using UnityEngine;

public class ShapeGenerator
{
    ShapeSettings _settings;

    public ShapeGenerator(ShapeSettings settings)
    {
        this._settings = settings;
    }

    public Vector3 CalculatePointOnPlanet(Vector3 pointOnUnitSphere)
    {
        return pointOnUnitSphere * _settings.radius;
    }
}