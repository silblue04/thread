using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class RandomBox : UnitBase
{
    [Header("RandomBox > Size")]
    [SerializeField] private BoxCollider2D _collider;
    private Vector2 _Pos { get { return ((Vector2)_collider.transform.position) + _collider.offset; } }
    private Vector3 _Size { get { return _collider.size; } }

    private Rect RANDOM_BOX_ZONE_CENTER_RECT    = Rect.zero;

    [Header("RandomBox > ThreeDUI")]
    [SerializeField] private ThreeDSlider _essenceGauge;


    protected enum State
    {
        NONE = DefsDefault.VALUE_NONE,

        Idle,
        Moving,
    }

    protected State _curState { get; private set; } = State.NONE;
    private Vector3 _moveDir = Vector3.zero;


    public void Init(Rect RANDOM_BOX_ZONE_CENTER_RECT)
    {
        base.Init();

        this.RANDOM_BOX_ZONE_CENTER_RECT = RANDOM_BOX_ZONE_CENTER_RECT;

        _SetState(State.Idle);
    }
    public override void Release()
    {
        //_projectileChargingThreeDUI.Release();
        
        base.Release();
    }


    public void OnMove(Vector2 moveVector)
    {
        _moveDir = moveVector.normalized;

        _SetState(State.Moving);
    }
    public void OnStopMove()
    {
        _moveDir = Vector3.zero;

        _SetState(State.Idle);
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

    }

    private void FixedUpdate()
    {
        _UpdateMove();
        _UpdatePosToBeInSafeArea();
    }
    private void _UpdateMove()
    {
        if(_curState != State.Moving)
        {
            return;
        }

        this.transform.Translate(_moveDir * DefsUnit.RandomBox.MOVE_SPEED * Time.fixedDeltaTime);
    }
    private void _UpdatePosToBeInSafeArea()
    {
        if(_curState != State.Moving)
        {
            return;
        }


        Rect randomBoxRect      = new Rect(_Pos, _Size);
        randomBoxRect.center = _Pos;


        float xGap = 0.0f;
        if(RANDOM_BOX_ZONE_CENTER_RECT.xMin > randomBoxRect.xMin)
        {
            xGap = RANDOM_BOX_ZONE_CENTER_RECT.xMin - randomBoxRect.xMin;
        }
        else if(RANDOM_BOX_ZONE_CENTER_RECT.xMax < randomBoxRect.xMax)
        {
            xGap = RANDOM_BOX_ZONE_CENTER_RECT.xMax - randomBoxRect.xMax;
        }

        float yGap = 0.0f;
        if(RANDOM_BOX_ZONE_CENTER_RECT.yMin > randomBoxRect.yMin)
        {
            yGap = RANDOM_BOX_ZONE_CENTER_RECT.yMin - randomBoxRect.yMin;
        }
        else if(RANDOM_BOX_ZONE_CENTER_RECT.yMax < randomBoxRect.yMax)
        {
            yGap = RANDOM_BOX_ZONE_CENTER_RECT.yMax - randomBoxRect.yMax;
        }

        this.transform.position = new Vector3
        (
            this.transform.position.x + xGap
            , this.transform.position.y + yGap
            , this.transform.position.z
        );
    }

    public override void ReadyToStartBattle()
    {

    }
    public override void StartBattle()
    {

    }
    public override void ReadyToCompleteBattle()
    {

    }
    public override void CompleteBattle()
    {

    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(DefsUnit.TriggerName.ESSENCE))
        {
            // TODO : 정보 가지고 와서 게이지 차면 된다
        }
    }
}
