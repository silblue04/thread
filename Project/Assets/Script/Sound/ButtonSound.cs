using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


//[RequireComponent(typeof(Button))]
public class ButtonSound : MonoBehaviour, IPointerDownHandler, IPointerClickHandler 
{
    [Header("눌렀을 때의 사운드")]
    public AudioEvent startSoundPrefab = null;
    [Header("손을 땠을 때의 사운드")]    
    public AudioEvent buttonSoundPrefab;


    private Button _targetButton = null;

    void Awake()
    {
        _targetButton = GetComponent<Button>();

        if (startSoundPrefab != null)
        {
            SoundManager.Instance.LoadEvent(startSoundPrefab);
        }
        if (buttonSoundPrefab != null)
        {
            SoundManager.Instance.LoadEvent(buttonSoundPrefab);
        }
    }

    private void _PlaySound(bool endSound = true)
    {
        if (false == _targetButton.interactable)
            return;

        if(endSound)
        {
            if(buttonSoundPrefab != null)
                SoundManager.Instance.PlayEvent(buttonSoundPrefab, null);
        }
        else
        {
            if(startSoundPrefab != null)
                SoundManager.Instance.PlayEvent(startSoundPrefab, null);
        }
    }

    //누름
    public void OnPointerDown(PointerEventData eventData)
    {
        if (false == _targetButton.interactable)
            return;

        _PlaySound(false);
    }

    //땜
    public void OnPointerClick(PointerEventData eventData)
    {
        if (false == _targetButton.interactable)
            return;
            
        _PlaySound(true);
    }
}