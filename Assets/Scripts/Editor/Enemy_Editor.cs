using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemyController))]
public class Enemy_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EnemyController generator = (EnemyController)target;
        if (GUILayout.Button("EnemyMove"))
        {
            generator.EnemyMove();
        }
    }
}
