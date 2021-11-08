using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(PlanetGenerator))]
public class PlanetGeneratorEditor : Editor
{
    PlanetGenerator _planetGenerator;
    Editor shpaeEditor;
    Editor colourEditor;

    public override void OnInspectorGUI()
    {
        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();

            if (check.changed)
                _planetGenerator.GeneratePlanet();
        }

        if (GUILayout.Button("Generate Planet"))
        {
            (target as PlanetGenerator).GeneratePlanet();
        }

        DrawSettingsEditor(_planetGenerator._shapeSettings, _planetGenerator.GenerateMeshFromTerrainFaces, ref _planetGenerator._shapeSettingFoldout, ref shpaeEditor);
        DrawSettingsEditor(_planetGenerator._colourSettings, _planetGenerator.GenerateMeshColour, ref _planetGenerator._colourSettingFoldout, ref colourEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref bool foldout, ref Editor editor)
    {
        if (settings != null)
        {
            foldout = EditorGUILayout.InspectorTitlebar(foldout, settings);

            using (var check = new EditorGUI.ChangeCheckScope())
            {
                if (foldout)
                {
                    CreateCachedEditor(settings, null, ref editor);
                    editor.OnInspectorGUI();

                    if (check.changed)
                    {
                        if (onSettingsUpdated != null)
                        {
                            onSettingsUpdated();
                        }
                    }
                }
            }
        }
    }

    private void OnEnable()
    {
        _planetGenerator = (PlanetGenerator)target;
    }
}
