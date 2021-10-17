using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class InGameRoutineManager : Singleton<InGameRoutineManager>
{
    private List<IReadyToStartChapter> _readyToStartChapterReceiverList       = new List<IReadyToStartChapter>();
    private List<IStartChapter> _startChapterReceiverList                     = new List<IStartChapter>();

    private List<IReadyToStartStage> _readyToStartStageReceiverList       = new List<IReadyToStartStage>();
    private List<IStartStage> _startStageReceiverList                     = new List<IStartStage>();

    private List<IReadyToStartBattle> _readyToStartBattleReceiverList       = new List<IReadyToStartBattle>();
    private List<IStartBattle> _startBattleReceiverList                     = new List<IStartBattle>();
    
    // 

    private List<IReadyToCompleteBattle> _readyToCompleteBattleReceiverList = new List<IReadyToCompleteBattle>();
    private List<ICompleteBattle> _completeBattleReceiverList               = new List<ICompleteBattle>();

    private List<IReadyToCompleteStage> _readyToCompleteStageReceiverList = new List<IReadyToCompleteStage>();
    private List<ICompleteStage> _completeStageReceiverList               = new List<ICompleteStage>();

    private List<IReadyToCompleteChapter> _readyToCompleteChapterReceiverList = new List<IReadyToCompleteChapter>();
    private List<ICompleteChapter> _completeChapterReceiverList               = new List<ICompleteChapter>();


    public void LoadLevel(InGameType inGameType)
    {
        ObjectPoolManager.Instance.Init();

        //AddOnManager.Instance.Init();
        //StatManager.Instance.Init();
        InGameLevelManager.Instance.Init(inGameType);
    }
    public void Release()
    {
        InGameLevelManager.Instance.Release();
        //StatManager.Instance.Release();
        //AddOnManager.Instance.Release();

        ObjectPoolManager.Instance.Release();
        ObjectPoolManager.DestroyInstance();
    }


    public void AddReceiver<T>(T instance) where T : class
    {
        _AddReceiver(instance);
    }
    public void RemoveReceiver<T>(T instance) where T : class
    {
        _RemoveReceiver(instance);
    }

    private void _AddReceiver<T>(T instance )
    {
        Type type = instance.GetType();


        if (typeof(IReadyToStartChapter).IsAssignableFrom(type))
        {
            _readyToStartChapterReceiverList.Add((IReadyToStartChapter)instance);
        }
        if (typeof(IStartChapter).IsAssignableFrom(type))
        {
            _startChapterReceiverList.Add((IStartChapter)instance);
        }
        if (typeof(IReadyToStartStage).IsAssignableFrom(type))
        {
            _readyToStartStageReceiverList.Add((IReadyToStartStage)instance);
        }
        if (typeof(IStartStage).IsAssignableFrom(type))
        {
            _startStageReceiverList.Add((IStartStage)instance);
        }
        if (typeof(IReadyToStartBattle).IsAssignableFrom(type))
        {
            _readyToStartBattleReceiverList.Add((IReadyToStartBattle)instance);
        }
        if (typeof(IStartBattle).IsAssignableFrom(type))
        {
            _startBattleReceiverList.Add((IStartBattle)instance);
        }

        if (typeof(IReadyToCompleteBattle).IsAssignableFrom(type))
        {
            _readyToCompleteBattleReceiverList.Add((IReadyToCompleteBattle)instance);
        }
        if (typeof(ICompleteBattle).IsAssignableFrom(type))
        {
            _completeBattleReceiverList.Add((ICompleteBattle)instance);
        }
        if (typeof(IReadyToCompleteStage).IsAssignableFrom(type))
        {
            _readyToCompleteStageReceiverList.Add((IReadyToCompleteStage)instance);
        }
        if (typeof(ICompleteStage).IsAssignableFrom(type))
        {
            _completeStageReceiverList.Add((ICompleteStage)instance);
        }
        if (typeof(IReadyToCompleteChapter).IsAssignableFrom(type))
        {
            _readyToCompleteChapterReceiverList.Add((IReadyToCompleteChapter)instance);
        }
        if (typeof(ICompleteChapter).IsAssignableFrom(type))
        {
            _completeChapterReceiverList.Add((ICompleteChapter)instance);
        }
    }
    private void _RemoveReceiver<T>(T instance)
    {
        Type type = instance.GetType();


        if (typeof(IReadyToStartChapter).IsAssignableFrom(type))
        {
            _readyToStartChapterReceiverList.Remove((IReadyToStartChapter)instance);
        }
        if (typeof(IStartChapter).IsAssignableFrom(type))
        {
            _startChapterReceiverList.Remove((IStartChapter)instance);
        }
        if (typeof(IReadyToStartStage).IsAssignableFrom(type))
        {
            _readyToStartStageReceiverList.Remove((IReadyToStartStage)instance);
        }
        if (typeof(IStartStage).IsAssignableFrom(type))
        {
            _startStageReceiverList.Remove((IStartStage)instance);
        }
        if (typeof(IStartBattle).IsAssignableFrom(type))
        {
            _startBattleReceiverList.Remove((IStartBattle)instance);
        }
        if (typeof(IReadyToStartBattle).IsAssignableFrom(type))
        {
            _readyToStartBattleReceiverList.Remove((IReadyToStartBattle)instance);
        }

        if (typeof(IReadyToCompleteBattle).IsAssignableFrom(type))
        {
            _readyToCompleteBattleReceiverList.Remove((IReadyToCompleteBattle)instance);
        }
        if (typeof(ICompleteBattle).IsAssignableFrom(type))
        {
            _completeBattleReceiverList.Remove((ICompleteBattle)instance);
        }
        if (typeof(IReadyToCompleteStage).IsAssignableFrom(type))
        {
            _readyToCompleteStageReceiverList.Remove((IReadyToCompleteStage)instance);
        }
        if (typeof(ICompleteStage).IsAssignableFrom(type))
        {
            _completeStageReceiverList.Remove((ICompleteStage)instance);
        }
        if (typeof(IReadyToCompleteChapter).IsAssignableFrom(type))
        {
            _readyToCompleteChapterReceiverList.Remove((IReadyToCompleteChapter)instance);
        }
        if (typeof(ICompleteChapter).IsAssignableFrom(type))
        {
            _completeChapterReceiverList.Remove((ICompleteChapter)instance);
        }
    }


    public void ReadyToStartChapter()
    {
        foreach (var receivier in _readyToStartChapterReceiverList)
        {
            receivier.ReadyToStartChapter();
        }
    }
    public void StartChapter()
    {
        foreach (var receivier in _startChapterReceiverList)
        {
            receivier.StartChapter();
        }
    }
    public void ReadyToStartStage()
    {
        foreach (var receivier in _readyToStartStageReceiverList)
        {
            receivier.ReadyToStartStage();
        }
    }
    public void StartStage()
    {
        foreach (var receivier in _startStageReceiverList)
        {
            receivier.StartStage();
        }
    }
    public void ReadyToStartBattle()
    {
        foreach (var receivier in _readyToStartBattleReceiverList)
        {
            receivier.ReadyToStartBattle();
        }
    }
    public void StartBattle()
    {
        foreach (var receivier in _startBattleReceiverList)
        {
            receivier.StartBattle();
        }
    }

    public void ReadyToCompleteBattle()
    {
        foreach (var receivier in _readyToCompleteBattleReceiverList)
        {
            receivier.ReadyToCompleteBattle();
        }
    }
    public void CompleteBattle()
    {
        foreach (var receivier in _completeBattleReceiverList)
        {
            receivier.CompleteBattle();
        }
    }
    public void ReadyToCompleteStage()
    {
        foreach (var receivier in _readyToCompleteStageReceiverList)
        {
            receivier.ReadyToCompleteStage();
        }
    }
    public void CompleteStage()
    {
        foreach (var receivier in _completeStageReceiverList)
        {
            receivier.CompleteStage();
        }
    }
    public void ReadyToCompleteChapter()
    {
        foreach (var receivier in _readyToCompleteChapterReceiverList)
        {
            receivier.ReadyToCompleteChapter();
        }
    }
    public void CompleteChapter()
    {
        foreach (var receivier in _completeChapterReceiverList)
        {
            receivier.CompleteChapter();
        }
    }
}
