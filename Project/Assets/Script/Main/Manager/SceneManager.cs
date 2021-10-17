using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


// public class SceneStackData
// {
//     public SceneType type;

//     public SceneStackData(SceneType type, string name)
//     {
//         this.type = type;
//     }
// }

public class SceneManager : Singleton<SceneManager>
{
    //private Stack<SceneStackData> _sceneStack = new Stack<SceneStackData>();

    private SceneType _prevSceneType = SceneType.NONE;

    private Scene _curScene = null;
    public Scene CurScene { get { return _curScene; } }
    public SceneType CurSceneType { get { return (_curScene != null) ? _curScene.Type : SceneType.NONE; } }

    private bool _isLoadingSceneStarted = false;
    private float _loadingSceneProgressPercentage = DefsDefault.MIN_PERCENTAGE;
    public float LoadingSceneProgressPercentage
    {
        get
        {
            return ((_isLoadingSceneStarted)
                ? _loadingSceneProgressPercentage : DefsDefault.MIN_PERCENTAGE) * 100.0f;
        }
    }


    public void ChangeScene(SceneType type, Action<bool> callbackResult = null)
    {
        string sceneName = DefsScene.GetSceneName(type);
        if(sceneName == string.Empty)
        {
            return;
        }
        

        UIManager.Instance.ShowPopupLoading(true);
        UIManager.Instance.SetPopupLoadingType(PopupLoading.LoadingType.Necessary);

        Action loadScene = () =>
        {
            _LoadScene(sceneName, (loadResult) =>
            {
                UIManager.Instance.ShowPopupLoading(false);
                callbackResult?.Invoke(loadResult);
            });
        };
        

        if(_curScene != null)
        {
            _UnloadScene(_curScene.SceneName, (unloadResult) =>
            {
                if(unloadResult)
                {
                    loadScene.Invoke();
                }
            });
        }
        else
        {
            loadScene.Invoke();
        }
    }

    private void _UnloadScene(string sceneName, Action<bool> callbackResult)
    {
        StartCoroutine(_CoUnloadScene(sceneName, callbackResult));
    }

    private void _LoadScene(string sceneName, Action<bool> callbackResult)
    {
        StartCoroutine(_CoLoadScene(sceneName, callbackResult, (progressPercentage) =>
        {
            
        }));
    }

    private IEnumerator _CoUnloadScene(string sceneName, Action<bool> callbackResult)
    {
        AsyncOperation asyncOperation
            = UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(sceneName);

#if UNITY_EDITOR
        if(asyncOperation == null)
        {
            Debug.LogFormat("[Fail] Failed Unload Scene Async : {0}", sceneName);
            callbackResult?.Invoke(false);
            yield break;
        }
#endif

        while (asyncOperation.isDone == false)
        {
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        yield return YieldInstructionCache.WaitForEndOfFrame;

        callbackResult?.Invoke(true);
        yield break;
    }

    private IEnumerator _CoLoadScene(string sceneName, Action<bool> callbackResult, Action<float> callbackProgressPercentage)
    {
        yield return YieldInstructionCache.WaitForEndOfFrame;
        
        if(Main.Instance.IsGameStarted == false)
        {
            yield return StartCoroutine(Main.Instance.CoInitCommonManagerAndData());
        }


        AsyncOperation asyncOperation
            = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(
                sceneName
                , UnityEngine.SceneManagement.LoadSceneMode.Additive);

#if UNITY_EDITOR
        if(asyncOperation == null)
        {
            Debug.LogFormat("[Fail] Failed Load Scene Async : {0}", sceneName);
            callbackResult?.Invoke(false);
            yield break;
        }
#endif

        while(asyncOperation == null)
        {
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        while (asyncOperation.isDone == false)
        {
            callbackProgressPercentage?.Invoke(asyncOperation.progress);
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }

        callbackProgressPercentage?.Invoke(1.0f);
        callbackResult?.Invoke(true);

        yield break;
    }

    

    public void SetCurScene(Scene curScene)
    {
        _prevSceneType = CurSceneType;
        _curScene = curScene;
    }

    public override void OnDestroy()
    {
        if(_curScene != null)
        {
            DestroyImmediate(_curScene);
            _curScene = null;
        }

        base.OnDestroy();
    }
}
