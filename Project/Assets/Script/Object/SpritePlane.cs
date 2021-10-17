using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpritePlane : MonoBehaviour
{
    [Header("Renderer")]
    [SerializeField] protected SpriteRenderer _spriteRenderer;


    public void SetSprite(Sprite sprite)
    {
        _spriteRenderer.sprite = sprite;
        _spriteRenderer.size = Vector2.one;
    }
    public void SetSprite(Sprite sprite, Vector2 size)
    {
        _spriteRenderer.sprite = sprite;
        _spriteRenderer.size = size;
    }
}
