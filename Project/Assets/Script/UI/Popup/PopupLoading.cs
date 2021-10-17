using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public class PopupLoading : MonoBehaviour
{
    public enum LoadingType
    {
        Necessary,
        Pak,

        MAX
    }
    [SerializeField] private GameObjectSelector _typeSelector;
    [SerializeField] private LoadPakUIAnimation _loadPakUIAnimation;

    private Action _callbackBeforeLoading   = null;
    private Action _callbackLoading         = null;
    private Action _callbackAfterLoading    = null;


    public void Init()
    {
        _loadPakUIAnimation.SetCallbackAnimationCompleted(LoadPakAnimationType.BeforeLoading, _CallbackBeforeLoadingAnimation);
        _loadPakUIAnimation.SetCallbackAnimationCompleted(LoadPakAnimationType.Loading, _CallbackLoadingAnimation);
        _loadPakUIAnimation.SetCallbackAnimationCompleted(LoadPakAnimationType.AfterLoading, _CallbackAfterLoadingAnimation);
        _loadPakUIAnimation.StopAnimation();
    }

    public void Show(bool on)
    {
        this.gameObject.SetActive(on);
    }

    public void SetLoadingType(LoadingType type)
    {
        _typeSelector.Select(type);
        _loadPakUIAnimation.StopAnimation();
    }

    public void PlayLoadPakAnimation(LoadPakAnimationType type, Action callback = null)
    {
        _loadPakUIAnimation.PlayAnimation(type);

        if (type == LoadPakAnimationType.BeforeLoading) { _callbackBeforeLoading = callback; }
        else if (type == LoadPakAnimationType.BeforeLoading) { _callbackLoading = callback; }
        else if (type == LoadPakAnimationType.BeforeLoading) { _callbackAfterLoading = callback; }
    }

    private void _CallbackBeforeLoadingAnimation()
    {
        _callbackBeforeLoading?.Invoke();
        _callbackBeforeLoading = null;
    }

    private void _CallbackLoadingAnimation()
    {
        _callbackLoading?.Invoke();
        _callbackLoading = null;
    }

    private void _CallbackAfterLoadingAnimation()
    {
        _callbackAfterLoading?.Invoke();
        _callbackAfterLoading = null;
    }
}
