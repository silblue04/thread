using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;
using DG.Tweening;


public class EffectGetGold : EffectBase
{
    [Header("Particle System")]
    [SerializeField] private ParticleSystem _partileSystem;

    protected override void Awake()
    {
        base.Awake();

        _partileSystem.Emit(100);
    }

    public void SetParticleNum(int num)
    {
        _partileSystem.Emit(num);
    }
}
