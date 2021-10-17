using UnityEngine;
using UnityEditor;

public class EditorTool : ScriptableObject
{
    [UnityEditor.MenuItem("April11th/Open PlayTest Window _F1")]
    static void OpenPlayTestWindow()
    {
        PlayTestWindow window = (PlayTestWindow)EditorWindow.GetWindow(typeof(PlayTestWindow));
        window.Show();
    }
    [UnityEditor.MenuItem("April11th/Open Worker Window _F2")]
    static void OpenWorkerWindow()
    {
        WorkerWindow window = (WorkerWindow)EditorWindow.GetWindow(typeof(WorkerWindow));
        window.Show();
    }
    [UnityEditor.MenuItem("April11th/Open Mercenary Window _F3")]
    static void OpenMercenaryWindow()
    {
        MercenaryWindow window = (MercenaryWindow)EditorWindow.GetWindow(typeof(MercenaryWindow));
        window.Show();
    }

    [UnityEditor.MenuItem("April11th/Go to MainScene And then Play _F5")]
    static void GoToMainSceneAndThenPlay()
    {
        if(UnityEditor.EditorApplication.isPlaying)
        {
            return;
        }
        if(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().isDirty)
        {
            UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }

        GoToMainScene();
        UnityEditor.EditorApplication.isPlaying = true;
    }

    [UnityEditor.MenuItem("April11th/Go to MainScene _F6")]
    static void GoToMainScene()
    {
        if(UnityEditor.EditorApplication.isPlaying)
        {
            return;
        }
        if(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().isDirty)
        {
            UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }

        UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/Main.unity");
    }
    [UnityEditor.MenuItem("April11th/Go to InGameScene _F7")]
    static void GoToInGameScene()
    {
        if(UnityEditor.EditorApplication.isPlaying)
        {
            return;
        }
        if(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().isDirty)
        {
            UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
        }

        UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Scenes/InGame.unity");
    }


    // [UnityEditor.MenuItem("April11th/Go to ToolScene And then Play _F12")]
    // static void GoToToolScene()
    // {
    //     if(UnityEditor.EditorApplication.isPlaying)
    //     {
    //         return;
    //     }
    //     if(UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().isDirty)
    //     {
    //         UnityEditor.SceneManagement.EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
    //     }

    //     UnityEditor.SceneManagement.EditorSceneManager.OpenScene("Assets/Tool/Tool.unity");
    // }

    // [UnityEditor.MenuItem("April11th/Show Popup _F12")]
    // static void ShowPopup()
    // {
    //     if(UnityEditor.EditorApplication.isPlaying == false)
    //     {
    //         return;
    //     }

    //     //UIManager.Instance.Show<PopupGallery>(DefsUI.Prefab.POPUP_GALLERY);
    // }
}
