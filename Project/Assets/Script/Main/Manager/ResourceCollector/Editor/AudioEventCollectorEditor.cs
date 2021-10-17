using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AudioEventCollector))]
public class AudioEventCollectorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AudioEventCollector generator = (AudioEventCollector)target;

        EditorGUILayout.Space();
        if (GUILayout.Button("Build"))
        {
            generator.Build();
            Repaint();
        }
    }
}