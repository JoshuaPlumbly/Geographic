using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetGenerator : MonoBehaviour
{
    [SerializeField] private ShapeSettings _shapeSettings;
    [SerializeField] private PlanetColourSettings _colourSettings;
    [SerializeField, Range(2,200)] private int _resolution = 10;
    [SerializeField] private Material _material;

    private SphereGenerator _sphereGenerator;

    private void OnValidate()
    { 
        if(_sphereGenerator == null)
            _sphereGenerator = new SphereGenerator(_shapeSettings, _resolution, transform, _material);

        _sphereGenerator._resolution = _resolution;
        _sphereGenerator.InitializeMeshFilters();
    }
}