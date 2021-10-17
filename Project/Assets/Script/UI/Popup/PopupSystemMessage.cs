using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;




public class PopupSystemMessageOpenData : Param
{
    public string message;
    public PopupSystemMessage.Type type;

    public PopupSystemMessageOpenData(string message, PopupSystemMessage.Type type)
    {
        this.message = message;
        this.type = type;
    } 
}

public class PopupSystemMessage : UIBase
{
    public enum Type
    {
        Good,
        Normal,
        Bad,

        MAX
    }

    [Header("Text & Image")]
    [SerializeField] private Image _messageBg;
    [SerializeField] private TextMeshProUGUI _messageText;

    [Header("0 : Good 1 : Normal 2 : Bad")]
    [SerializeField] private Sprite[] _messageBgImage = new Sprite[BitConvert.Enum32ToInt(Type.MAX)];
    [SerializeField] private Color[] _messageTextColor = new Color[BitConvert.Enum32ToInt(Type.MAX)];

    private PopupSystemMessageOpenData _openData = null;
    private Sequence _closeSequence = null;

    public static void Show(string message, Type type = Type.Good)
    {
        PopupSystemMessageOpenData openData = new PopupSystemMessageOpenData(message, type);
        UIManager.Instance.Show<PopupSystemMessage>(DefsUI.Prefab.POPUP_SYSTEM_MESSAGE, openData);
        //Show(openData);
    }

    public static void Show(PopupSystemMessageOpenData openData)
    {
        UIManager.Instance.AddSystemMessageUI(openData);
    }

    public override void OnOpenStart(Param param = null)
    {
        base.OnOpenStart(param);

        _openData = param as PopupSystemMessageOpenData;
        if(_openData == null)
        {
            Close();
            return;
        }

        _Init();
    }

    private void _Init()
    {
        float closeTime = 2.2f;

        int typeIndex = BitConvert.Enum32ToInt(_openData.type);
        _messageBg.sprite = _messageBgImage[typeIndex];
        _messageText.text = _openData.message;
        _messageText.color = _messageTextColor[typeIndex];
        

        _closeSequence = DOTween.Sequence();
        _closeSequence.SetAutoKill(false);
        _closeSequence.AppendInterval(closeTime);
        _closeSequence.AppendCallback(() =>
        {
            Close();
        });
    }

    public override void Close()
    {
        if(_closeSequence != null)
        {
            _closeSequence.Kill();
        }

        base.Close();
    }
}
