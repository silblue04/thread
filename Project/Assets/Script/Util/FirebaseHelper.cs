using Firebase.Analytics;
using Firebase.RemoteConfig;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;


public class FirebaseHelper
{
    static public Firebase.FirebaseApp app = null;
 
    static public void InitMessaging()
    {
#if !UNITY_EDITOR
        Firebase.Messaging.FirebaseMessaging.TokenReceived += OnTokenReceived;
        Firebase.Messaging.FirebaseMessaging.MessageReceived += OnMessageReceived;
#endif
    }
    
    static public IEnumerator InitRemoteConfig()
    {
        Firebase.FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == Firebase.DependencyStatus.Available)
            {
                app = Firebase.FirebaseApp.DefaultInstance;
                Debug.Log("[INIT] Success Firebase Initialize ");

                _FetchRemoteConfig();
            }
            else
            {
                Debug.LogError(System.String.Format("[ERROR] Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                _isEndLoadRemoteConfig = true;
            }
        });

        UnityEngine.WaitUntil wait = new UnityEngine.WaitUntil(()=> _isEndLoadRemoteConfig == true);
        yield return wait;
    }

    static public bool IsFirebaseInit()
    {
        return app != null;
    }

    static public void OnTokenReceived(object sender, Firebase.Messaging.TokenReceivedEventArgs token)
    {
        Debug.Log("Received Registration Token: " + token.Token);
    }

    static public void OnMessageReceived(object sender, Firebase.Messaging.MessageReceivedEventArgs e)
    {
        Debug.Log("Received a new message from: " + e.Message.From);

        var notification = e.Message.Notification;
        if (notification != null)
        {
            Debug.Log("title: " + notification.Title);
            Debug.Log("body: " + notification.Body);

        }
        if (e.Message.From.Length > 0)
            Debug.Log("from: " + e.Message.From);
        if (e.Message.Data.Count > 0)
        {
            Debug.Log("data:");
            foreach (System.Collections.Generic.KeyValuePair<string, string> iter in e.Message.Data)
            {
                Debug.Log("  " + iter.Key + ": " + iter.Value);
            }
        }
    }


    // static public void LogEventWithStageInfo(string name)
    // {
    //     LocalStageInfo LocalStageInfo = LocalInfoConnecter.Instance.LocalStageInfo;

    //     Firebase.Analytics.Parameter[] parameters =
    //     {
    //         new Firebase.Analytics.Parameter("st_now", LocalStageInfo.curStageIdx.ToString())
    //         , new Firebase.Analytics.Parameter("st_max", LocalStageInfo.bestStageIdx.ToString())
    //     };

    //     LogEvent(name, parameters);
    // }
    // static public void LogEventWithStageInfo(string name, Parameter[] parameterList)
    // {
    //     List<Parameter> totalParameter = new List<Parameter>(2 + parameterList.Length);

    //     LocalStageInfo LocalStageInfo = LocalInfoConnecter.Instance.LocalStageInfo;
    //     totalParameter.Add(new Firebase.Analytics.Parameter("st_now", LocalStageInfo.curStageIdx.ToString()));
    //     totalParameter.Add(new Firebase.Analytics.Parameter("st_max", LocalStageInfo.bestStageIdx.ToString()));

    //     totalParameter.AddRange(parameterList);

    //     LogEvent(name, totalParameter.ToArray());
    // }

    // static public void SendLogSpendGem(LogRefType logRefType, System.Numerics.BigInteger quantity)
    // {
    //     if(quantity > System.Numerics.BigInteger.Zero)
    //     {
    //         return;
    //     }
    //     if(logRefType == LogRefType.NONE)
    //     {
    //         return;
    //     }

    //     Firebase.Analytics.Parameter[] parameters =
    //     {
    //         new Firebase.Analytics.Parameter("quantity", System.Numerics.BigInteger.Abs(quantity).ToString())
    //         , new Firebase.Analytics.Parameter("ref", (BitConvert.Enum32ToInt(logRefType)).ToString())
    //     };
    //     LogEventWithStageInfo("m_gem_spend", parameters);
    // }
    // static public void SendLogOpenEquip(LogRefType logRefType, int idx)
    // {
    //     if(logRefType == LogRefType.NONE)
    //     {
    //         return;
    //     }
        
    //     Firebase.Analytics.Parameter[] parameters =
    //     {
    //         new Firebase.Analytics.Parameter("idx", idx.ToString())
    //         , new Firebase.Analytics.Parameter("ref", (BitConvert.Enum32ToInt(logRefType)).ToString())
    //     };
    //     LogEventWithStageInfo("m_equip_open", parameters);
    // }
    // static public void SendLogAdReward(LogAdRewardType type, int tier = DefsDefault.VALUE_ZERO)
    // {
    //     LocalShopInfo LocalShopInfo = LocalInfoConnecter.Instance.LocalShopInfo;
    //     bool isAdSkip = LocalShopInfo.IsADSkip();

    //     Firebase.Analytics.Parameter[] parameters =
    //     {
    //         new Firebase.Analytics.Parameter("tier", tier.ToString())
    //         , new Firebase.Analytics.Parameter("ref", (BitConvert.Enum32ToInt(type)).ToString())
    //         , new Firebase.Analytics.Parameter("skip", isAdSkip ? "Y" : "N")
    //     };
    //     LogEventWithStageInfo("m_ad_reward", parameters);
    // }
