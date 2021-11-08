using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PerlinSphereTest : MonoBehaviour
{
    [SerializeField] private float _frequency = 1f;
    [SerializeField, Range(0, 20)] private int _height = 5;
    [SerializeField, Range(0, 20)] private int _width = 5;
    [SerializeField, Range(0, 20)] private int _depth = 5;

    public Vector3[] _points;

    private void Awake()
    {
        CreatePoints();
    }

    public void CreatePoints()
    {
        _points = new Vector3[_height * _depth * _width];

        for (int z = 0, i = 0; z < _depth; z++)
        {
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++, i++)
                {
                    _points[i] = new Vector3(x, y, z);
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (_points != null)
        {
            foreach (Vector3 p in _points)
            {
                float sample = NoisePerlin.GetValue3D(p, _frequency);
                sample = (sample + 1f) / 2f;
                Gizmos.color = new Color(sample, sample, sample, 1);
                Gizmos.DrawSphere(p, 0.3f);
            }
        }
    }

}

[CustomEditor(typeof(PerlinSphereTest))]
public class PerlinSphereTestInspector : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        if (GUILayout.Button("Create Points"))
        {
            (target as PerlinSphereTest).CreatePoints();

        }
    }
}