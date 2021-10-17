using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

public class ToggleButton : Toggle
{
    [SerializeField] private GameObjectSelector _stateSelector;


    public void Init(bool isOn = true)
    {
        this.isOn = isOn;
        _OnValueChanged(this.isOn);

        this.onValueChanged.AddListener(_OnValueChanged);
    }

    private void _OnValueChanged(bool isOn)
    {
        _stateSelector.Select(isOn);
    }
}