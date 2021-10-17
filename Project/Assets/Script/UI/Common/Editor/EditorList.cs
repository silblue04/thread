using UnityEditor;
using UnityEngine;

// https://catlikecoding.com/unity/tutorials/editor/custom-list/
public class EditorList
{
    public static void Show(SerializedProperty list)
    {
        EditorGUILayout.PropertyField(list);
		// for (int i = 0; i < list.arraySize; ++i)
        // {
		// 	EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
		// }
    }
}
