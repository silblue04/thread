using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using Spine.Unity.AttachmentTools;


public class LoadPakAnimationType : AnimationType
{
    public static readonly LoadPakAnimationType BeforeLoading = new LoadPakAnimationType("before_loading");
    public static readonly LoadPakAnimationType Loading = new LoadPakAnimationType("loading", true);
    public static readonly LoadPakAnimationType AfterLoading = new LoadPakAnimationType("after_loading");
    
    private LoadPakAnimationType(string animationName, bool loop = false)
       : base(animationName, loop)
    {
        
    }
}

public class LoadPakUIAnimation : SpineUIAnimationBase
{
    [Header("Bone")]
    [SerializeField] private GameObject _bonePakLogoObj;

    [Header("Pak logo")]
    [SerializeField] private Image _pakLogo;


    protected override void Awake()
    {
        _bonePakLogoObj.SetActive(false);
        base.Awake();
    }

    public override void PlayAnimation()
    {
        bool isNeededBonePakLogo = (_setAnimationName.Equals(LoadPakAnimationType.AfterLoading.ToString()));
        _bonePakLogoObj.SetActive(isNeededBonePakLogo);

        if(isNeededBonePakLogo)
        {
            _UpdatePakLogo();
        }

        base.PlayAnimation();
    }

    public override bool PlayAnimation(AnimationType animType)
    {
        bool isNeededBonePakLogo = (animType == LoadPakAnimationType.AfterLoading);
        _bonePakLogoObj.SetActive(isNeededBonePakLogo);

        if(isNeededBonePakLogo)
        {
            _UpdatePakLogo();
        }

        return base.PlayAnimation(animType);
    }

    private void _UpdatePakLogo()
    {
        // LocalStageInfo LocalStageInfo       = LocalInfoConnecter.Instance.LocalStageInfo;
        // StageContainer StageContainer       = LocalStageInfo.CurStageContainer;
        // var pakData                         = StageContainer.PakData;

        // Sprite sprite = ResourceManager.Instance.GetPakSkinCollector(StageContainer.pak_idx).Get(pakData.logo_key);
        // _pakLogo.sprite = sprite;
    }
}
