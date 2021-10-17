using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

[RequireComponent(typeof(Canvas))]
[RequireComponent(typeof(GraphicRaycaster))]
public class UIBase : MonoBehaviour
{
    [Header("-- Base Information ------")]
    [SerializeField] protected bool _isOnlyOne              = true;
    [SerializeField] protected bool _isForcedTop            = false;
    [SerializeField] protected bool _isSkippedBackButton    = false;
    [SerializeField] protected bool _isNeededSafeArea       = true;
    public bool IsOnlyOne { get { return _isOnlyOne; } }
    public bool IsForcedTop { get { return _isForcedTop; } }
    public bool IsSkippedBackButton { get { return _isSkippedBackButton; } }

    [SerializeField] protected UIType _uiType = UIType.Popup;
    public UIType UIType { get { return _uiType; } }

    protected IntrinsicUIData _uiData = null;
    public IntrinsicUIData UIData { get { return _uiData; } }

    private bool _nowOpening = false;
    private bool _nowClosing = false;
    public bool IsOpening { get { return _nowOpening; } }
    public bool IsActivated { get { return (_nowClosing == false); } }

    private int _uiSortingLayer = DefsDefault.VALUE_ONE;
    private int _maxOrder = DefsDefault.VALUE_ONE;
    public int UISortingLayer { get { return _uiSortingLayer; } }
    public int MaxOrder { get { return _maxOrder; } }

    /// <summary>
    /// UIManager에서 생성할때 사용한다. 직접 호출 할 일 없다.
    /// </summary>
    /// <param name="uiData"></param>
    public void Initialize(IntrinsicUIData uiData)
    {
        _uiData = uiData;
    }

    private Coroutine _openCoroutine = null;
    public virtual void OnOpenStart(Param param = null)
    {
        _nowOpening = true;

        _uiData.callBackOpenStart?.Invoke(this);
        _openCoroutine = StartCoroutine(_OpenEndCoroutine());
    }
    protected void OnOpenStart(Param param = null, int endCoroutineDelayFrameNum = 0)
    {
        _nowOpening = true;

        _uiData.callBackOpenStart?.Invoke(this);
        _openCoroutine = StartCoroutine(_OpenEndCoroutine(endCoroutineDelayFrameNum));
    }
    private IEnumerator _OpenEndCoroutine(int delayFrameNum = 0)
    {
        for(int i = 0; i < delayFrameNum; ++i)
        {
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        OnOpenEnd();
    }
    public virtual void OnOpenEnd()
    {
        _uiData.callBackOpenEnd?.Invoke(this);

        _nowOpening = false;
    }

    private Coroutine _updateCoroutine = null;
    public virtual void OnUpdateStart(IntrinsicUIData uiData)
    {
        _updateCoroutine = StartCoroutine(_OnUpdateEndCoroutine(uiData));
    }
    public virtual void OnUpdateStart(IntrinsicUIData uiData, int endCoroutineDelayFrameNum = 0)
    {
        _updateCoroutine = StartCoroutine(_OnUpdateEndCoroutine(uiData, endCoroutineDelayFrameNum));
    }
    private IEnumerator _OnUpdateEndCoroutine(IntrinsicUIData uiData, int delayFrameNum = 0)
    {
        for(int i = 0; i < delayFrameNum; ++i)
        {
            yield return null;
        }
        yield return new WaitForEndOfFrame();
        OnUpdateEnd(uiData);
    }
    public virtual void OnUpdateEnd(IntrinsicUIData uiData)
    {
        
    }


    public virtual void Close()
    {
        UIManager.Instance.Close(_uiData);
    }

    private Coroutine _closeCoroutine = null;
    public virtual void OnCloseStart()
    {
        _nowClosing = true;

        _uiData.callBackCloseStart?.Invoke(this);

        if (this.gameObject.activeInHierarchy == false)
        {
            OnCloseEnd();
        }
        else
        {
            _closeCoroutine = StartCoroutine(_CloseEndCoroutine());
        }   
    }
    private IEnumerator _CloseEndCoroutine()
    {
        yield return new WaitForEndOfFrame();

        OnCloseEnd();
    }
    public virtual void OnCloseEnd()
    {
        _uiData.callBackCloseEnd?.Invoke(this);

        _nowClosing = false;
        UIManager.Instance.CloseDestory(this);

        if (_uiData != null)
        {
            _uiData.Remove();
            _uiData = null;
        }

        _openCoroutine = null;
        _updateCoroutine = null;
        _closeCoroutine = null;
    }

    public virtual bool OnBackButton()
    {
        Close();

        return true;
    }
}
