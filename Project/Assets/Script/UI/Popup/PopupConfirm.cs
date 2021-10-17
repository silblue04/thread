using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class PopupConfirmOpenData : Param
{
    public string message;
    public Action callbackConfirm;

    public PopupConfirmOpenData(string message, Action callbackConfirm = null)
    {
        this.message = message;
        this.callbackConfirm = callbackConfirm;
    } 
}

public class PopupConfirm : UIBase
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _messageText;

    private PopupConfirmOpenData _openData = null;


    public static void Show(string message, Action callbackConfirm = null)
    {
        PopupConfirmOpenData openData = new PopupConfirmOpenData(message, callbackConfirm);
        Show(openData);
    }
    public static void Show(PopupConfirmOpenData openData)
    {
        UIManager.Instance.Show<PopupConfirm>(DefsUI.Prefab.POPUP_CONFIRM, openData);
    }

    public override void OnOpenStart(Param param = null)
    {
        base.OnOpenStart(param);

        _openData = param as PopupConfirmOpenData;
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

    public override void Close()
    {
        _openData.callbackConfirm?.Invoke();
        base.Close();
    }
}
