using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

[ExecuteInEditMode]
public class DotSliderElement : MonoBehaviour
{
    public enum State
    {
        Off,
        On,
        Now,
    }
    [SerializeField] private GameObjectSelector _stateSelector;

#if UNITY_EDITOR
    [Header("-- [UNITY_EDITOR] State ------")]
    [SerializeField] private State _state = State.Off;
#endif 

    public void SetState(State state)
    {
        _stateSelector.Select(state);
    }

#if UNITY_EDITOR
    public void OnGUI()
    {
        if(UnityEditor.EditorApplication.isPlaying)
        {
            return;
        }

        SetState(_state);
    }
#endif 
}
