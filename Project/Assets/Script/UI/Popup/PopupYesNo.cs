using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class PopupYesNoOpenData : Param
{
    public string message;
    public Action callbackYes;
    public Action callbackNo;

    public PopupYesNoOpenData(string message, Action callbackYes = null, Action callbackNo = null)
    {
        this.message = message;
        this.callbackYes = callbackYes;
        this.callbackNo = callbackNo;
    } 
}

public class PopupYesNo : UIBase
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _messageText;

    private PopupYesNoOpenData _openData = null;


    public static void Show(string message, Action callbackYes = null, Action callbackNo = null)
    {
        PopupYesNoOpenData openData = new PopupYesNoOpenData(message, callbackYes, callbackNo);
        Show(openData);
    }
    public static void Show(PopupYesNoOpenData openData)
    {
        UIManager.Instance.Show<PopupYesNo>(DefsUI.Prefab.POPUP_YES_NO, openData);
    }

    public override void OnOpenStart(Param param = null)
    {
        base.OnOpenStart(param);

        _openData = param as PopupYesNoOpenData;
        if(_openData == null)
        {
            Close();
            return;
        }

        _Init();
    }

    private void _Init()
    {
        _messageText.text = _openData.message;
    }

    public void OnClickYesButton()
    {
        _openData.callbackYes?.Invoke();
        base.Close();
    }

    public void OnClickNoButton()
    {
        _openData.callbackNo?.Invoke();
        base.Close();
    }

    public override void Close()
    {
        OnClickNoButton();
    }
}
