using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;
using TMPro;


public class TextObject : MonoBehaviour
{
    [Header("TextObject")]
    [SerializeField] private TextMeshProUGUI _text;


    public void SetFont(int langegeIndex)
    {
        //Material material = _text.fontMaterial;
        _text.font = ResourceManager.Instance.fontAssetList[langegeIndex];
        //_text.fontMaterial = material;
    }

    public void SetText(string text)
    {
        _text.text = text;
    }
}
