using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Essence : ObjectBase
{
    [Header("Essence > Renderer")]
    [SerializeField] private SpritePlane _renderer;


    public void Init(Vector2 pos)
    {
        this.transform.position = pos; // TODO : tmp
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag(DefsUnit.TriggerName.RANDOM_BOX))
        {
            Despawn(); // TODO : tmp
        }
    }
}