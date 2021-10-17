using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(SkeletonAnimation))]
public class SpineAnimationPlayer : MonoBehaviour
{
    [Header("Spine")]
    [SerializeField] private SkeletonAnimation _skeletonAnimation;
    public SkeletonAnimation SkeletonAnimation { get { return _skeletonAnimation; }}
    [SerializeField] private MeshRenderer _meshRenderer;
    public MeshRenderer MeshRenderer { get { return _meshRenderer; }}

    private MaterialPropertyBlock _block;
    private System.Action<TrackEntry> CompleteCallback = null;

    private int _fillAlpha;
    private int _fillColor;
    private int _tintColor;

    void Awake()
    {
        if (_skeletonAnimation == null)
        {
            _skeletonAnimation = GetComponent<SkeletonAnimation>();
        }
        if (_meshRenderer == null)
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }


        _block = new MaterialPropertyBlock();
        _meshRenderer.SetPropertyBlock(_block);

        _fillAlpha = Shader.PropertyToID("_FillAlpha");
        _fillColor = Shader.PropertyToID("_FillColor");
        _tintColor = Shader.PropertyToID("_Color");
    }

    public void SetSortingOrder(int order)
    {
        _meshRenderer.sortingOrder = order;
    }

    // absolute use shader skeleton fill;
    public void SetOpaque(float value)
    {
        if (_block != null)
        {
            _block.SetFloat(_fillAlpha, value); // Make the fill opaque.
        }
        _meshRenderer.SetPropertyBlock(_block);
    }

    // absolute use shader skeleton tint;
    public void SetTintColor(Color color)
    {
        if (_block != null)
        {
            _block.SetColor(_tintColor, color);
        }
        _meshRenderer.SetPropertyBlock(_block);
    }

    // absolute use shader skeleton fill;
    public void SetFlashColor(Color color)
    {
        if (_block != null)
        {
            _block.SetColor(_fillColor, color); // Fill with white.
        }
        _meshRenderer.SetPropertyBlock(_block);
    }

    public void SetAnimationHandleEventCallback(Spine.AnimationState.TrackEntryEventDelegate HandleEvent)
    {
        if (_skeletonAnimation.AnimationState != null)
        {
            _skeletonAnimation.AnimationState.Event += HandleEvent;
        }
    }

    public void SetAnimationCompleteCallback(System.Action<TrackEntry> onEventComplete)
    {
        if (_skeletonAnimation.AnimationState != null)
        {
            _skeletonAnimation.AnimationState.Complete += (entry) => {
                onEventComplete?.Invoke(entry);
            };
        }
    }

    public void SetAnimationCompleteCallbackOnce(System.Action<TrackEntry> onEventComplete)
    {
        if (_skeletonAnimation.AnimationState != null)
        {
            CompleteCallback = onEventComplete;
            _skeletonAnimation.AnimationState.Complete -= OnCompleteAni;
            _skeletonAnimation.AnimationState.Complete += OnCompleteAni;
        }
    }

    private void OnCompleteAni(TrackEntry entry)
    {
        CompleteCallback?.Invoke(entry);
        CompleteCallback = null;
    }

    public void SetMaterialAndSkeletonDataAsset(Material material, SkeletonDataAsset skeletonDataAsset)
    {
        _meshRenderer.material = material;
        _skeletonAnimation.skeletonDataAsset = skeletonDataAsset;
        _skeletonAnimation.Initialize(true);
    }

    public void SetTimeScale(float ratio)
    {
        _skeletonAnimation.AnimationState.TimeScale = ratio;
    }

    public void SetColor(Color color)
    {
        _skeletonAnimation.skeleton.SetColor(color);
    }

    public void StopAnimation(int trackIndex = 0)
    {
        if (_skeletonAnimation.AnimationState != null)
        {
            _skeletonAnimation.AnimationState.ClearTrack(0);
            _skeletonAnimation.timeScale = 0.0f;
        }
        _meshRenderer.enabled = false;
    }

    public void SetAnimation(int trackIndex, string animationName, bool loop)
    {
        if (_skeletonAnimation.AnimationState != null)
        {
            _skeletonAnimation.AnimationState.ClearTrack(0);
            _skeletonAnimation.timeScale = 1.0f;
            _skeletonAnimation.AnimationState.SetAnimation(0, animationName, loop);
        }
        _meshRenderer.enabled = true;
    }

    public void AddAnimation(int trackIndex, string animationName, bool loop, float delay = 0.0f)
    {
        if (_skeletonAnimation.AnimationState != null)
        {
            _skeletonAnimation.AnimationState.AddAnimation(0, animationName, loop, delay);
        }
    }

    public string GetCurrentAnimationName()
    {
        if (_skeletonAnimation.AnimationState != null)
        {
            return _skeletonAnimation.AnimationState.GetCurrent(0).Animation.Name;
        }
        return string.Empty;
    }

    // [Obsolete("please using cached GetBone, in update")]
    // public Vector3 GetPosition(string boneName)
    // {
    //     Bone bone = GetBone(boneName);
    //     if (bone != null)
    //     {
    //         return bone.GetLocalPosition();
    //     }
    //     return Vector3.zero;
    // }

    public Bone GetBone(string boneName)
    {
        return _skeletonAnimation.skeleton.FindBone(boneName);
    }

    public void SetSkin(string strSkinName)
    {
        if (string.IsNullOrEmpty(strSkinName))
        {
            return;
        }
        
        _skeletonAnimation.Skeleton.SetSkin(strSkinName);
        _skeletonAnimation.Skeleton.SetSlotsToSetupPose();
        //_skeletonAnimation.AnimationState.Apply(_skeletonAnimation.Skeleton);
    }
}
