using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Map_Generator))]
public class Map_Generator_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Map_Generator generator = (Map_Generator)target;
        if (GUILayout.Button("Generate Tilemap"))
        {
            generator.Generate_Tilemap();
        }
        if (GUILayout.Button("Clear All Tilemap"))
        {
            generator.ClearAllTiles();
        }
    }
}
