using UnityEngine;

public class TerrainFace
{
    ShapeGenerator _shapeGenerator;
    Mesh _mesh;
    int _resolution;
    Vector3 _localUp;
    Vector3 _axisA;
    Vector3 _axisB;

    public TerrainFace(ShapeGenerator shapeGenerator, Mesh mesh, int resolution, Vector3 localUp)
    {
        _shapeGenerator = shapeGenerator;
        _mesh = mesh;
        _resolution = resolution;
        _localUp = localUp;

        _axisA = new Vector3(localUp.y, localUp.z, localUp.x);
        _axisB = Vector3.Cross(localUp, _axisA);
    }

    public void ConstructMesh()
    {
        Vector3[] vertices = new Vector3[_resolution * _resolution];
        int[] triangles = new int[(_resolution - 1) * (_resolution - 1) * 6];
        int triangleIndex = 0;

        for (int y = 0; y < _resolution; y++)
        {
            for (int x = 0; x < _resolution; x++)
            {
                int i = x + (y * _resolution);
                Vector2 percent = new Vector2(x, y) / (_resolution - 1);
                Vector3 pointOnUnitySphere = _localUp + (percent.x - 0.5f) * 2 * _axisA + (percent.y - 0.5f) * 2 * _axisB;
                pointOnUnitySphere.Normalize();
                vertices[i] = _shapeGenerator.CalculatePointFrom(pointOnUnitySphere);

                if (x != _resolution - 1 && y != _resolution - 1)
                {
                    triangles[triangleIndex] = i;
                    triangles[triangleIndex + 1] = i + _resolution + 1;
                    triangles[triangleIndex + 2] = i + _resolution;
                    triangles[triangleIndex + 3] = i;
                    triangles[triangleIndex + 4] = i + 1;
                    triangles[triangleIndex + 5] = i + _resolution + 1;
                    triangleIndex += 6;
                }
            }
        }

        _mesh.Clear();
        _mesh.vertices = vertices;
        _mesh.triangles = triangles;
        _mesh.RecalculateNormals();
    }
}