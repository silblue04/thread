using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;
using TMPro;

public class OptionButton : MonoBehaviour
{
    [SerializeField] private OptionType _optionType;
    [SerializeField] private GameObjectSelector _stateSelector;

    private OptionState _state = OptionState.On;
    private Action<OptionType, OptionState> _callbackOnClickOnOffButton = null;


    public void Init(Action<OptionType, OptionState> callbackOnClickButton = null)
    {
        LocalOptionInfo LocalOptionInfo = LocalInfoConnecter.Instance.LocalOptionInfo;
        _state = LocalOptionInfo.GetOptionState(_optionType);

        int stateIndex = BitConvert.Enum32ToInt(_state);
        _stateSelector.Select(stateIndex);

        _callbackOnClickOnOffButton = callbackOnClickButton;
    }

    public void OnClickButton()
    {
        _state = (_state == OptionState.On) ? OptionState.Off : OptionState.On;
        int stateIndex = BitConvert.Enum32ToInt(_state);

        _stateSelector.Select(stateIndex);
        _callbackOnClickOnOffButton?.Invoke(_optionType, _state);
    }
}
