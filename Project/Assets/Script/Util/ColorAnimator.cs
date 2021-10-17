using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using UnityEngine.UI;


public class ColorAnimator : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Graphic _targetGraphic;
    [SerializeField] private SpriteRenderer _targetSpriteRenderer;

    [SerializeField] private Color[] _colorList;
    [SerializeField] private float _duration = 1.0f;
    [SerializeField] private float _delay = 3.0f;


    private void Start()
    {
        Sequence sequence = DOTween.Sequence();

        if(_targetGraphic != null)
        {
            for (int i = 0; i < _colorList.Length; ++i)
            {
                sequence.Append(
                    _targetGraphic.DOColor(_colorList[i], _duration)
                        .SetDelay(_delay));
            }
        }
        else if(_targetSpriteRenderer != null)
        {
            for (int i = 0; i < _colorList.Length; ++i)
            {
                sequence.Append(
                    _targetSpriteRenderer.DOColor(_colorList[i], _duration)
                        .SetDelay(_delay));
            }
        }
        

        sequence.SetLoops(-1, LoopType.Restart);
        sequence.Restart();
    }
}
