using UnityEngine;
using UnityEngine.UI;

namespace UnityEditor.UI
{
    [CustomEditor(typeof(ToggleButton), true)]
    [CanEditMultipleObjects]
    /// <summary>
    ///   Custom Editor for the Toggle Component.
    ///   Extend this class to write a custom editor for an Toggle-derived component.
    /// </summary>
    public class ToggleButtonEditor : SelectableEditor
    {
        SerializedProperty m_OnValueChangedProperty;
        SerializedProperty m_TransitionProperty;
        SerializedProperty m_GraphicProperty;
        SerializedProperty m_GroupProperty;
        SerializedProperty m_IsOnProperty;

        protected override void OnEnable()
        {
            base.OnEnable();

            m_TransitionProperty = serializedObject.FindProperty("toggleTransition");
            m_GraphicProperty = serializedObject.FindProperty("graphic");
            m_GroupProperty = serializedObject.FindProperty("m_Group");
            m_IsOnProperty = serializedObject.FindProperty("m_IsOn");
            m_OnValueChangedProperty = serializedObject.FindProperty("onValueChanged");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();

            serializedObject.Update();
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_IsOnProperty);
            if (EditorGUI.EndChangeCheck())
            {
                Toggle toggle = serializedObject.targetObject as Toggle;
                ToggleGroup group = m_GroupProperty.objectReferenceValue as ToggleGroup;

                toggle.isOn = m_IsOnProperty.boolValue;

                if (group != null && toggle.IsActive())
                {
                    if (toggle.isOn || (!group.AnyTogglesOn() && !group.allowSwitchOff))
                    {
                        toggle.isOn = true;
                        group.NotifyToggleOn(toggle);
                    }
                }
            }
            EditorGUILayout.PropertyField(m_TransitionProperty);
            EditorGUILayout.PropertyField(m_GraphicProperty);
            EditorGUI.BeginChangeCheck();
            EditorGUILayout.PropertyField(m_GroupProperty);
            if (EditorGUI.EndChangeCheck())
            {
                Toggle toggle = serializedObject.targetObject as Toggle;
                ToggleGroup group = m_GroupProperty.objectReferenceValue as ToggleGroup;
                toggle.group = group;
            }

            EditorGUILayout.Space();

            // Draw the event notification options

            EditorList.Show(base.serializedObject.FindProperty("_stateSelector"));
            EditorGUILayout.PropertyField(m_OnValueChangedProperty);

            serializedObject.ApplyModifiedProperties();
        }
    }
}