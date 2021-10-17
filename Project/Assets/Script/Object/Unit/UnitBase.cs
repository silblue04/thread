using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class UnitBase
    : ObjectBase
    , IReadyToStartBattle , IStartBattle, IReadyToCompleteBattle, ICompleteBattle
{
    [Header("UnitBase > Renderer")]
    [SerializeField] protected Animation _unitAnimation;

    protected int _unit_idx {get; private set; } = DefsDefault.VALUE_NONE;
    //protected int _unitLevel = DefsDefault.VALUE_ZERO;


    public virtual void Init()
    {
        InGameRoutineManager.Instance.AddReceiver(this);
    }
    public virtual void Release()
    {
        InGameRoutineManager.Instance.RemoveReceiver(this);
    }


    public abstract void ReadyToStartBattle();
    public abstract void StartBattle();
    public abstract void ReadyToCompleteBattle();
    public abstract void CompleteBattle();
}
