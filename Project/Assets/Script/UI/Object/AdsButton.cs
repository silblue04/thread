using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Numerics;
using UnityEngine.UI;
using TMPro;


public class AdsButton : MonoBehaviour
{
    public enum State
    {
        WatchAds,
        SkipAds,
    }

    [Header("AdsButton")]
    [SerializeField] protected GameObjectSelector _stateSelector;
    [SerializeField] private TextMeshProUGUI _multiValueText;

    protected Action _callbackOnClickButton = null;
    protected Action _callbackCheck = null;
    
    public void Init(Action callbackOnClickButton = null)
    {
        _stateSelector.Select(State.WatchAds);
        _callbackOnClickButton = callbackOnClickButton;

        _multiValueText.gameObject.SetActive(false);
    }

    public void CheckClick(Action checkEvent = null, Action callbackOnClickButton = null)
    {
        _stateSelector.Select(State.WatchAds);
        _callbackCheck = checkEvent;
        _callbackOnClickButton = callbackOnClickButton;

        _multiValueText.gameObject.SetActive(false);
    }

    public void SetMultiValueText(int multi_value_ad, bool onOffGameObject = false)
    {
        if(onOffGameObject)
        {
            bool isPossibleToWatchAds = DefsAds.IsValiedMulitValue(multi_value_ad);
            this.gameObject.SetActive(isPossibleToWatchAds);
            if (isPossibleToWatchAds == false)
            {
                return;
            }
        }
        
        _multiValueText.gameObject.SetActive(true);
        _multiValueText.text = string.Format(DefsString.TextFormat.MULTI_VALUE_TEXT_FORMAT, multi_value_ad);
    }

    public void OnClickButton()
    {
        // TODO : 광고 스킵권
        if(_callbackCheck != null)
            _PlayADReward();
        else
            OnClickWatchAdsButton();
    }

    public void OnClickWatchAdsButton()
    {
        if(_callbackCheck == null)
        {
            _PlayADReward();
        }
        else
        {
            _callbackCheck.Invoke();
        }        
    }

    private void _PlayADReward()
    {
        // TODO : ads
        // AdsHelper.Instance.PlayRewardAd(() =>
        // {
            _callbackOnClickButton?.Invoke();
        // });
    }

    public void OnClickSkipAdsButton()
    {
        _callbackOnClickButton?.Invoke();
    }
}
