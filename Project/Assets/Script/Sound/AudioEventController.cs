using System.Collections;
using UnityEngine;


public enum PlayType
{
    OnAwake = 0,
    OnEnable,
    OnCalled,
}

public class AudioEventController : MonoBehaviour
{
    public AudioEvent soundPrefab;
    public PlayType playType = PlayType.OnAwake;
    public bool onDisableStop = false;

    public virtual void Awake()
    {
        SoundManager.Instance.LoadEvent(soundPrefab);

        if (PlayType.OnAwake == playType)
        {
            Play();
        }
    }

    public virtual void OnEnable()
    {
        if (PlayType.OnEnable == playType)
        {
            Play();
        }
    }

    public virtual void OnDisable()
    {
        if (true == onDisableStop)
        {
            Stop();
        }
    }

    public virtual void Play()
    {
        SoundManager.Instance.PlayEvent(soundPrefab, null);
    }

    public virtual void Stop()
    {
        SoundManager.Instance.StopEvent(soundPrefab, null);
    }
}