using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;


public class LeftTimeTextUpdater : MonoBehaviour
{
    [SerializeField] private TMP_Text _leftTimeText;
    
    public System.Action<int> OnUpdate = null;
    private void Awake()
    {
        if(_leftTimeText == null)
        {
            _leftTimeText = this.GetComponent<TMP_Text>();
        }
        if(_leftTimeText == null)
        {
            Debug.LogErrorFormat("LeftTimeText doesn't exit name: {0}", this.name);
        }
    }
    
    public void StopUpdate()
    {
        _StopCoOfUpdateTimer();
    }

    public void SetLeftTimeText(float sce)
    {
        SetLeftTimeText(sce.ToString());
    }

    public void SetLeftTimeText(string text)
    {
        _leftTimeText.text = text;
    }

    public void SetLeftTime(float sec, float delay = DefsDefault.DEFAULT_FLOAT)
    {
        int secInt = Mathf.FloorToInt(sec);
        SetLeftTime(secInt, delay);
    }

    public void SetLeftTime(int sec, float delay = DefsDefault.DEFAULT_FLOAT)
    {
        _StartCoOfUpdateTimer(sec, delay);
    }

    private IEnumerator _coUpdateTimer = null;
    private void _StartCoOfUpdateTimer(int targetSec, float delay = DefsDefault.DEFAULT_FLOAT)
    {
        _StopCoOfUpdateTimer();

        _coUpdateTimer = _CoUpdateTimer(targetSec, delay);
        StartCoroutine(_coUpdateTimer);
    }
    private void _StopCoOfUpdateTimer()
    {
        if(_coUpdateTimer == null)
        {
            return;
        }

        StopCoroutine(_coUpdateTimer);
        _coUpdateTimer = null;
    }

    private IEnumerator _CoUpdateTimer(int targetSec, float delay = DefsDefault.DEFAULT_FLOAT)
    {
        int leftTime = targetSec;
        _leftTimeText.text = _GetLeftTimeText(leftTime);

        if(delay > 0.0f)
        {
            yield return YieldInstructionCache.WaitForSeconds(delay);
        }

        var waitForSeconds = YieldInstructionCache.WaitForSeconds(1.0f);
        while(true)
        {
            yield return waitForSeconds;

            --leftTime;
            _leftTimeText.text = _GetLeftTimeText(leftTime);
            OnUpdate?.Invoke(leftTime);

            if (leftTime <= 0)
            {
                yield break;
            }
        }
    }

    private string _GetLeftTimeText(int leftTime)
	{
        string leftTimeText = UtilTime.GetTimeString(leftTime);
		return leftTimeText;
	}
}
