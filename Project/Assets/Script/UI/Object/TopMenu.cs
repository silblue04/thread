using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;
using DG.Tweening;
using TMPro;


public class TopMenu : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] private DOTweenAnimation[] _appearTweenerList;


    public void Init()
    {
        
        
    }
    public void Release()
    {

    }

    public void PlayAppearAnimation(bool onAppear)
    {
        Util.PlayTweenerList(_appearTweenerList, (onAppear == false));
    }
}
