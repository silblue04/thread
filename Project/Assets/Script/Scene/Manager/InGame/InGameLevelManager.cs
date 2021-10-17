using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public class InGameLevelManager : Singleton<InGameLevelManager>
{
    [Header("System")]
    [SerializeField] private ChapterBg _chapterBg;
    [SerializeField] private RandomBoxZone _randomBoxZone;

    private InGameType _inGameType = InGameType.General;


    public void Init(InGameType inGameType)
    {
        _inGameType = inGameType;

        _chapterBg.Init();
        _randomBoxZone.Init();
    }
    public void Release()
    {
        _randomBoxZone.Release();
        _chapterBg.Release();
    }


    public void ReadyToStartChapter()
    {
        _chapterBg.ChangeChapterBg( () =>
        {
            StartChapter();
        });
    }
    public void StartChapter()
    {
        
    }
    public void ReadyToStartStage()
    {
        // TODO : 몬스터 생성
    }
    public void StartStage()
    {
        
    }
    public void ReadyToStartBattle()
    {
        // TODO : 몬스터 등장
    }
    public void StartBattle()
    {

    }

    public void SkipBattle(SkipBossBattleType skipBattleType)
    {

    }

    public void ReadyToCompleteBattle()
    {

    }
    public void CompleteBattle()
    {

    }
    public void ReadyToCompleteStage()
    {

    }
    public void CompleteStage()
    {

    }
    public void ReadyToCompleteChapter()
    {

    }
    public void CompleteChapter()
    {

    }


    public void OnDrag(Vector2 dragVector)
    {
        _randomBoxZone.OnMoveRandomBox(dragVector);
    }
    public void OnEndDrag()
    {
        _randomBoxZone.OnStopMoveRandomBox();
    }
}
