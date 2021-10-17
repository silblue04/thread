using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using System;


public enum MixerGroupType
{
    None = 0,
    BGM,
    SFX,
    DuckBGM,
}

[Serializable]
public class AudioLayer
{
    public AudioClip clip;
    [RangeAttribute(0f, 1f)]
    public float volume = 1f;
    [RangeAttribute(-12f, 12f)]
    public float pitch = 1f;
    [RangeAttribute(0f, 1f)]
    public float random = 0f;
    public float fadeInTime = 0f;
    public float fadeOutTime = 0f;
    public float randomPitch = 0f;
    public float triggerDelay = 0f;
    public float randomTriggerDelay = 0f;
    public float startOffset = 0f;
    public bool loop = false;

    [HideInInspector]
    public int playingSamples = 0;
}

public class AudioEvent : MonoBehaviour
{
    public List<AudioLayer> layers = new List<AudioLayer>();
    public MixerGroupType mixerGroup = MixerGroupType.None;
    public int maxPlaybackCount = 1;
    [RangeAttribute(0, 256)]
    public int priority = 128;
    public bool is3D = false;

    public enum PlaybackStyle
    {
        StealOldest = 0,
        StealLowest,
        PreventNew,
    }

    public PlaybackStyle style = PlaybackStyle.StealOldest;
    public bool randomNoRepeat = false;

    protected float[] _randomWeights = null;
    protected float _tatalRandomWeight = 0f;
    protected int _lastPlayedLayerIndex = -1;
    protected bool _isPlaying = false;
    public bool IsPlaying
    {
        get { return _isPlaying; }
    }

    protected void _CalcRandomWeight(int exceptIndex)
    {
        if (1 < layers.Count)
        {
            _randomWeights = new float[layers.Count];

            for (int i = 0, count = layers.Count; i < count; ++i)
            {
                if (i == exceptIndex)
                {
                    _randomWeights[i] = 0f;
                    continue;
                }

                _tatalRandomWeight += layers[i].random;
                _randomWeights[i] = _tatalRandomWeight;
            }
        }
    }

    public virtual void Awake()
    {
        _CalcRandomWeight(-1);
    }

    public void ForcePlay2D()
    {
        Play(null);
    }

    public virtual void Play(Transform target)
    {
        if (1 == layers.Count)
        {
            _PlayLayer(layers[0], target);
        }
        else if (0f == _tatalRandomWeight)
        {
            if (true == randomNoRepeat && -1 != _lastPlayedLayerIndex)
            {
                _PlayRandom(_lastPlayedLayerIndex, target);
            }
            else
            {
                _PlayRandom(-1, target);
            }
        }
        else
        {
            if (true == randomNoRepeat && -1 != _lastPlayedLayerIndex)
            {
                _PlayRandomWeight(_lastPlayedLayerIndex, target);
            }
            else
            {
                _PlayRandomWeight(-1, target);
            }
        }
    }

    protected void _PlayRandom(int exceptIndex, Transform target)
    {
        if (-1 != exceptIndex)
        {
            AudioLayer[] candidates = layers.SkipWhile((x, index) => index == exceptIndex).ToArray();

            if (2 < candidates.Length)
            {
                _PlayLayer(candidates[UnityEngine.Random.Range(0, candidates.Length)], target);
            }
            else
            {
                _PlayLayer(candidates[0], target);
            }
        }
        else
        {
            _PlayLayer(layers[UnityEngine.Random.Range(0, layers.Count)], target);
        }
    }

    protected void _PlayRandomWeight(int exceptIndex, Transform target)
    {
        if (-1 != exceptIndex)
        {
            AudioLayer[] candidates = layers.SkipWhile((x, index) => index == exceptIndex).ToArray();

            if (2 < candidates.Length)
            {
                _CalcRandomWeight(exceptIndex);

                float selectedRand = UnityEngine.Random.Range(0, _tatalRandomWeight);
                for (int i = 0, count = candidates.Length; i < count; ++i)
                {
                    if (selectedRand < _randomWeights[i])
                    {
                        _PlayLayer(candidates[i], target);
                        break;
                    }
                }
            }
            else
            {
                _PlayLayer(candidates[0], target);
            }
        }
        else
        {
            float selectedRand = UnityEngine.Random.Range(0, _tatalRandomWeight);
            for (int i = 0, count = layers.Count; i < count; ++i)
            {
                if (selectedRand < _randomWeights[i])
                {
                    _PlayLayer(layers[i], target);
                }
            }
        }
    }

    protected virtual void _PlayLayer(AudioLayer selected, Transform target)
    {
        if (layers.Sum(x => x.playingSamples) >= maxPlaybackCount)
        {
            switch (style)
            {
                case PlaybackStyle.StealOldest:
                    {
                        SoundManager.Instance.StopOldestSample(layers);
                        _PlaySample(selected, target);
                        break;
                    }

                case PlaybackStyle.StealLowest:
                    {
                        SoundManager.Instance.StopLowestPrioritySample();
                        _PlaySample(selected, target);
                        break;
                    }

                case PlaybackStyle.PreventNew:
                default:
                    break;
            }
        }
        else
        {
            _PlaySample(selected, target);
        }
    }

    protected void _PlaySample(AudioLayer selected, Transform target)
    {
        if (null == selected.clip)
        {
            Debug.LogError(string.Format("There is no audio clip in {0}", gameObject.name));
            return;
        }

        // if (Debug.isDebugBuild)
        // {
        //     Debug.LogFormat("Play Audio Prefab : {0} clip : {1}", gameObject.name, selected.clip.name);
        // }

        _isPlaying = true;
        _lastPlayedLayerIndex = layers.FindIndex(x => x == selected);

        if (false == is3D || null == target)
        {
            SoundManager.Instance.PlaySample(selected, priority, mixerGroup, null);
        }
        else
        {
            SoundManager.Instance.PlaySample(selected, priority, mixerGroup, target);
        }
    }

    public virtual void Stop()
    {
        // if (Debug.isDebugBuild)
        // {
        //     Debug.LogFormat("Stop Audio Prefab : {0}", gameObject.name);
        // }

        _isPlaying = false;
        SoundManager.Instance.StopSamples(layers);
    }

    public virtual void Pause()
    {
        // if (Debug.isDebugBuild)
        // {
        //     Debug.LogFormat("Pause Audio Prefab : {0}", gameObject.name);
        // }

        _isPlaying = false;
        SoundManager.Instance.PauseSamples(layers);
    }

    public virtual void UnPause()
    {
        // if (Debug.isDebugBuild)
        // {
        //     Debug.LogFormat("Resume Audio Prefab : {0}", gameObject.name);
        // }

        _isPlaying = false;
        SoundManager.Instance.UnPauseSamples(layers);
    }
}