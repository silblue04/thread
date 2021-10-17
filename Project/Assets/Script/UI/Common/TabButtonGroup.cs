using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class TabButtonGroup : MonoBehaviour
{
    protected List<TabButton> _tabButtonList = new List<TabButton>();
    
    protected int _prevSelectedButtonIndex = DefsDefault.VALUE_NONE;
    protected int _curSelectedButtonIndex = DefsDefault.VALUE_NONE;

    private Action<int> _callbackOnClick = null;
    private Action _callbackOnDeselectWhenOnClickSelectedButton = null;

    public List<TabButton> TabButtonList { get { return _tabButtonList; } set { } }
    public int PrevSelectedButtonIndex { get { return _prevSelectedButtonIndex; } }
    public int CurSelectedButtonIndex { get { return _curSelectedButtonIndex; } }
    public TabButton PrevSelectedButton { get { return (_curSelectedButtonIndex == DefsDefault.VALUE_NONE) ? null : _tabButtonList[_curSelectedButtonIndex]; } }
    public TabButton CurSelectedButton { get { return _tabButtonList[_curSelectedButtonIndex]; } }

    private bool _isNeededUpdateInteractable = true;
    private bool _isNeededToDeselectIfOnClickSelectedButton = true;


    public int ChildCount
    {
        get
        {
            return _tabButtonList.Count;
        }
    }

    public void Init
    (
        Action<int> callbackOnClick = null, bool isNeededUpdateInteractable = true
        , bool isNeededToDeselectIfOnClickSelectedButton = false, Action callbackOnDeselectWhenOnClickSelectedButton = null
    )
    {
        this._callbackOnClick = callbackOnClick;
        this._isNeededUpdateInteractable = isNeededUpdateInteractable;

        this._callbackOnDeselectWhenOnClickSelectedButton = callbackOnDeselectWhenOnClickSelectedButton;
        this._isNeededToDeselectIfOnClickSelectedButton = isNeededToDeselectIfOnClickSelectedButton;

        _SetTabButtonList();
    }

    protected void _SetTabButtonList()
    {
        _tabButtonList.Clear();

        int noneActiveStateObjCnt = 0;
        for (int i = 0; i < this.transform.childCount; ++i)
        {
            TabButton tabButton = this.transform.GetChild(i).GetComponent<TabButton>();
            if (tabButton == null || tabButton.gameObject.activeSelf == false)
            {
                ++noneActiveStateObjCnt;
                continue;
            }

            int buttonIndexInActiveList = i - noneActiveStateObjCnt;
            tabButton.onClick.RemoveAllListeners();
            tabButton.onClick.AddListener(() => OnClickButton(buttonIndexInActiveList));

            tabButton.SetState(tabButton.GetInitialState());
            switch (tabButton.curState)
            {
                case TabButton.State.Basic:
                    if(_isNeededUpdateInteractable)
                    {
                        tabButton.SetInteractable(true);
                    }
                    break;

                case TabButton.State.Selected:
                    if(_isNeededToDeselectIfOnClickSelectedButton)
                    {
                        tabButton.SetInteractable(true);
                    }
                    else if (_isNeededUpdateInteractable)
                    {
                        tabButton.SetInteractable(false);
                    }
                    _curSelectedButtonIndex = buttonIndexInActiveList;
                    break;

                case TabButton.State.Locked:
                    tabButton.SetInteractable(false);
                    //tabButton.enabled = false;
                    break;
            }

            _tabButtonList.Add(tabButton);
        }
    }

    public virtual void OnClickButton(int index, bool onCallback = true)
    {
        if (_tabButtonList.Count == 0)
        {
            return;
        }

        _prevSelectedButtonIndex = _curSelectedButtonIndex;
        _curSelectedButtonIndex = index;

        if(_isNeededToDeselectIfOnClickSelectedButton)
        {
            if(_prevSelectedButtonIndex == _curSelectedButtonIndex)
            {
                if(_curSelectedButtonIndex < DefsDefault.VALUE_ZERO || _curSelectedButtonIndex >= _tabButtonList.Count)
                {
                    if (onCallback)
                    {
                        _callbackOnDeselectWhenOnClickSelectedButton?.Invoke();
                    }
                    return;
                }
                else
                {
                    if(_tabButtonList[_curSelectedButtonIndex].curState == TabButton.State.Selected)
                    {
                        _tabButtonList[_curSelectedButtonIndex].SetState(TabButton.State.Basic);
                        _prevSelectedButtonIndex = DefsDefault.VALUE_NONE;

                        if (onCallback)
                        {
                            _callbackOnDeselectWhenOnClickSelectedButton?.Invoke();
                        }
                        return;
                    }
                }
            }
        }


        if (_prevSelectedButtonIndex != DefsDefault.VALUE_NONE)
        {
            if (_isNeededUpdateInteractable)
            {
                _tabButtonList[_prevSelectedButtonIndex].SetInteractable(true);
            }
            _tabButtonList[_prevSelectedButtonIndex].SetState(TabButton.State.Basic);
        }

        if(_isNeededToDeselectIfOnClickSelectedButton)
        {
            _tabButtonList[_curSelectedButtonIndex].SetInteractable(true);
        }
        else if (_isNeededUpdateInteractable)
        {
            _tabButtonList[_curSelectedButtonIndex].SetInteractable(false);
        }
        _tabButtonList[_curSelectedButtonIndex].SetState(TabButton.State.Selected);


        if (onCallback)
        {
            _callbackOnClick?.Invoke(index);
        }
    }

    public virtual void OnClickSelectedButton(bool onCallback = true)
    {
        OnClickButton(CurSelectedButtonIndex);
    }


    public void SetAllButtonToBeInteractable(bool isInteractable)
    {
        foreach(var button in _tabButtonList)
        {
            button.SetInteractable(isInteractable);
        }
    }

    public void UpdateAllButtonState()
    {
        if (_tabButtonList.Count == 0)
        {
            return;
        }

        for (int i = 0; i < _tabButtonList.Count; i++)
        {
            switch (_tabButtonList[i].curState)
            {
                case TabButton.State.Basic:
                    if (_isNeededUpdateInteractable)
                    {
                        _tabButtonList[i].SetInteractable(true);
                    }
                    break;

                case TabButton.State.Selected:
                    if(_isNeededToDeselectIfOnClickSelectedButton)
                    {
                        _tabButtonList[i].SetInteractable(true);
                    }
                    else if (_isNeededUpdateInteractable)
                    {
                        _tabButtonList[i].SetInteractable(false);
                    }
                    _curSelectedButtonIndex = i;
                    break;

                case TabButton.State.Locked:
                    _tabButtonList[i].SetInteractable(false);
                    //_tabButtonList[i].enabled = false;
                    break;
            }
        }
    }
}
