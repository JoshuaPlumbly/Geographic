using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    public bool autoUpdate = true;
    [SerializeField, Range(2, 256)] private int _resolution = 10;
    [SerializeField] public ShapeSettings _shapeSettings;
    [SerializeField] public PlanetColourSettings _colourSettings;

    [HideInInspector] public bool _shapeSettingFoldout;
    [HideInInspector] public bool _colourSettingFoldout;

    private ShapeGenerator _shapeGenerator = new ShapeGenerator();
    private ColourGenerator _colourGenerator = new ColourGenerator();

    [SerializeField, HideInInspector] MeshFilter[] _meshFilters;
    TerrainFace[] _terrainFaces;

    public void GeneratePlanet()
    {
        InitializeTerrainFaces();
        GenerateMeshFromTerrainFaces();
        GenerateMeshColour();
    }

    private void InitializeTerrainFaces()
    {
        _shapeGenerator.UpdateSettings(_shapeSettings);
        _colourGenerator.UpdateSettings(_colourSettings);

        if (_meshFilters == null || _meshFilters.Length == 0)
            _meshFilters = new MeshFilter[6];

        _terrainFaces = new TerrainFace[6];

        Vector3[] directions =
        {
            Vector3.up,
            Vector3.down,
            Vector3.left,
            Vector3.right,
            Vector3.forward,
            Vector3.back
        };

        for (int i = 0; i < 6; i++)
        {
            if (_meshFilters[i] == null)
            {
                GameObject meshObject = new GameObject("mesh");
                meshObject.transform.parent = transform;

                meshObject.AddComponent<MeshRenderer>();
                _meshFilters[i] = meshObject.AddComponent<MeshFilter>();
                _meshFilters[i].sharedMesh = new Mesh();
            }

            _meshFilters[i].GetComponent<MeshRenderer>().sharedMaterial = _colourSettings.material;

            _terrainFaces[i] = new TerrainFace(_shapeGenerator, _meshFilters[i].sharedMesh, _resolution, directions[i]);
        }
    }

    public void GenerateMeshFromTerrainFaces()
    {
        foreach (var face in _terrainFaces)
        {
            face.ConstructMesh();
        }

        _colourGenerator.UpdateElevation(_shapeGenerator.ElevationMinMax);
    }

    public void GenerateMeshColour()
    {
        _colourGenerator.UpdateColour();
    }
}