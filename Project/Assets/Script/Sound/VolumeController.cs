using UnityEngine;
using UnityEngine.Audio;


public class VolumeController : MonoBehaviour
{
    public AudioMixer masterMixer;

    public float MasterVolume
    {
        set
        {
            masterMixer.SetFloat(DefsString.Sound.MASTER_VOLUME, Util.GetDecibel(value));
        }
    }

    private float _BGMVolume = 1f;
    public float BGMVolume
    {
        get { return _BGMVolume; }
        set
        {
            _BGMVolume = value;
            masterMixer.SetFloat(DefsString.Sound.BGM_VOLUME, Util.GetDecibel(_BGMVolume));
        }
    }

    private float _SFXVolume = 1f;
    public float SFXVolume
    {
        get { return _SFXVolume; }
        set
        {
            _SFXVolume = value;
            masterMixer.SetFloat(DefsString.Sound.SFX_VOLUME, Util.GetDecibel(_SFXVolume));
        }
    }

    public AudioMixerGroup GetAudioMixerGroup(MixerGroupType type)
    {
        AudioMixerGroup[] groups = masterMixer.FindMatchingGroups(type.ToString());

        if (null != groups && 0 < groups.Length)
        {
            return groups[0];
        }

        return null;
    }
}
