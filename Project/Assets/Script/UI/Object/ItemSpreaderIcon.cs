using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.Events;
using UnityEngine.UI;
using DG.Tweening;


public class ItemSpreaderIcon : MonoBehaviour
{
    [SerializeField] private Image _icon;

    private Action<ItemSpreaderIcon> _callbackComplete = null;


    public void Init()
    {
        this.gameObject.SetActive(false);
    }

    public void SetSprite(int item_idx)
    {
        _icon.sprite = DefsItem.GetItemIcon(item_idx);
    }

    public void SetPathTween
    (
        int index
        , List<Vector3> path
        , Ease ease, float duration
        , float maxDelay, float minDelay
        , Action<ItemSpreaderIcon> callbackComplete = null
    )
    {
        this.transform.DOPath(path.ToArray(), duration, PathType.CatmullRom, PathMode.TopDown2D)
            .SetDelay(UnityEngine.Random.Range(minDelay, maxDelay))
            .SetEase(ease)
            .OnComplete(CompletePathTween);

        this.gameObject.SetActive(true);
        _callbackComplete = callbackComplete;
    }

    private void CompletePathTween()
    {
        _callbackComplete?.Invoke(this);
        this.gameObject.SetActive(false);
    }
}
