using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AssetCollector))]
public class AssetCollectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AssetCollector generator = (AssetCollector)target;

        EditorGUILayout.Space();
        if (GUILayout.Button("Build"))
        {
            generator.Build();
            Repaint();
        }
    }
}