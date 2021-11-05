using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGenerator
{
    private Mesh[] _meshFaces;
    private MeshFilter[] _meshFilter;
    private IShapeSettings _shapeSettings;
    public int _resolution;
    private Transform _parent;
    private Material _material;

    public SphereGenerator(IShapeSettings shapeSettings, int resolution, Transform parent, Material material)
    {
        _shapeSettings = shapeSettings;
        _resolution = resolution;
        _parent = parent;
        _material = material;
    }

    public static Mesh GenerateSphereFace(Vector3 normal, int resolution, IShapeSettings settings)
    {
        Mesh mesh = new Mesh();
        mesh.name = "MeshFace";

        Vector3 axisA = new Vector3(normal.y, normal.z, normal.x);
        Vector3 axisB = Vector3.Cross(normal, axisA);

        Vector3[] vertices = new Vector3[resolution * resolution];
        int[] triangles = new int[(resolution - 1) * (resolution - 1) * 6];
        int triIndex = 0;

        for (int y = 0; y < resolution; y++)
        {
            for (int x = 0; x < resolution; x++)
            {
                int vertexIndex = x + y * resolution;
                Vector2 t = new Vector2(x, y) / (resolution - 1f);
                Vector3 point = normal + axisA * (2 * t.x - 1) + axisB * (2 * t.y - 1);
                point = PointOnCubeToPointOnSphere(point);
                point = settings.CalculatePoint(point);
                vertices[vertexIndex] = point;

                if (x != resolution - 1 && y != resolution - 1)
                {
                    triangles[triIndex] = vertexIndex;
                    triangles[triIndex + 1] = vertexIndex + resolution + 1;
                    triangles[triIndex + 2] = vertexIndex + resolution;
                    triangles[triIndex + 3] = vertexIndex;
                    triangles[triIndex + 4] = vertexIndex + 1;
                    triangles[triIndex + 5] = vertexIndex + resolution + 1;
                    triIndex += 6;
                }
            }
        }

        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        return mesh;
    }

    public static Mesh[] GenerateSphereFaces(int resolution, IShapeSettings settings)
    {
        Mesh[] meshFaces = new Mesh[6];

        Vector3[] faceNormals = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < faceNormals.Length; i++)
        {
            meshFaces[i] = GenerateSphereFace(faceNormals[i], resolution, settings);
        }

        return meshFaces;
    }

    public void InitializeMeshFilters()
    {
        if (_meshFilter == null || _meshFilter.Length != 6)
            _meshFilter = new MeshFilter[6];

        _meshFaces = GenerateSphereFaces(_resolution, _shapeSettings);

        for (int i = 0; i < 6; i++)
        {
            if (_meshFilter[i] == null)
            {
                GameObject meshObject = new GameObject("Mesh");
                meshObject.transform.parent = _parent;

                meshObject.AddComponent<MeshRenderer>().sharedMaterial = _material;
                _meshFilter[i] = meshObject.AddComponent<MeshFilter>();
            }

            _meshFilter[i].sharedMesh = _meshFaces[i];
        }
    }

    public static Vector3 PointOnCubeToPointOnSphere(Vector3 p)
    {
        float x2 = p.x * p.x;
        float y2 = p.y * p.y;
        float z2 = p.z * p.z;
        float x = p.x * Mathf.Sqrt(1f - (y2 + z2) / 2 + (y2 * z2) / 3);
        float y = p.y * Mathf.Sqrt(1f - (z2 + x2) / 2 + (z2 * x2) / 3);
        float z = p.z * Mathf.Sqrt(1f - (x2 + y2) / 2 + (x2 * y2) / 3);
        return new Vector3(x, y, z);
    }

    public static void GenerateSphere(Transform parent, int resolution, Material material, ShapeSettings shapeSettings)
    {
        var faces = GenerateSphereFaces(resolution, shapeSettings);

        for (int i = 0; i < faces.Length; i++)
        {
            GameObject meshFace = new GameObject("mesh");
            meshFace.AddComponent<MeshFilter>().mesh = faces[i];
            meshFace.AddComponent<MeshRenderer>().sharedMaterial = material;
            meshFace.transform.parent = parent;
        }
    }
}