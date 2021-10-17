#if UNITY_EDITOR
using UnityEditor.Experimental.SceneManagement;
using UnityEditor;
using UnityEngine;

public static class EditUtil
{
    public static void TouchPrefab()
    {
        var prefabStage = GetCurrentPrefabStage();
        if (prefabStage != null)
        {
            UnityEditor.SceneManagement.EditorSceneManager.MarkSceneDirty(prefabStage.scene);
        }
    }

    public static PrefabStage GetPrefabStage(GameObject go)
    {
        return PrefabStageUtility.GetPrefabStage(go);
    }

    public static PrefabStage GetCurrentPrefabStage()
    {
        return PrefabStageUtility.GetCurrentPrefabStage();
    }
}

#endif