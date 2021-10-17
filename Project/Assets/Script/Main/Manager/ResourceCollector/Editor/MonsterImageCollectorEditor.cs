using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PakImageCollector))]
public class PakImageCollectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        PakImageCollector generator = (PakImageCollector)target;

        EditorGUILayout.Space();
        if (GUILayout.Button("Build"))
        {
            generator.Build();
            Repaint();
        }
    }
}