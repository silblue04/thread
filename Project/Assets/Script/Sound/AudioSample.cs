using System.Collections;
using UnityEngine;


public class AudioSample : MonoBehaviour
{
    public AudioSource audioSource;

    [HideInInspector]
    public float playStartTime = 0f;

    private AudioLayer _curPlayedLayer = null;
    private Coroutine _fade = null;
    private Coroutine _stop = null;
    private Transform _target = null;

    public void Init()
    {
        audioSource.playOnAwake = false;
    }

    public void Pause()
    {
        audioSource.Pause();
    }

    public void UnPause()
    {
        audioSource.UnPause();
    }

    public void Play(AudioClip clip, float vol)
    {
        audioSource.outputAudioMixerGroup = SoundManager.Instance.volumeController.GetAudioMixerGroup(MixerGroupType.SFX);

        audioSource.clip = clip;
        audioSource.volume = vol;
        audioSource.spatialBlend = 0f;
        audioSource.loop = false;
        audioSource.time = 0f;
        audioSource.pitch = 1f;

        _curPlayedLayer = null;
        _target = null;
        _Play(0f, 0f);
    }

    public void Play(AudioLayer layer, int priority, MixerGroupType group, Transform target = null)
    {
        _curPlayedLayer = layer;
        _target = target;

        audioSource.priority = priority;

        audioSource.outputAudioMixerGroup = SoundManager.Instance.volumeController.GetAudioMixerGroup(group);

        audioSource.volume = layer.volume;
        audioSource.clip = layer.clip;
        audioSource.loop = layer.loop;
        audioSource.time = layer.startOffset;

        if (null == target)
        {
            audioSource.spatialBlend = 0f;
        }
        else
        {
            audioSource.spatialBlend = 1f;
        }

        if (0f < layer.randomPitch)
        {
            audioSource.pitch = layer.pitch + Random.Range(0f, layer.randomPitch);
        }
        else
        {
            audioSource.pitch = layer.pitch;
        }

        float delay = 0f;
        if (0f < layer.randomTriggerDelay)
        {
            delay = Random.Range(0f, layer.randomTriggerDelay);
        }
        else if (0f < layer.triggerDelay)
        {
            delay = layer.triggerDelay;
        }

        if (0f < layer.fadeInTime)
        {
            _fade = StartCoroutine(_FadeIn(delay, layer.volume, layer.fadeInTime));
        }

        _Play(delay, layer.fadeOutTime);
    }

    void LateUpdate()
    {
        if (null != _target)
        {
            transform.position = _target.position;
        }
    }

    private IEnumerator _FadeIn(float delay, float volume, float time)
    {
        if (0f < delay)
        {
            yield return YieldInstructionCache.WaitForSeconds(delay);
        }

        float passedTime = 0f;
        while (passedTime <= time)
        {
            passedTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(0f, volume, Mathf.Clamp01(passedTime / time));
            yield return null;
        }
    }

    private void _Play(float delay, float fadeOutTime)
    {
        if (0 < fadeOutTime)
        {
            fadeOutTime = Mathf.Clamp(fadeOutTime, 0f, audioSource.clip.length);
        }

        float duration = delay + audioSource.clip.length - fadeOutTime;

        if (false == audioSource.loop)
        {
            _stop = StartCoroutine(_Stop(duration));
        }

        if (0f < delay)
        {
            audioSource.PlayDelayed(delay);
        }
        else
        {
            audioSource.Play();
        }

        if (null != _curPlayedLayer)
        {
            _curPlayedLayer.playingSamples++;
        }

        playStartTime = Time.time;
    }

    public IEnumerator _Stop(float time)
    {
        yield return YieldInstructionCache.WaitForSeconds(time);
        Stop();
    }

    public void Stop()
    {
        if (playStartTime == Time.time)
        {
            Despawn();
            return;
        }

        if (null != _stop)
        {
            StopCoroutine(_stop);
            _stop = null;
        }

        if (null != _fade)
        {
            StopCoroutine(_fade);
            _fade = null;
        }

        if (null != _curPlayedLayer)
        {
            _fade = StartCoroutine(_FadeOut(_curPlayedLayer.fadeOutTime));
        }
        else
        {
            Despawn();
        }
    }

    private IEnumerator _FadeOut(float time)
    {
        if (0f < time)
        {
            float passedTime = 0f;
            float startVolume = audioSource.volume;

            while (passedTime <= time && true == audioSource.isPlaying)
            {
                passedTime += Time.deltaTime;
                audioSource.volume = Mathf.Lerp(startVolume, 0f, Mathf.Clamp01(passedTime / time));
                yield return null;
            }
        }

        yield return YieldInstructionCache.WaitForSeconds(0.1f);
        audioSource.volume = 0f;
        Despawn();
    }

    public void Despawn()
    {
        if (null != _stop)
        {
            StopCoroutine(_stop);
            _stop = null;
        }

        if (null != _fade)
        {
            StopCoroutine(_fade);
            _fade = null;
        }

        audioSource.Stop();
        audioSource.volume = 0f;

        playStartTime = float.MaxValue;

        if (null != _curPlayedLayer)
        {
            _curPlayedLayer.playingSamples--;
        }

        _target = null;
        SoundManager.Instance.DespawnSample(_curPlayedLayer, this);
    }
}