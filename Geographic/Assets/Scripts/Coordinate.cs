using UnityEngine;

public struct Coordinate
{
    public float latitude;
    public float longitude;

    public Coordinate(float latitude, float longitude)
    {
        this.latitude = latitude;
        this.longitude = longitude;
    }

    public static Coordinate PointToCoordinate(Vector3 pointOnUnitSphere)
    {
        return new Coordinate(
            GetLatitudeFor(pointOnUnitSphere),
            GetLongitudeFor(pointOnUnitSphere));
    }

    public static Vector2 CoordinateAsVector2(Vector3 pointOnUnitSphere)
    {
        return new Vector2(
            GetLatitudeFor(pointOnUnitSphere),
            GetLongitudeFor(pointOnUnitSphere));
    }

    public static float GetLatitudeFor(Vector3 pointOnUnitSphere)
    {
        return Mathf.Asin(pointOnUnitSphere.y);
    }

    public static float GetLongitudeFor(Vector3 pointOnUnitSphere) 
    { 
        return Mathf.Atan2(pointOnUnitSphere.x, -pointOnUnitSphere.z); 
    }

    public static Vector3 CoordinateToPoint(Coordinate coordinate)
    {
        float y = Mathf.Sin(coordinate.latitude);
        float r = Mathf.Cos(coordinate.latitude);
        float x = Mathf.Sin(coordinate.latitude) * r;
        float z = -Mathf.Cos(coordinate.longitude) * r;
        return new Vector3(x, y, z);
    }
}