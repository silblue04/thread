using UnityEngine;
using UnityEditor;

public class PlayTestWindow : EditorWindow
{
    // Basic
    private string _gold = "940608";
    private int _gem = 940120;
    private int _medal = 840519;
    private string _language = Metadata.Languages[0];

    // Stage
    private int _stage_idx = 0;

    // Quest
    private int _questOrderIndex = 0;


    [MenuItem("Window/PlayTest ")]
    static void Init()
    {
        PlayTestWindow window = (PlayTestWindow)EditorWindow.GetWindow(typeof(PlayTestWindow));
        window.Show();
    }

    void OnSelectionChange()
    {
        Repaint();
    }

    // public void OnInspectorUpdate()
    // {
    //     Repaint();
    // }

    int GetSelectObjectCount()
    {
        return UnityEditor.Selection.objects.Length;
    }

    Object[] GetSelectedObject()
    {
        return UnityEditor.Selection.objects;
    }

    // https://debuglog.tistory.com/30
    // https://docs.unity3d.com/kr/530/ScriptReference/EditorGUILayout.html
    void OnGUI()
    {
        if (UnityEditor.EditorApplication.isPlaying == false
            || SceneManager.Instance.CurSceneType != SceneType.InGame)
        {
            return;
        }

        // EditorGUILayout.Space();
        // GUILayout.Label("-- Basic Curreny ------", EditorStyles.boldLabel);
        // GUILayout.BeginHorizontal();
        // {
        //     _gold = EditorGUILayout.TextField("Gold", _gold);
        //     if (GUILayout.Button("Get Gold", GUILayout.Width(100)))
        //     {
        //         if (!string.IsNullOrEmpty(_gold))
        //         {
        //             System.Numerics.BigInteger getGold = 0;
        //             if (System.Numerics.BigInteger.TryParse(_gold, out getGold))
        //             {
        //                 LocalInventoryInfoContainer LocalInventoryInfoContainer = LocalInfoConnecter.Instance.LocalInventoryInfoContainer;
        //                 LocalInventoryInfoContainer.AddItem(BasicCurrencyType.Gold, getGold);
        //             }
        //         }
        //     }
        // }
        // GUILayout.EndHorizontal();

        // GUILayout.BeginHorizontal();
        // {
        //     _gem = EditorGUILayout.IntField("Gem", _gem);
        //     if (GUILayout.Button("Get Gem", GUILayout.Width(100)))
        //     {
        //         if (_gem > 0)
        //         {
        //             LocalInventoryInfoContainer LocalInventoryInfoContainer = LocalInfoConnecter.Instance.LocalInventoryInfoContainer;
        //             LocalInventoryInfoContainer.AddItem(BasicCurrencyType.Gem, _gem);
        //         }
        //     }
        // }
        // GUILayout.EndHorizontal();

        // GUILayout.BeginHorizontal();
        // {
        //     _medal = EditorGUILayout.IntField("Medal", _medal);
        //     if (GUILayout.Button("Get Medal", GUILayout.Width(100)))
        //     {
        //         LocalInventoryInfoContainer LocalInventoryInfoContainer = LocalInfoConnecter.Instance.LocalInventoryInfoContainer;
        //         LocalInventoryInfoContainer.AddItem(BasicCurrencyType.Medal, _medal);
        //     }
        // }
        // GUILayout.EndHorizontal();

        // EditorGUILayout.Space();
        // GUILayout.Label("-- Upgrade Worker ------", EditorStyles.boldLabel);
        // GUILayout.BeginHorizontal();
        // {
        //     if (GUILayout.Button("Set Worker Max Level", GUILayout.Width(200)))
        //     {
        //         LocalWorkerInfo LocalWorkerInfo = LocalInfoConnecter.Instance.LocalWorkerInfo;
        //         LocalWorkerInfo.SetMaxLevel();
        //     }
        //     if (GUILayout.Button("Set Worker Zero Level", GUILayout.Width(200)))
        //     {
        //         LocalWorkerInfo LocalWorkerInfo = LocalInfoConnecter.Instance.LocalWorkerInfo;
        //         LocalWorkerInfo.SetZeroLevel();
        //     }
        // }
        // GUILayout.EndHorizontal();

        // EditorGUILayout.Space();
        // GUILayout.Label("-- Stage ------", EditorStyles.boldLabel);
        // GUILayout.BeginHorizontal();
        // {
        //     if (GUILayout.Button("Go to Next Pak", GUILayout.Width(120)))
        //     {
        //         LocalStageInfo LocalStageInfo = LocalInfoConnecter.Instance.LocalStageInfo;
        //         LocalStageInfo.test_OnGoToNextPak();
        //         InGameLevelManager.Instance.ReadyToCompleteBattle();
        //     }
        //     if (GUILayout.Button("Complete Battle", GUILayout.Width(120)))
        //     {
        //         InGameLevelManager.Instance.ReadyToCompleteBattle();
        //     }
        //     if (GUILayout.Button("Set Max need_battle_count", GUILayout.Width(200)))
        //     {
        //         LocalStageInfo LocalStageInfo = LocalInfoConnecter.Instance.LocalStageInfo;
        //         LocalStageInfo.test_SetMaxNeedBattleCount();
        //     }
        //     if (GUILayout.Button("Get Prestige", GUILayout.Width(100)))
        //     {
        //         LocalInGameInfoContainer LocalInGameInfoContainer = LocalInfoConnecter.Instance.LocalInGameInfoContainer;
        //         LocalInGameInfoContainer.GetPrestige(false);
                
        //         GetPrestigeData getPrestigeData = new GetPrestigeData
        //         (
        //             0
        //             , PrestigeType.Prestige
        //             , false
        //         );
        //         InGameLevelManager.Instance.ReadyToGetPrestige(getPrestigeData);
        //     }
        //     // GUILayout.BeginHorizontal();
        //     // {
        //     //     _stage_idx = EditorGUILayout.IntField("Tagget Stage Idx", _stage_idx);
        //     //     if (GUILayout.Button("Go", GUILayout.Width(50)))
        //     //     {
        //     //         if (_stage_idx > 0)
        //     //         {
        //     //             LocalStageInfo LocalStageInfo = LocalInfoConnecter.Instance.LocalStageInfo;
        //     //             LocalStageInfo.test_OnGoToStage(_stage_idx);
        //     //             InGameLevelManager.Instance.ReadyToCompleteBattle();
        //     //         }
        //     //     }
        //     // }
        //     // GUILayout.EndHorizontal();
        // }
        // GUILayout.EndHorizontal();
        // GUILayout.BeginHorizontal();
        // {
        //     _stage_idx = EditorGUILayout.IntField("Target Stage Idx", _stage_idx);
        //     if (GUILayout.Button("Go", GUILayout.Width(50)))
        //     {
        //         if (_stage_idx > 0)
        //         {
        //             LocalStageInfo LocalStageInfo = LocalInfoConnecter.Instance.LocalStageInfo;
        //             LocalStageInfo.test_OnGoToStage(_stage_idx);
        //             InGameLevelManager.Instance.ReadyToCompleteBattle();
        //         }
        //     }
        // }
        // GUILayout.EndHorizontal();


        // EditorGUILayout.Space();
        // GUILayout.Label("-- Others ------", EditorStyles.boldLabel);
        // GUILayout.BeginHorizontal();
        // {
        //     _questOrderIndex = EditorGUILayout.IntField("Quest Order Index", _questOrderIndex);
        //     if (GUILayout.Button("ChangeQuest", GUILayout.Width(120)))
        //     {
        //         LocalQuestInfo LocalQuestInfo = LocalInfoConnecter.Instance.LocalQuestInfo;
        //         LocalQuestInfo.test_ChangeQuest(_questOrderIndex);
        //     }
        // }
        // GUILayout.EndHorizontal();
    }
}