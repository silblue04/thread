using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class TabButton : Button
{
    public enum State
    {
        Basic,
        Selected,
        Locked,

        MAX
    }
    private State _curState = State.MAX;
    public State curState { get { return _curState; } }

    [Header("[InitialState]")]
    [SerializeField] private State _initialState = State.Basic;

    [Header("[OnOff StateObj] 0 : Basic, 1 : Selected, 2 : Locked")]
    [SerializeField] private  GameObject[] _stateObjList = new GameObject[BitConvert.Enum32ToInt(State.MAX)];

    [SerializeField] private  Action<State> _callbackOnChangeState = null;

    private List<TabButtonState> _tabButtonStates = new List<TabButtonState>();


    protected override void Awake()
    {
        _tabButtonStates.AddRange(this.GetComponentsInChildren<TabButtonState>(true));
        
        _InitStateObjList();
        SetState(_initialState);
    }

    protected override void Start()
    {
        for(int i = 0; i < onClick.GetPersistentEventCount(); ++i)
        {
            onClick.SetPersistentListenerState(i, UnityEventCallState.RuntimeOnly);
        }
    }

    /*
     * myung_ari
     * _returnInitialStateFunction 를
     *      1) 등록하여 TabButtonGroup::_SetTabButtonList 내부에서 자동으로 SetState 를 호출하게 하거나
     *      2) 등록하지 않고 직접 SetState를 호출하여 UI 초기상태 셋팅이 가능합니다.
    */
    private Func<TabButton.State> _returnInitialStateFunction = null;
    public void SetCheckStateAction(Func<TabButton.State> returnInitialStateFunction)
    {
        _returnInitialStateFunction = returnInitialStateFunction;
    }


    public void SetStateCallback(Action<State> callbackOnChangeState)
    {
        _callbackOnChangeState = callbackOnChangeState;
    }

    public void SetState(State state)
    {
        if (_curState == state || state == State.MAX)
        {
            return;
        }


        _curState = state;
        int stateIndex = BitConvert.Enum32ToInt(_curState);

        _InitStateObjList();
        if (_stateObjList[stateIndex])
        {
            _stateObjList[stateIndex].SetActive(true);
        }
        _callbackOnChangeState?.Invoke(_curState);
    }

    public TabButton.State GetInitialState()
    {
        if(this._returnInitialStateFunction == null)
        {
            return _curState;
        }

        TabButton.State state = TabButton.State.MAX;
        state = _returnInitialStateFunction.Invoke();

        return state;
    }

    public virtual void SetInteractable( bool enable )
    {
        this.interactable = enable;

        if( this.gameObject.activeInHierarchy == false )
        {
            Debug.Log( "비활성화 되어 있습니다." );
        }
    }

    protected override void DoStateTransition( SelectionState state, bool instant )
    {
        base.DoStateTransition( state, instant );

        if (_tabButtonStates.Count > 0)
        {
            var tabButtonState = _ConvertToTabButtonStateType(state);
            for (int i = 0; i < _tabButtonStates.Count; ++i)
            {
                _tabButtonStates[i].DoStateTransition(tabButtonState, instant, colors.fadeDuration);
            }
        }
    }

    private TabButtonState.TabButtonStateType _ConvertToTabButtonStateType(SelectionState selectionState)
    {
        switch (selectionState)
        {
            case SelectionState.Pressed:
                {
                    return TabButtonState.TabButtonStateType.Pressed;
                }
            case SelectionState.Disabled:
                {
                    return TabButtonState.TabButtonStateType.Disabled;
                }
            default:
                {
                    return TabButtonState.TabButtonStateType.Normal;
                }
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (false == interactable)
        {
            return;
        }
            

        if (1 >= eventData.clickCount)
        {
            onClick.Invoke();
        }
    }

    private void _InitStateObjList()
    {
        foreach (var statObj in _stateObjList)
        {
            if (statObj)
            {
                statObj.SetActive(false);
            }
        }
    }
}