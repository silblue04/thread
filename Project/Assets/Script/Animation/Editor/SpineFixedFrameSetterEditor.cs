using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SpineFixedFrameSetter))]
public class SpineFixedFrameSetterEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        SpineFixedFrameSetter generator = (SpineFixedFrameSetter)target;
        var prefabType = UnityEditor.PrefabUtility.GetPrefabAssetType(generator);

        EditorGUILayout.Space();
        if (GUILayout.Button("Set Frame"))
        {
            if(prefabType != PrefabAssetType.NotAPrefab)
            {
                generator.SetFrame();
                Repaint();
                SceneView.RepaintAll();
                //UnityEditorInternal.InternalEditorUtility.RepaintAllViews();
            }
        }
    }
}