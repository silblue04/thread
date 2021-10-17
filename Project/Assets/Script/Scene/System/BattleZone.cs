using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleZone : MonoBehaviour
{
    [Header("BattleZone")]
    [SerializeField] private ObjectSize _BattleZoneSize;

    [Header("Units")]
    [SerializeField] private Attacker[] _attackerList = new Attacker[DefsInGame.UNIT_ATTACTER_SLOT_NUM];
    [SerializeField] private Monster[] _monsterList = new Monster[DefsInGame.UNIT_MONSTER_SLOT_NUM];


    public void Init()
    {
        _InitUnits();
    }
    public void Release()
    {
        _ReleaseUnits();
    }

    private void _InitUnits()
    {
        for(int i = 0; i < _attackerList.Length; ++i)
        {
            _attackerList[i].Init();
        }
        for(int i = 0; i < _monsterList.Length; ++i)
        {
            _monsterList[i].Init();
        }
    }
    private void _ReleaseUnits()
    {
        for(int i = 0; i < _attackerList.Length; ++i)
        {
            _attackerList[i].Release();
        }
        for(int i = 0; i < _monsterList.Length; ++i)
        {
            _monsterList[i].Release();
        }
    }
}
