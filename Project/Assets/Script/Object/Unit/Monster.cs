using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Monster : UnitBase
{
    [Header("MonsterBase > HpGauge")]
    [SerializeField] protected ThreeDSlider _hpGauge;
    
    protected enum State
    {
        NONE = DefsDefault.VALUE_NONE,

        Appear,
        Idle,
        Die,
    }
    protected State _curState { get; private set; } = State.NONE;
    protected bool _onCharging  = false;

    protected MonsterAppearBase _appearAction = null;


    public override void Init()
    {
        base.Init();
        // ThreeDSlider
    }
    public override void Release()
    {
        //_StopCoOfUpdateProjectileIntervalTimer();
        //_projectileChargingThreeDUI.Release();
        
        base.Release();
    }

    protected virtual void _SetState(State state)
    {
        if(_curState == state)
        {
            return;
        }

        _curState = state;
        _OnState(_curState);
    }

    protected virtual void _OnState(State state)
    {
        switch(_curState)
        {
            case State.Appear :
                {

                }
                break;

            case State.Idle :
                {

                }
                break;

            case State.Die :
                {

                }
                break;
        }
    }

    public override void ReadyToStartBattle()
    {
        _SetState(State.Appear);
    }
    public override void StartBattle()
    {
        _SetState(State.Idle);
    }
    public override void ReadyToCompleteBattle()
    {

    }
    public override void CompleteBattle()
    {
        // switch(nextInGameType)
        // {
        //     case InGameType. :
        //         {
        //             _SetState(State.Tada);
        //         }
        //         break;
        // }
    }
}
