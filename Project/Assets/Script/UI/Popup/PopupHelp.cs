using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class PopupHelpOpenData : Param
{
    public string context;

    public PopupHelpOpenData(string context)
    {
        this.context = context;
    } 
}

public class PopupHelp : UIBase
{
    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _messageText;

    private PopupHelpOpenData _openData = null;


    public static void Show(string message)
    {
        PopupHelpOpenData openData = new PopupHelpOpenData(message);
        Show(openData);
    }
    public static void Show(PopupHelpOpenData openData)
    {
        UIManager.Instance.Show<PopupHelp>(DefsUI.Prefab.POPUP_HELP, openData);
    }

    public override void OnOpenStart(Param param = null)
    {
        base.OnOpenStart(param);

        _openData = param as PopupHelpOpenData;
        if(_openData == null)
        {
            Close();
            return;
        }

        _Init();
    }

    private void _Init()
    {
        _messageText.text = _openData.context;
    }
}
