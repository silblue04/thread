using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.Numerics;
using UnityEngine.UI;
using TMPro;


public class AdsButtonEx : AdsButton
{
    [Header("AdsButtonEx")]
    [SerializeField] protected InteractableButton _watchAdsButton;
    [SerializeField] protected InteractableButton _skipAdsButton;

    [SerializeField] protected LeftTimeTextUpdater _leftTimeText;
 

    public void SetInteractable(bool interactable)
    {
        _watchAdsButton.SetInteractable(interactable);
        _skipAdsButton.SetInteractable(interactable);

        _leftTimeText.gameObject.SetActive(!interactable);
    }

    public void SetLeftTime(int leftTime)
    {
        _leftTimeText.SetLeftTime(leftTime);
    }
}
