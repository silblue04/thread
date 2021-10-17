using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(MaterialCollector))]
public class MaterialCollectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        MaterialCollector generator = (MaterialCollector)target;

        EditorGUILayout.Space();
        if (GUILayout.Button("Build"))
        {
            generator.Build();
            Repaint();
        }
    }
}