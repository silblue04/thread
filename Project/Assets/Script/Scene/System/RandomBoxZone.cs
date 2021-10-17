using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomBoxZone : MonoBehaviour
{
    [Header("RandomBoxZone")]
    [SerializeField] private ObjectSize _randomBoxZoneSize;
    [SerializeField] private int _splitXNum = DefsDefault.VALUE_ZERO;
    [SerializeField] private int _splitYNum = DefsDefault.VALUE_ZERO;
    [SerializeField] private float _borderSpareLenght = 0.5f;

    private Rect RANDOM_BOX_ZONE_CENTER_RECT    = Rect.zero;
    private Vector2 SPLITED_BOX_SIZE            = Vector2.zero;

    [Header("RandomBox")]
    [SerializeField] private RandomBox _randomBox;


    public void Init()
    {
        _InitConstSize();
        _randomBox.Init(RANDOM_BOX_ZONE_CENTER_RECT);
    }
    public void Release()
    {
        _randomBox.Release();
    }


    private void _InitConstSize()
    {
        RANDOM_BOX_ZONE_CENTER_RECT        = _randomBoxZoneSize.Rect;
        RANDOM_BOX_ZONE_CENTER_RECT.center = _randomBoxZoneSize.Pos;

        SPLITED_BOX_SIZE = new Vector2
        (
            _randomBoxZoneSize.Size.x / (float)_splitXNum
            , _randomBoxZoneSize.Size.y / (float)_splitYNum
        );

    }


    public void OnMoveRandomBox(Vector2 moveVector)
    {
        _randomBox.OnMove(moveVector);
    }
    public void OnStopMoveRandomBox()
    {
        _randomBox.OnStopMove();
    }

    private void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space)) // TODO : tmp
        {
            CreateEssence();
        }
    }

    public void CreateEssence()
    {
        int SPLITED_RECT_NUM = _splitXNum * _splitYNum;
        int splitedRectIndex = Random.Range(DefsDefault.VALUE_ZERO, SPLITED_RECT_NUM);

        int splitedRectXIndex = splitedRectIndex % _splitXNum;
        int splitedRectYIndex = splitedRectIndex / _splitXNum;


        float splitedRectRectMinX = RANDOM_BOX_ZONE_CENTER_RECT.xMin + (SPLITED_BOX_SIZE.x * splitedRectXIndex) + _borderSpareLenght;
        float splitedRectRectMaxX = splitedRectRectMinX + SPLITED_BOX_SIZE.x - _borderSpareLenght;
        float splitedRectRectMinY = RANDOM_BOX_ZONE_CENTER_RECT.yMin + (SPLITED_BOX_SIZE.y * splitedRectYIndex) + _borderSpareLenght;
        float splitedRectRectMaxY = splitedRectRectMinY + SPLITED_BOX_SIZE.y - _borderSpareLenght;

        Vector2 essencePos = new Vector2
        (
            Random.Range(splitedRectRectMinX, splitedRectRectMaxX)
            , Random.Range(splitedRectRectMinY, splitedRectRectMaxY)
        );

        Essence instance = ObjectPoolManager.Instance.CreateObject<Essence>(ObjectType.Essence);
        instance.Init(essencePos);
    }


    private void OnDrawGizmosSelected()
    {
        Rect randomBoxZoneRect  = _randomBoxZoneSize.Rect;
        randomBoxZoneRect.center = _randomBoxZoneSize.Pos;


        Vector2 splitSize = new Vector2
        (
            _randomBoxZoneSize.Size.x / (float)_splitXNum
            , _randomBoxZoneSize.Size.y / (float)_splitYNum
        );

        int xLineNum = _splitXNum - 1;
        int yLineNum = _splitYNum - 1;

        Gizmos.color = Color.yellow;

        for(int i = 1; i <= xLineNum; ++i)
        {
            Gizmos.DrawLine
            (
                randomBoxZoneRect.min + new Vector2(splitSize.x * i, 0.0f)
                , randomBoxZoneRect.max + new Vector2(-splitSize.x * (_splitXNum - i), 0.0f)
            );
        }
        for(int i = 1; i <= yLineNum; ++i)
        {
            Gizmos.DrawLine
            (
                randomBoxZoneRect.min + new Vector2(0.0f, splitSize.y * i)
                , randomBoxZoneRect.max + new Vector2(0.0f, -splitSize.y * (_splitYNum - i))
            );
        }
    }
}
