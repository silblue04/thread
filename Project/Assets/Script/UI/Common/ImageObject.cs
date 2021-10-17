using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using UnityEngine.UI;
using TMPro;


public class ImageObject : MonoBehaviour
{
    [Header("ImageObject")]
    [SerializeField] private Image _image;

    public bool ImageActiveSelf { get { return _image.gameObject.activeSelf; } }


    public void SetImage(Sprite sprite, bool isNeededToFitSizeBySpriteSize = false)
    {
        _image.sprite = sprite;

        if(isNeededToFitSizeBySpriteSize)
        {
            RectTransform rectTrans =  this.transform as RectTransform;
            if(rectTrans != null)
            {
                rectTrans.sizeDelta = new Vector2(sprite.texture.width, sprite.texture.height);
            }
        }
    }

    public void SetImageActive(bool active)
    {
        _image.gameObject.SetActive(active);
    }
}
