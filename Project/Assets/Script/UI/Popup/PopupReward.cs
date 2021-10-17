using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;


public class PopupRewardOpenData : Param
{
    public int reward_item_idx;
    public System.Numerics.BigInteger reward_item_value;
    public Action callbackConfirm;

    public PopupRewardOpenData(int reward_item_idx, System.Numerics.BigInteger reward_item_value, Action callbackConfirm = null)
    {
        this.reward_item_idx    = reward_item_idx;
        this.reward_item_value  = reward_item_value;

        this.callbackConfirm    = callbackConfirm;
    }
}

public class PopupReward : UIBase
{
    [Header("Text")]
    [SerializeField] private Image _itemIcon;
    [SerializeField] private TextMeshProUGUI _itemValueText;
    [SerializeField] private TextMeshProUGUI _descText;
    [SerializeField] private Transform _spreadItemPivot;

    private PopupRewardOpenData _openData = null;


    public static void Show(int reward_item_idx, System.Numerics.BigInteger reward_item_value, Action callbackConfirm = null)
    {
        PopupRewardOpenData openData = new PopupRewardOpenData(reward_item_idx, reward_item_value, callbackConfirm);
        Show(openData);
    }
    public static void Show(PopupRewardOpenData openData)
    {
        UIManager.Instance.Show<PopupReward>(DefsUI.Prefab.POPUP_REWARD, openData);
    }

    public override void OnOpenStart(Param param = null)
    {
        base.OnOpenStart(param);

        _openData = param as PopupRewardOpenData;
        if(_openData == null)
        {
            Close();
            return;
        }

        _Init();
    }

    private void _Init()
    {
        _itemIcon.sprite    = DefsItem.GetItemIcon(_openData.reward_item_idx);
        _itemValueText.text = DefsItem.GetItemValueText(_openData.reward_item_idx, _openData.reward_item_value);
        _descText.text      = Localization.Get(DefsUI.LocalizationKey.POPUP_REWARD_DESC);
    }

    public override void Close()
    {
        ItemType itemType = DefsItem.GetItemType(_openData.reward_item_idx);
       
        if(itemType == ItemType.Currency)
        {
            // UIManager.Instance.PlayGetItemAnimation
            // (
            //     _spreadItemPivot.position
            //     , _openData.reward_item_idx
            //     , _openData.reward_item_value
            // );
        }
        

        _openData.callbackConfirm?.Invoke();
        base.Close();
    }
}