//     static public void SendLogPurchase(string currency, string id, string price, ProductCategory type, int idx, string artier = "000")
//     {
// #if UNITY_EDITOR || MAF_DEV
//         Debug.LogFormat("[Appsflyer_InAppEvent] currency: {0}, id: {1}, price: {2}", currency, id, price);
// #else
//         Appsflyer_InAppEvent(currency, id, price);
// #endif

//         Firebase.Analytics.Parameter[] parameters =
//         {
//             new Firebase.Analytics.Parameter("type", (BitConvert.Enum32ToInt(type)).ToString())
//             , new Firebase.Analytics.Parameter("idx", idx.ToString())
//             , new Firebase.Analytics.Parameter("artier", artier)
//         };
//         LogEventWithStageInfo("m_shop_purchase", parameters);
//     }

    static public void LogEvent(string name)
    {
#if UNITY_EDITOR || MAF_DEV
        Debug.Log("LogEvent: " + name);
#else
        FirebaseAnalytics.LogEvent(name);
        Appsflyer_TrackEvent(name);
#endif
    }

//     static public void LogEvent<T>(string name, string parameterName, T parameterValue)
//     {
// #if !UNITY_EDITOR
//         FirebaseAnalytics.LogEvent(name, parameterName, parameterValue);
//         Appsflyer_TrackEvent(name);
// #else
//         Debug.Log(string.Format("LogEvent: {0} {1} {2}", name, parameterName, parameterValue));
// #endif
//     }
    
    static public void LogEvent(string name, Parameter[] parameterList)
    {
#if UNITY_EDITOR || MAF_DEV
        Debug.Log("[LogEvent] " + name);
#else
        FirebaseAnalytics.LogEvent(name, parameterList);
        Appsflyer_TrackEvent(name);
#endif  
    }

    #region appsflyer
    private static void Appsflyer_InAppEvent(string currency, string id, string price)
    {
        Dictionary<string, string> purchaseEvent = new Dictionary<string, string>();
        purchaseEvent.Add(AFInAppEvents.CURRENCY, currency);
        purchaseEvent.Add(AFInAppEvents.CONTENT_ID, id);
        purchaseEvent.Add(AFInAppEvents.REVENUE, price);
        purchaseEvent.Add(AFInAppEvents.CONTENT_TYPE, "InApp");
        AppsFlyerSDK.AppsFlyer.sendEvent(AFInAppEvents.PURCHASE, purchaseEvent);
    }
    private static void Appsflyer_TrackEvent(string eventKey)
    {
        Dictionary<string, string> eventValue = new Dictionary<string, string>();
        AppsFlyerSDK.AppsFlyer.sendEvent(eventKey, eventValue);
    }
    #endregion

#region RemoteConfig
        static bool _isEndLoadRemoteConfig          = false;
        static private List<string> _remoteKeys     = new List<string>();


        static private void _FetchRemoteConfig()
        {
            ConfigSettings configSettings = FirebaseRemoteConfig.Settings;
    #if UNITY_EDITOR
            configSettings.IsDeveloperMode = true;
    #else
            configSettings.IsDeveloperMode = false;
    #endif
            FirebaseRemoteConfig.Settings = configSettings;

            System.Threading.Tasks.Task fetchTask = FirebaseRemoteConfig.FetchAsync(new System.TimeSpan(0));

            fetchTask.ContinueWith(task =>
            {
                if(task.IsCanceled || task.IsFaulted)
                {
                    _isEndLoadRemoteConfig = true;
                    return;
                }
                FirebaseRemoteConfig.ActivateFetched();
                IEnumerable<string> keys = FirebaseRemoteConfig.GetKeysByPrefix("");
                _remoteKeys.Clear();
                _remoteKeys = keys.ToList<string>();
                _isEndLoadRemoteConfig = true;
            });
        }

    static public bool IsRemoteConfigValue(string key)
    {
        return _remoteKeys.Contains(key);
    }

    static public float GetRemoteConfigValue(string key)
    {
        float value = FirebaseRemoteConfig.GetValue(key).LongValue;
        return value;
    }
#endregion
}
