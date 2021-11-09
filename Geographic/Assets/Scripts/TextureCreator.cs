using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class TextureCreator : MonoBehaviour
{
    [SerializeField, Range(2,512)] private int _resolution = 256;
    [SerializeField] private float _freqancey = 1f;

    private Texture2D _texture2D;
    private PerlinNoise _noise;

    public void OnEnable()
    {
        CalculateTextures();
    }

    public void CalculateTextures()
    {
        _texture2D = new Texture2D(_resolution, _resolution, TextureFormat.RGB24, true);
        _texture2D.name = "Procedural Texture";
        _texture2D.wrapMode = TextureWrapMode.Clamp;
        _texture2D.filterMode = FilterMode.Trilinear;
        _texture2D.anisoLevel = 9;
        _noise = new PerlinNoise(_freqancey);
        GetComponent<MeshRenderer>().material.mainTexture = _texture2D;

        FillTexture();
    }

    public void FillTexture()
    {
        if (_texture2D.width != _resolution || _texture2D.height != _resolution)
            _texture2D.Resize(_resolution, _resolution);

        if (_noise == null)
            _noise = new PerlinNoise(_freqancey);

        Vector3 point00 = new Vector3(-0.5f, -.05f);
        Vector3 point01 = new Vector3(-0.5f, 0.5f);
        Vector3 point10 = new Vector3(0.5f, -0.5f);
        Vector3 point11 = new Vector3(0.5f, 0.5f);

        float stepSize = 1f / _resolution;

        for (int y = 0; y < _resolution; y++)
        {
            Vector3 point0 = Vector3.Lerp(point00, point01, (y + 0.5f) * stepSize);
            Vector3 point1 = Vector3.Lerp(point10, point11, (y + 0.5f) * stepSize);

            for (int x = 0; x < _resolution; x++)
            {
                Vector3 point = Vector3.Lerp(point0, point1, (x + 0.5f) * stepSize);
                _texture2D.SetPixel(x, y, Color.white * (PerlinNoise.GetValue3D(new Vector3(x,y,0f),_freqancey)));
            }
        }

        _texture2D.Apply();
    }
}


[CustomEditor(typeof(TextureCreator))]
public class TextureCreatorInspector : Editor
{
    public override void OnInspectorGUI()
    {
        EditorGUI.BeginChangeCheck();
        DrawDefaultInspector();

        if (GUILayout.Button("Fill Texture"))
        {
            (target as TextureCreator).CalculateTextures();

        }

        if (EditorGUI.EndChangeCheck() && Application.isPlaying)
        {
            (target as TextureCreator).FillTexture();
        }
    }
}