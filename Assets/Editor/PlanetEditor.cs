using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Planet))]
public class PlanetEditor : Editor
{
    Planet planet;
    Editor shapeEditor;
    Editor colourEditor;

    public override void OnInspectorGUI()
    {
        SerializedProperty shapeSettings = serializedObject.FindProperty("shapeSettings");
        SerializedProperty colourSettings = serializedObject.FindProperty("colourSettings");

        using (var check = new EditorGUI.ChangeCheckScope())
        {
            base.OnInspectorGUI();
            if (check.changed)
            {
                planet.GeneratePlanet();
            }
        }

        if (GUILayout.Button("MAKE THAT PLANET BOI"))
        {
            planet.GeneratePlanet();
        }

        DrawSettingsEditor(planet.shapeSettings, planet.OnShapeSettingsUpdated, ref shapeSettings, ref shapeEditor);
        DrawSettingsEditor(planet.colourSettings, planet.OnColourSettingsUpdated, ref colourSettings, ref colourEditor);
    }

    void DrawSettingsEditor(Object settings, System.Action onSettingsUpdated, ref SerializedProperty serializedSettings, ref Editor editor)
    {
        if (settings != null)
        {
            serializedSettings.isExpanded = EditorGUILayout.InspectorTitlebar(serializedSettings.isExpanded, settings);
            // This monitors the change in the editor.
            using (var check = new EditorGUI.ChangeCheckScope())

            // serializedSettings.isExpanded will equal to the value of what it would be when the user clicks the foldout.
            if (serializedSettings.isExpanded)
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

    // I dont know what that does specifcally, I want to read it in the docs.
    private void OnEnable()
    {
        // What is the target object??
        planet = (Planet)target;
    }

}

/**
 * Some notes:
 * Things derived from "UnityEngine.Object" are serialized on their own.
 * 
 * The only classes you can use as base classes which are derived from UnityEngine.Object 
 * are MonoBehaviour or ScriptableObject.
 * 
 * So values saved here won't be serialised and saved when running. I.e the value you have when in editor mode, 
 * then when you click play, till go with the default value in the script.
 * 
 **/ 