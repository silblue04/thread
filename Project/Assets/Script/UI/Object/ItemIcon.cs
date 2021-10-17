using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;
using TMPro;


public class ItemIcon : MonoBehaviour
{
    [Header("ItemIcon")]
    [SerializeField] protected Image _gradeIcon;
    [SerializeField] protected Image _itemIcon;

    [Header("ItemIcon_Option")]
    [SerializeField] protected TextObject _itemNum = null;

    public int item_idx { get; protected set; } = DefsDefault.VALUE_NONE;

    private bool _OnSettingitemNum { get { return (_itemNum != null); }}
    

    public void Init(int item_idx, System.Numerics.BigInteger item_value)
    {
        this.item_idx = item_idx;

        _InitIcon();
        _InitGrade();
        _InitItemNumText(item_value);
    }

    protected void _InitIcon()
    {
        _itemIcon.sprite = DefsItem.GetItemIcon(item_idx);
    }

    protected void _InitGrade()
    {
        _gradeIcon.sprite = DefsItem.GetGradeIcon(item_idx);
    }

    protected virtual void _InitItemNumText(System.Numerics.BigInteger item_value)
    {
        if(_OnSettingitemNum == false)
        {
            return;
        }
        
        bool isNeededToShowItemNum = (item_value > DefsDefault.VALUE_ONE);
        _itemNum.gameObject.SetActive(isNeededToShowItemNum);
        if(isNeededToShowItemNum)
        {
            _itemNum.SetText(DefsItem.GetItemValueText(item_idx, item_value));
        }
    }
}
