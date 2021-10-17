using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Attacker : UnitBase
{
    [Header("AttackerBase > AttackCoolTimeGauge")]
    [SerializeField] protected ThreeDSlider _attackCoolTimeGauge;
    
    protected enum State
    {
        NONE = DefsDefault.VALUE_NONE,

        Idle,
        Attack,

        Tada,
    }
    protected State _curState { get; private set; } = State.NONE;
    protected bool _onCharging  = false;

    protected AttackerAttackBase _attackAction = null;
    

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
            case State.Idle :
                {

                }
                break;

            case State.Attack :
                {

                }
                break;
        }
    }

    public override void ReadyToStartBattle()
    {
        _SetState(State.Idle);
    }
    public override void StartBattle()
    {
        _SetState(State.Attack);
    }
    public override void ReadyToCompleteBattle()
    {
        _SetState(State.Idle);
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
