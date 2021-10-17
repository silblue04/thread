using UnityEngine;
using UnityEditor;

public class WorkerWindow : EditorWindow
{
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
        // GUILayout.Label(string.Format("-- unit_idx {0} : Stat ------", DefsUnit.WORKER_UNIT_IDX) , EditorStyles.boldLabel);
        // GUILayout.BeginHorizontal();
        // {
        //     GUILayout.Label("Stat Name", EditorStyles.miniBoldLabel);
        //     GUILayout.Label("Base Stat", EditorStyles.label);
        //     GUILayout.Label("Total Stat", EditorStyles.label);
        // }
        // GUILayout.EndHorizontal();

        // Stat baseStat = StatManager.Instance.GetBaseStat(UnitType.Worker);
        // Stat totalStat = StatManager.Instance.GetTotalStat(UnitType.Worker);
        
        // int MAX_STAT_TPYE = BitConvert.Enum32ToInt(StatType.MAX);
        // for(int i = 0; i < MAX_STAT_TPYE; ++i)
        // {
        //     StatType stat_type = (StatType)i;
        //     GUILayout.BeginHorizontal();
        //     {
        //         //_stat.SetValue(statType );
        //         GUILayout.Label(stat_type.ToString(), EditorStyles.miniBoldLabel);
        //         GUILayout.Label(baseStat.GetValue(stat_type).ToString(), EditorStyles.label);
        //         GUILayout.Label(totalStat.GetValue(stat_type).ToString(), EditorStyles.label);
        //     }
        //     GUILayout.EndHorizontal();
        // }
    }
}