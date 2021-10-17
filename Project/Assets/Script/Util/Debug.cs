#define USE_LOG
 
using UnityEngine;
 
public sealed class Debug : UnityEngine.Debug
{
    #if USE_LOG
    public const string ENABLE_LOGS = "ENABLE_LOG";
    #endif

    [System.Diagnostics.Conditional(ENABLE_LOGS)]
    public static new void Log(object message)
    {
        UnityEngine.Debug.Log(message);
    }
 
    [System.Diagnostics.Conditional(ENABLE_LOGS)]
    public static new void Log(object message, Object context)
    {
        UnityEngine.Debug.Log(message, context);
    }

    [System.Diagnostics.Conditional(ENABLE_LOGS)]
    public static new void LogFormat(string message, params object[] args)
    {
        UnityEngine.Debug.LogFormat(message, args);
    }
 
    [System.Diagnostics.Conditional(ENABLE_LOGS)]
    public static new void LogFormat(Object context, string message, params object[] args)
    {
        UnityEngine.Debug.LogFormat(context, message, args);
    }
 
    [System.Diagnostics.Conditional(ENABLE_LOGS)]
    public static new void LogWarning(object message)
    {
        UnityEngine.Debug.LogWarning(message);
    }
 
    [System.Diagnostics.Conditional(ENABLE_LOGS)]
    public static new void LogWarning(object message, Object context)
    {
        UnityEngine.Debug.LogWarning(message, context);
    }
 
    [System.Diagnostics.Conditional(ENABLE_LOGS)]
    public static new void LogWarningFormat(string message, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(message, args);
    }

    [System.Diagnostics.Conditional(ENABLE_LOGS)]
    public static new void LogWarningFormat(Object context, string message, params object[] args)
    {
        UnityEngine.Debug.LogWarningFormat(context, message, args);
    }
 
    [System.Diagnostics.Conditional(ENABLE_LOGS)]
    public static new void LogError(object message)
    {
        UnityEngine.Debug.LogError(message);
    }
 
    [System.Diagnostics.Conditional(ENABLE_LOGS)]
    public static new void LogError(object message, Object context)
    {
        UnityEngine.Debug.LogError(message, context);
    }

    [System.Diagnostics.Conditional(ENABLE_LOGS)]
    public static new void LogErrorFormat(string message, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(message, args);
    }

    [System.Diagnostics.Conditional(ENABLE_LOGS)]
    public static new void LogErrorFormat(Object context, string message, params object[] args)
    {
        UnityEngine.Debug.LogErrorFormat(context, message, args);
    }
    
    [System.Diagnostics.Conditional(ENABLE_LOGS)]
    public static new void LogException(System.Exception exception)
    {
        UnityEngine.Debug.LogException(exception);
    }

    [System.Diagnostics.Conditional(ENABLE_LOGS)]
    public static new void LogException(System.Exception exception, Object context)
    {
        UnityEngine.Debug.LogException(exception, context);
    }
}