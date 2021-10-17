using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectBase : ObjectBase
{
    [Header("[사운드]")]
    public AudioEvent soundPrefab;
    public PlayType soundPlayType = PlayType.OnEnable;

    [Header("[이펙트]")]
    public bool isPermanent = false;
    public float duration;

    protected override void Awake()
    {
        base.Awake();

        if (null != soundPrefab)
        {
            SoundManager.Instance.LoadEvent(soundPrefab);
        }
    }

    public override void OnSpawn(ObjectBasePool instancePool)
    {
        base.OnSpawn(instancePool);

        if (soundPlayType == PlayType.OnEnable)
        {
            PlaySound();
        }
    }

    public void PlaySound()
    {
        if (null != soundPrefab)
        {
            SoundManager.Instance.PlayEvent(soundPrefab, transform);
        }
    }

    public override void OnDespawn()
    {
        if (null != soundPrefab)
        {
            SoundManager.Instance.StopEvent(soundPrefab, transform);
        }
    }

    public void Reset()
    {
        gameObject.SetActive(false);
        gameObject.SetActive(true);
    }
}