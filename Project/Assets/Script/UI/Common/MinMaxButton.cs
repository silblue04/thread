using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Numerics;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Purchasing;



public class MinMaxButton : MonoBehaviour
{
    [Header("Button")]
    [SerializeField] private InteractableButton _minButton;
    [SerializeField] private InteractableButton _maxButton;

    [SerializeField] private InteractableButton _minusButton;
    [SerializeField] private InteractableButton _plusButton;

    [Header("Text")]
    [SerializeField] private TextMeshProUGUI _numText;


    private int _minNum = DefsDefault.VALUE_ZERO;
    private int _maxNum = DefsDefault.VALUE_ZERO;

    public int curNum { get; private set; } = DefsDefault.VALUE_NONE;
    
    private Action<int> _callbackChangeNum = null;


    public void Init
    (
        Action<int> callbackChangeNum = null
    )
    {
        _callbackChangeNum = callbackChangeNum;
    }

    public void SetMinMax(int minNum, int maxNum)
    {
        _minNum = minNum;
        _maxNum = maxNum;
    }
    public void SetCurNum(int curNum)
    {
        _SetNumAndUpdateButtonInteractable(curNum);
    }


    public void OnClickMinButton()
    {
        if(_minNum == curNum)
        {
            return;
        }

        _SetNumAndUpdateButtonInteractable(_minNum);
    }
    public void OnClickMaxButton()
    {
        if(_maxNum == curNum)
        {
            return;
        }

        _SetNumAndUpdateButtonInteractable(_maxNum);
    }

    public void OnClickMinusButton()
    {
        if(_minNum == curNum)
        {
            return;
        }
        if(_minNum > curNum)
        {
            _SetNumAndUpdateButtonInteractable(_minNum);
            return;
        }

        _SetNumAndUpdateButtonInteractable(curNum - DefsDefault.VALUE_ONE);
    }
    public void OnClickPlusButton()
    {
        if(_maxNum == curNum)
        {
            return;
        }
        if(_maxNum < curNum)
        {
            _SetNumAndUpdateButtonInteractable(_maxNum);
            return;
        }

        _SetNumAndUpdateButtonInteractable(curNum + DefsDefault.VALUE_ONE);
    }

    private void _SetNumAndUpdateButtonInteractable(int num)
    {
        bool isChangedNum = (curNum != num);

        if(isChangedNum)
        {
            curNum = num;
            _numText.text = curNum.ToString();

            _callbackChangeNum?.Invoke(curNum);
        }


        _minButton.SetInteractable(_minNum < curNum);
        _maxButton.SetInteractable(_maxNum > curNum);

        _minusButton.SetInteractable(_minNum < curNum);
        _plusButton.SetInteractable(_maxNum > curNum);
    }
}
