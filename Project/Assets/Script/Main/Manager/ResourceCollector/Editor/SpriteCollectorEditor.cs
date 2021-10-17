using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpriteCollector))]
public class SpriteCollectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SpriteCollector generator = (SpriteCollector)target;

        EditorGUILayout.Space();
        if (GUILayout.Button("Build"))
        {
            generator.Build();
            Repaint();
        }
    }
}