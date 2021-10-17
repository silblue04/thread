using UnityEngine;
using UnityEditor;

public class MercenaryWindow : EditorWindow
{
    private int _curMercenarySlotTypeIndex = 0;


    [MenuItem("Window/Worker ")]
    static void Init()
    {
        WorkerWindow window = (WorkerWindow)EditorWindow.GetWindow(typeof(WorkerWindow));
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

    void Update()
    {
        Repaint();
    }


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
        // GUILayout.Label("-- Select Mercenary Slot Type ------", EditorStyles.boldLabel);
        // GUILayout.BeginHorizontal();
        // {
        //     _curMercenarySlotTypeIndex = EditorGUILayout.Popup(_curMercenarySlotTypeIndex, DefsString.MercenarySlotTypeString);
        // }
        // GUILayout.EndHorizontal();
        // GUILayout.BeginHorizontal();
        // {
        //     GUILayout.Label("Stat Name", EditorStyles.miniBoldLabel);
        //     GUILayout.Label("Base Stat", EditorStyles.label);
        //     GUILayout.Label("Total Stat", EditorStyles.label);
        // }
        // GUILayout.EndHorizontal();


        // EditorGUILayout.Space();
        // // for(int i = 0; i < DefsUnit.MERCENARY_SLOT_TYPE_NUM; ++i)
        // // {
        //     var mercenaryData = Metadata.MercenaryContainer.GetDataByMercenarySlotTypeIndex(_curMercenarySlotTypeIndex);

        //     GUILayout.Label(string.Format("-- unit_idx {0} : Stat ------", mercenaryData.idx) , EditorStyles.boldLabel);

        //     Stat baseStat = StatManager.Instance.GetBaseStat(UnitType.Mercenary, mercenaryData.idx);
        //     Stat totalStat = StatManager.Instance.GetTotalStat(UnitType.Mercenary, mercenaryData.idx);
            
        //     int MAX_STAT_TPYE = BitConvert.Enum32ToInt(StatType.MAX);
        //     for(int j = 0; j < MAX_STAT_TPYE; ++j)
        //     {
        //         StatType stat_type = (StatType)j;
        //         GUILayout.BeginHorizontal();
        //         {
        //             //_stat.SetValue(statType );
        //             GUILayout.Label(stat_type.ToString(), EditorStyles.miniBoldLabel);
        //             GUILayout.Label(baseStat.GetValue(stat_type).ToString(), EditorStyles.label);
        //             GUILayout.Label(totalStat.GetValue(stat_type).ToString(), EditorStyles.label);
        //         }
        //         GUILayout.EndHorizontal();
        //     }

        // //     EditorGUILayout.Space();
        // //     EditorGUILayout.Space();
        // // }
    }
}