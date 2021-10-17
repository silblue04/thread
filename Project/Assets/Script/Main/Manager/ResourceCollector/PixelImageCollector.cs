using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelImageCollector : MonoBehaviour
{
    [SerializeField] private Sprite _pixelHardwallSprite;
    [SerializeField] private Sprite _pixelBaseSprite;
    [SerializeField] private Sprite _pixelOpenSprite;

    public ref Sprite GetPixelHardwallSprite()
    {
        return ref _pixelHardwallSprite;
    }

    public ref Sprite GetPixelBaseSprite()
    {
        return ref _pixelBaseSprite;
    }

    public ref Sprite GetPixelOpenSprite()
    {
        return ref _pixelOpenSprite;
    }
}
