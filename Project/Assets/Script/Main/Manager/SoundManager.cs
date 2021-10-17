using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : Singleton<SoundManager>
{
    public class EventContainer
    {
        public AudioEvent audioPrefab;
        public AudioEvent audioInstance;
        public Transform target = null;

        public EventContainer(AudioEvent prefab, Transform parent)
        {
            audioPrefab = prefab;
            audioInstance = Instantiate(audioPrefab, parent, false);
        }

        public void Play(Transform t)
        {
            target = t;
            audioInstance.Play(target);
        }

        public void Stop()
        {
            target = null;
            audioInstance.Stop();
        }
    }

    public VolumeController volumeController;

    [Space]
    [SerializeField] private AudioSource _bgm;
    public Transform eventPivot;
    public Transform samplePivot;
    public AudioSample baseSample;

    [Space]
    public int startSampleCount = 50;

    // 총 오디오 샘플 리스트
    private List<AudioSample> _samples = new List<AudioSample>();

    // AudioLayer를 통해서 재생중인 샘플들
    private Dictionary<AudioLayer, List<AudioSample>> _playingSamples = new Dictionary<AudioLayer, List<AudioSample>>();
    // AudioLayer가 없이 Clip으로  재생중인 샘플들
    private List<AudioSample> _playingClipSamples = new List<AudioSample>();

    private List<EventContainer> _globalEvents = new List<EventContainer>();
    private int _sampleIndex = 0;

    private bool _onBgm = true;
    private bool _onSound = true;

    public void Awake()
    {
        for (int i = 0, count = startSampleCount; i < count; ++i)
        {
            _samples.Add(_CreateSample());
        }
    }

    public void Init()
    {
        InitOption();
    }

    public void InitOption()
    {
        LocalOptionInfo LocalOptionInfo = LocalInfoConnecter.Instance.LocalOptionInfo;
        OptionState optionState = LocalOptionInfo.GetOptionState(OptionType.BGM);
        OnBgm((optionState == OptionState.On));

        optionState = LocalOptionInfo.GetOptionState(OptionType.SFX);
        OnSound((optionState == OptionState.On));
    }

    public void OnBgm(bool on)
    {
        _onBgm = on;
        _bgm.mute = (_onBgm) ? false : true;
    }

    public void OnSound(bool on)
    {
        _onSound = on;
    }

    private AudioSample _CreateSample()
    {
        AudioSample sample = Instantiate<AudioSample>(baseSample);
        sample.name = "SoundSample" + _sampleIndex++;
        sample.transform.SetParent(samplePivot);
        sample.gameObject.SetActive(false);
        sample.Init();

        return sample;
    }

    public EventContainer LoadEvent(AudioEvent prefab)
    {
        if (prefab == null)
            return null;

        EventContainer container = _globalEvents.Find(x => x.audioPrefab == prefab);
        if (null == container)
        {
            container = new EventContainer(prefab, eventPivot);

            _globalEvents.Add(container);

            return container;
        }

        return container;
    }

    public bool IsPlayingEvent(AudioEvent prefab, Transform target)
    {
        if (null != prefab)
        {
            EventContainer container = _globalEvents.Find(x => x.audioPrefab == prefab && x.target == target);

            if (null != container)
            {
                return container.audioInstance.IsPlaying;
            }
        }

        return false;
    }

    public void PlayEvent(AudioEvent prefab, Transform target)
    {
        if(_onSound == false)
        {
            return;
        }

        if (null != prefab)
        {
            EventContainer container = _globalEvents.Find(x => x.audioPrefab == prefab && x.target == target);
            if (null != container)
            {
                container.Play(target);
            }
            else
            {
                container = _globalEvents.Find(x => x.audioPrefab == prefab && x.target == null);
                if (null == container)
                {
                    container = LoadEvent(prefab);
                }

                container.Play(target);
            }
        }
    }

    public void StopEvent(AudioEvent prefab, Transform target)
    {
        if (null != prefab)
        {
            EventContainer container = _globalEvents.Find(x => x.audioPrefab == prefab && x.target == target);
            if (null != container)
            {
                container.Stop();
            }
            else
            {
                container = _globalEvents.Find(x => x.audioPrefab == prefab && x.target == null);
                if (null != container)
                {
                    container.Stop();
                }
            }
        }
    }

    private AudioSample _SpawnSameple()
    {
        AudioSample ret = null;
        if (0 < _samples.Count)
        {
            ret = _samples[0];
            _samples.RemoveAt(0);
        }
        else
        {
            ret = _CreateSample();
        }

        return ret;
    }

    public void DespawnSample(AudioLayer layer, AudioSample sample)
    {
        sample.gameObject.SetActive(false);

        if (null != layer && true == _playingSamples.ContainsKey(layer))
        {
            _playingSamples[layer].Remove(sample);

            if (0 == _playingSamples[layer].Count)
            {
                _playingSamples.Remove(layer);
            }
        }
        else
        {
            _playingClipSamples.Remove(sample);
        }

        _samples.Add(sample);
    }

    public void PlaySample(AudioLayer layer, int priority, MixerGroupType group, Transform target)
    {
        AudioSample sample = _SpawnSameple();

        if (null != sample)
        {
            if (false == _playingSamples.ContainsKey(layer))
            {
                _playingSamples.Add(layer, new List<AudioSample>());
            }

            _playingSamples[layer].Add(sample);
            sample.gameObject.SetActive(true);

            sample.Play(layer, priority, group, target);
        }
        else
        {
            Debug.LogErrorFormat("[ERROR] Can't create SoundSample --> {0}"
                , System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName);
        }
    }

    public void PauseAllSamples()
    {
        foreach (var layer in _playingSamples)
        {
            foreach (AudioSample sample in layer.Value)
            {
                sample.Pause();
            }
        }
    }

    public void StopAllSamples()
    {
        AudioSample[] playingSamples = _playingSamples.Values.SelectMany(x => x).ToArray();
        foreach (AudioSample playingSample in playingSamples)
        {
            playingSample.Stop();
        }
        _playingSamples.Clear();

        foreach (AudioSample playingSample in _playingClipSamples)
        {
            playingSample.Stop();
        }
        _playingClipSamples.Clear();
    }

    public void MuteAllSamples(bool mute)
    {
        if (mute == true)
        {
            volumeController.MasterVolume = 0f;
        }
        else
        {
            volumeController.MasterVolume = 1f;
        }
    }

    public void UnPauseSamples(List<AudioLayer> layers)
    {
        foreach (AudioLayer layer in layers)
        {
            if (true == _playingSamples.ContainsKey(layer))
            {
                AudioSample[] playingSamples = _playingSamples[layer].ToArray();
                foreach (AudioSample sample in playingSamples)
                {
                    sample.UnPause();
                }
            }
        }
    }

    public void PauseSamples(List<AudioLayer> layers)
    {
        foreach (AudioLayer layer in layers)
        {
            if (true == _playingSamples.ContainsKey(layer))
            {
                AudioSample[] playingSamples = _playingSamples[layer].ToArray();
                foreach (AudioSample sample in playingSamples)
                {
                    sample.Pause();
                }
            }
        }
    }

    public void StopSamples(List<AudioLayer> layers)
    {
        foreach (AudioLayer layer in layers)
        {
            if (true == _playingSamples.ContainsKey(layer))
            {
                AudioSample[] playingSamples = _playingSamples[layer].ToArray();
                foreach (AudioSample sample in playingSamples)
                {
                    sample.Stop();
                }
            }
        }
    }

    public void StopOldestSample(List<AudioLayer> layers)
    {
        AudioSample oldestSample = null;
        foreach (AudioLayer layer in layers)
        {
            if (true == _playingSamples.ContainsKey(layer))
            {
                if (0 < _playingSamples[layer].Count)
                {
                    if (null == oldestSample ||
                        oldestSample.playStartTime > _playingSamples[layer][0].playStartTime)
                    {
                        oldestSample = _playingSamples[layer][0];
                    }
                }
            }
        }

        if (null != oldestSample)
        {
            oldestSample.Despawn();
        }
    }

    public void StopLowestPrioritySample()
    {
        AudioSample[] playingSamples = _playingSamples.Values.SelectMany(x => x).OrderBy(x => x.audioSource.priority).ToArray();

        if (0 < playingSamples.Length)
        {
            playingSamples[0].Despawn();
        }
    }

    public void PlayClip(AudioClip clip, float vol)
    {
        AudioSample sample = _SpawnSameple();

        if (null != sample)
        {
            sample.gameObject.SetActive(true);
            sample.Play(clip, vol);

            _playingClipSamples.Add(sample);
        }
    }

    public void ReleaseAudioEvents()
    {
        StopAllSamples();

        foreach (var global in _globalEvents)
        {
            Destroy(global.audioInstance.gameObject);
        }

        _globalEvents.Clear();
    }
}
