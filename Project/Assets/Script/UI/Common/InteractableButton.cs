using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;


public class InteractableButton
    : Button
    , IPointerDownHandler, IPointerExitHandler, IPointerUpHandler
{
    public enum State
    {
        ImpossibleToClick,
        PossibleToClick,

        MAX
    }
    private State _curState = State.MAX;
    public State curState { get { return _curState; } }

    [Header("꾸욱- 눌르고 있을 때 이벤트 콜 할 것인지")]
    [SerializeField] private bool _onPointDownEvent = false;

    [Header("[OnOff StateObj] 0 : ImpossibleToClick, 1 : PossibleToClick")]
    [SerializeField] private GameObject[] _stateObjList = new GameObject[BitConvert.Enum32ToInt(State.MAX)];

    
    public void SetInteractable(bool interactable)
    {
        this.interactable = interactable;
        _curState = (interactable) ? State.PossibleToClick : State.ImpossibleToClick;

        for(int i = 0; i < _stateObjList.Length; ++i)
        {
            if(_stateObjList[i] != null)
            {
                _stateObjList[i].SetActive(false);
            }
        }

        int curStateIndex = BitConvert.Enum32ToInt(_curState);
        if (_stateObjList[curStateIndex] != null)
        {
            _stateObjList[curStateIndex].SetActive(true);
        }
    }

    
    public override void OnPointerDown(PointerEventData eventData)
    {
        base.OnPointerDown(eventData);

        if(_onPointDownEvent)
        {
            _StartCoOfUpdateOnPoinsterDownTimer();
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        if(_onPointDownEvent)
        {
            _StopCoOfUpdateOnPoinsterDownTimer();
        }
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        base.OnPointerUp(eventData);

        if(eventData.dragging)
        {
            return;
        }

        if(_onPointDownEvent)
        {
            _StopCoOfUpdateOnPoinsterDownTimer();
        }
    }

    private IEnumerator _coUpdateOnPoinsterDownTimer    = null;
    private void _StartCoOfUpdateOnPoinsterDownTimer()
    {
        _StopCoOfUpdateOnPoinsterDownTimer();

        _coUpdateOnPoinsterDownTimer = _CoUpdateOnPoinsterDownTimer();
        StartCoroutine(_coUpdateOnPoinsterDownTimer);
    }
    private void _StopCoOfUpdateOnPoinsterDownTimer()
    {
        if(_coUpdateOnPoinsterDownTimer == null)
        {
            return;
        }

        StopCoroutine(_coUpdateOnPoinsterDownTimer);
        _coUpdateOnPoinsterDownTimer = null;
    }
    private IEnumerator _CoUpdateOnPoinsterDownTimer()
    {
        var waitForSeconds = YieldInstructionCache.WaitForSeconds(0.02f);

        yield return YieldInstructionCache.WaitForSeconds(1.0f);
        while(true)
        {
            onClick?.Invoke();
            yield return waitForSeconds;
        }
    }
}
