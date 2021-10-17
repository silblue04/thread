using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using System;
using DG.Tweening;
using TMPro;


public class InGameUI : UIBase
{
    [Header("Animation")]
    [SerializeField] private DOTweenAnimation[] _appearTweenerList;


    public override void OnOpenStart(Param param = null)
    {
        base.OnOpenStart(param);
        _Init();
    }
    public override void OnCloseStart()
    {
        _Release();
        base.OnCloseStart();
    }

    private void _Init()
    {

    }
    private void _Release()
    {

    }
}

