using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class InGameScene : Scene
{
    protected override void _OnSceneStart()
    {
        base._OnSceneStart();
        UIManager.Instance.Show<InGameUI>(DefsUI.Prefab.INGAME_UI, null, false, true);

        InGameRoutineManager.Instance.LoadLevel(InGameType.General);
        InGameLevelManager.Instance.ReadyToStartChapter();
    }

    protected override void _OnSceneEnd()   
    {
        UIManager.Instance.Close<InGameUI>();
        UIManager.Instance.CloseAll();

        InGameRoutineManager.Instance.Release();
        InGameRoutineManager.DestroyInstance();
        base._OnSceneEnd();
    }
}
