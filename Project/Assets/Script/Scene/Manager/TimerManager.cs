using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public class ContentsTimerData
{
    public int leftTime                = DefsDefault.VALUE_NONE;
    public Action callbackOnEndTimer   = null;
}

public class TimerManager : Singleton<TimerManager>
{
    public enum TimerContentsType
    {
        //ResetAllContents,

        Fairy,
    }
    private Dictionary<TimerContentsType, ContentsTimerData> _perContentsTimerDic = new Dictionary<TimerContentsType, ContentsTimerData>();


    public void Init()
    {
        _StartCoUpdateTimer();
    }
    public void Release()
    {
        //_StopCoOfUpdateResetContentsTimer();
        _StopCoUpdateTimer();
    }


// #region ResetContentsTimer
//     public void StartResetContentsTimer(int second, System.Action callback = null)
//     {
//         _StartCoOfUpdateResetContentsTimer(second, callback);
//     }

//     public int GetLeftTimeToResetContents()
//     {
//         long nextContentsResetTicks = LocalInfoConnecter.Instance.LocalCommonInfoContainer.NextContentsResetTicks;
//         long nowTicks = DateTime.UtcNow.Ticks;

//         return UtilTime.GetTimeInterval(nextContentsResetTicks, nowTicks);
//     }

//     private IEnumerator _coUpdateResetContentsTimer    = null;
//     private void _StartCoOfUpdateResetContentsTimer(int second, System.Action callback = null)
//     {
//         _StopCoOfUpdateResetContentsTimer();

//         _coUpdateResetContentsTimer = _CoUpdateResetContentsTimer(second, callback);
//         StartCoroutine(_coUpdateResetContentsTimer);
//     }
//     private void _StopCoOfUpdateResetContentsTimer()
//     {
//         if(_coUpdateResetContentsTimer == null)
//         {
//             return;
//         }

//         StopCoroutine(_coUpdateResetContentsTimer);
//         _coUpdateResetContentsTimer = null;
//     }
//     private IEnumerator _CoUpdateResetContentsTimer(int second, System.Action callback = null)
//     {
//         yield return YieldInstructionCache.WaitForSeconds(second);
//         callback?.Invoke();
//     }
// #endregion


#region Contents Timer
    public bool AddPerContentsTimer(TimerContentsType type, int leftTime, Action callbackOnEndTimer = null)
    {
        if(leftTime <= DefsDefault.VALUE_ZERO)
        {
            return false;
        }


        if(_perContentsTimerDic.ContainsKey(type))
        {
            _perContentsTimerDic[type].leftTime = leftTime;
            _perContentsTimerDic[type].callbackOnEndTimer = callbackOnEndTimer;
        }
        else
        {
            ContentsTimerData data = new ContentsTimerData();
            data.leftTime = leftTime;
            data.callbackOnEndTimer = callbackOnEndTimer;

            _perContentsTimerDic.Add(type, data);
        }
        return true;
    }
    public bool RemovePerContentsTimer(TimerContentsType type)
    {
        if(_perContentsTimerDic.ContainsKey(type) == false)
        {
            return false;
        }

        _perContentsTimerDic.Remove(type);
        return true;
    }

    public int GetContentsLeftTime(TimerContentsType type)
    {
        if(_perContentsTimerDic.ContainsKey(type) == false)
        {
            return DefsDefault.VALUE_NONE;
        }
        return _perContentsTimerDic[type].leftTime;
    }

    private IEnumerator _coUpdateTimer    = null;
    private void _StartCoUpdateTimer()
    {
        _StopCoUpdateTimer();

        _coUpdateTimer = _CoUpdateTimer();
        StartCoroutine(_coUpdateTimer);
    }
    private void _StopCoUpdateTimer()
    {
        if(_coUpdateTimer == null)
        {
            return;
        }

        StopCoroutine(_coUpdateTimer);
        _coUpdateTimer = null;
    }
    private IEnumerator _CoUpdateTimer()
    {
        float UPDATE_INTERVAL = 1.0f;
        var waitForSeconds = YieldInstructionCache.WaitForSeconds(UPDATE_INTERVAL);

        //LocalAdFreeFuncInfo LocalAdFreeFunclInfo    = LocalInfoConnecter.Instance.LocalAdFreeFuncInfo;
        //LocalAdRewardFreeGemInfo LocalAdRewardFreeGemInfo    = LocalInfoConnecter.Instance.LocalAdRewardFreeGemInfo;


        while(true)
        {
            yield return waitForSeconds;

            // LocalAdFreeFunclInfo.ReduceLeftTimeAndUpdateToOff();
            // LocalAdRewardFreeGemInfo.ReduceLeftTimeAndUpdateToOff();

            _UpdateContentsLeftTime();
        }
    }

    private void _UpdateContentsLeftTime()
    {
        foreach(var dicKey in _perContentsTimerDic.Keys)
        {
            int leftTime = _perContentsTimerDic[dicKey].leftTime;
            if(leftTime > DefsDefault.VALUE_ZERO)
            {
                --_perContentsTimerDic[dicKey].leftTime;

                if(_perContentsTimerDic[dicKey].leftTime <= DefsDefault.VALUE_ZERO)
                {
                    _perContentsTimerDic[dicKey].callbackOnEndTimer?.Invoke();
                }
            }
        }
    }
#endregion
}
