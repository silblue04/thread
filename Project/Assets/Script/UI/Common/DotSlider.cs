using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[ExecuteInEditMode]
public class DotSlider : MonoBehaviour
{
#if UNITY_EDITOR
    [Header("-- [UNITY_EDITOR] Value ------")]
    [SerializeField] protected int _maxStep;
    [SerializeField] protected int _curStep;
#endif 

    [Header("Prefab")]
    [SerializeField] protected RectTransform _dotSliderElementRoot;
    [SerializeField] protected DotSliderElement _dotSliderElementPrefab;

    protected List<DotSliderElement> _dotSliderElementList = null;
    protected int _max      = DefsDefault.VALUE_ZERO;
    protected int _value    = DefsDefault.VALUE_ZERO;
    

    public virtual void SetMaxValue(int max)
    {   
        if(_dotSliderElementPrefab == null)
        {
            return;
        }
        if(_max == max)
        {
            return;
        }

        _InitDotSliderElementList();

        for (int i = 0; i < max; ++i)
        {
            DotSliderElement dotSliderElement = Util.Instantiate<DotSliderElement>(_dotSliderElementRoot, _dotSliderElementPrefab);
            _dotSliderElementList.Add(dotSliderElement);
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(_dotSliderElementRoot);

        _max = max;
    }

    public virtual void SetValue(int value)
    {   
        if (_dotSliderElementList == null)
        {
            return;
        }

        for (int i = 0; i < _dotSliderElementList.Count; ++i)
        {
            DotSliderElement.State state =  ((i < value) ? DotSliderElement.State.On : DotSliderElement.State.Off);
            _dotSliderElementList[i].SetState(state);
        }

        _value = value;
    }

    protected void _InitDotSliderElementList()
    {
        if (_dotSliderElementList == null)
        {
            _dotSliderElementList = new List<DotSliderElement>();
        }
        else
        {
#if UNITY_EDITOR
            if (UnityEditor.EditorApplication.isPlaying)
            {
                Util.DestroyChild(_dotSliderElementRoot);
            }
            else
            {
                Util.DestroyImmediateChild(_dotSliderElementRoot);
            }
#else
            Util.DestroyChild(_dotSliderElementRoot);
#endif

            _dotSliderElementList.Clear();
        }

        _max = DefsDefault.VALUE_ZERO;
    }

#if UNITY_EDITOR
    public virtual void OnGUI()
    {
        if (UnityEditor.EditorApplication.isPlaying)
        {
            return;
        }
        if(_dotSliderElementPrefab == null)
        {
            return;
        }
        if(UnityEditor.EditorApplication.isPlaying)
        {
            return;
        }

        SetMaxValue(_maxStep);
        SetValue(_curStep);
    }
#endif 
}
