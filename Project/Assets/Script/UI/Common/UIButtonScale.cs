using DG.Tweening;
using System.Collections;
using System.Collections.Generic; 
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//[RequireComponent(typeof(Button))]
public class UIButtonScale : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public Vector3 start = Vector3.one;
    public Vector3 end = new Vector3(0.96f, 0.96f, 0.96f);
    public Ease easeType = Ease.OutCirc;
    public float duration = 0.25f;
    public float delay = 0f;
    private Sequence _sequence = null;

    private Button _targetButton = null;
    private Toggle _targetToggle = null;
    private bool _isButton = true;

    private void Awake()
    {
        _targetButton = GetComponent<Button>();
        _targetToggle = GetComponent<Toggle>();

        if(_targetButton != null)
            _isButton = true;
        else if(_targetToggle != null)
            _isButton = false;
        else
        {
            Destroy(this);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if(_isButton)
        {            
            if (false == _targetButton.interactable)
                return;
        }
        else
        {
            if (false == _targetToggle.interactable)
                return;
        }

        transform.localScale = start;
        _PlayTween(end);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _PlayTween(start);
    }

    private void _PlayTween(Vector3 target)
    {
        _sequence = DOTween.Sequence();
        _sequence.Append(this.transform.DOScale(target, duration)).SetEase(easeType).SetDelay(delay);
    }
}
