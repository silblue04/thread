using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public class Main : Singleton<Main>
{
    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        _Init();
        _StartGame();
    } 

    private void _Init()
    {
        _InitGameSetting();
        _InitSreenResolution();
    }

    private void _InitGameSetting()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = DefsDefault.MAX_FRAME_RATE;
        Time.fixedDeltaTime = 1.0f / DefsDefault.MAX_FRAME_RATE;
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    private void _InitSreenResolution()
    {
        Vector2 screenSize = new Vector2(Screen.width, Screen.height);

        float maxW = DefsDefault.DEFAULT_SCREEN_WIDTH;
        if (screenSize.x > maxW)
        {
            float r = maxW / screenSize.x;
            screenSize.x *= r;
            screenSize.y *= r;
        }

        // Fillrate 방지
        Screen.SetResolution((int)screenSize.x, (int)screenSize.y, true);
    }


    [System.NonSerialized] bool _isGameStarted = false;
    public bool IsGameStarted { get { return _isGameStarted; } }
    private void _StartGame()
    {
        SceneManager.Instance.ChangeScene(SceneType.InGame, (result) =>
        {
            if (result)
            {
                _isGameStarted = true;
            }
        });
    }


    public IEnumerator CoInitCommonManagerAndData()
    {
        FirebaseHelper.InitMessaging();
        yield return StartCoroutine(FirebaseHelper.InitRemoteConfig());
        

        Queue<Action> parsingListQueue = new Queue<Action>();
        Metadata.GetCommonDataParsingListQueue(ref parsingListQueue);
        while (parsingListQueue.Count > 0)
        {
            parsingListQueue.Dequeue().Invoke();
            yield return YieldInstructionCache.WaitForEndOfFrame;
        }


        // AdsHelper.Instance.Init();
        // yield return YieldInstructionCache.WaitForEndOfFrame;

        LocalInfoConnecter.Instance.Init();
        yield return YieldInstructionCache.WaitForEndOfFrame;

        // IAPHelper.Instance.Init();
        // yield return YieldInstructionCache.WaitForEndOfFrame;
        
        // ConditionConnecter.Instance.Init();
        // yield return YieldInstructionCache.WaitForEndOfFrame;

        SoundManager.Instance.Init();
        yield return YieldInstructionCache.WaitForEndOfFrame;

        DG.Tweening.DOTween.SetTweensCapacity(5000, 50);
        yield return YieldInstructionCache.WaitForEndOfFrame;

        UIManager.Instance.Init();
        yield return YieldInstructionCache.WaitForEndOfFrame;

        TimerManager.Instance.Init();
        yield return YieldInstructionCache.WaitForEndOfFrame;
    }

    private void OnApplicationPause(bool isPaused)
    {
        if(_isGameStarted == false)
        {
            return;
        }


        Debug.LogFormat("[PAUSE] InternetReachability : {0} / isPaused : {1} --> {2}"
            , Application.internetReachability
            , isPaused
            , System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName);

        if(isPaused)
        {
            LocalInfoConnecter.Instance.SaveAllLocalInfo();
        }
    }

    private void OnApplicationQuit()
    {
        LocalInfoConnecter.Instance.SaveAllLocalInfo();

        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        SceneManager.DestroyInstance();
        UIManager.Instance.Release();
        UIManager.DestroyInstance();
        ResourceManager.Instance.Release();
        ResourceManager.DestroyInstance();
        SoundManager.Instance.ReleaseAudioEvents();
        SoundManager.DestroyInstance();
        //AdsHelper.DestroyInstance();
        LocalInfoConnecter.DestroyInstance();
        Metadata.ClearDatas();
    }
}
