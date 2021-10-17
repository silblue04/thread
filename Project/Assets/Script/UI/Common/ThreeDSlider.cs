using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThreeDSlider : MonoBehaviour
{
    [SerializeField] Transform _valuePivot;

    public void SetValue(float ratio)
    {
        if(ratio > DefsDefault.MAX_RATIO)
        {
            ratio = DefsDefault.MAX_RATIO;
        }
        else if(ratio < DefsDefault.MIN_RATIO)
        {
            ratio = DefsDefault.MIN_RATIO;
        }
        
        _valuePivot.localScale = new Vector3(
            ratio
            , _valuePivot.localScale.y
            , _valuePivot.localScale.z);
    }
}
