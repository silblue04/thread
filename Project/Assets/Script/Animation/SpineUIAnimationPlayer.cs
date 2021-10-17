using System;
using System.Collections;
using System.Collections.Generic;
using Spine;
using Spine.Unity;
using UnityEngine;

[RequireComponent(typeof(SkeletonGraphic))]
public class SpineUIAnimationPlayer : MonoBehaviour
{
    [Header("Spine")]
    [SerializeField] private SkeletonGraphic _skeletonGraphic;
    public SkeletonGraphic SkeletonGraphic { get { return _skeletonGraphic; }}


    void Awake()
    {
        if (_skeletonGraphic == null)
        {
            _skeletonGraphic = GetComponent<SkeletonGraphic>();
        }
    }

    public void SetAnimationHandleEventCallback(Spine.AnimationState.TrackEntryEventDelegate HandleEvent)
    {
        if (_skeletonGraphic.AnimationState == null)
        {
            _skeletonGraphic.Initialize(false);
        }

        if (_skeletonGraphic.AnimationState != null)
        {
            _skeletonGraphic.AnimationState.Event += HandleEvent;
        }
    }

    public void SetAnimationCompleteCallback(System.Action<TrackEntry> onEventComplete)
    {
        if (_skeletonGraphic.AnimationState == null)
        {
            _skeletonGraphic.Initialize(false);
        }
        
        if (_skeletonGraphic.AnimationState != null)
        {
            _skeletonGraphic.AnimationState.Complete += (entity) =>
            {
                onEventComplete(entity);
            };
        }
    }

    public void SetSkeletonDataAsset(SkeletonDataAsset skeletonDataAsset)
    {
        _skeletonGraphic.skeletonDataAsset = skeletonDataAsset;
        SetSkin(DefsString.Spine.DEFAULT_SKIN_NAME);
        _skeletonGraphic.Initialize(true);
    }

    public void SetTimeScale(float ratio)
    {
        _skeletonGraphic.AnimationState.TimeScale = ratio;
    }

    public void StopAnimation(int trackIndex = 0)
    {
        if (_skeletonGraphic.AnimationState == null)
        {
            _skeletonGraphic.Initialize(false);
        }

        if (_skeletonGraphic.AnimationState != null)
        {
            _skeletonGraphic.AnimationState.ClearTrack(0);
            _skeletonGraphic.timeScale = 0.0f;
        }

        _skeletonGraphic.enabled = false;
    }

    public void SetAnimation(int trackIndex, string animationName, bool loop)
    {
        if (_skeletonGraphic.AnimationState == null)
        {
            _skeletonGraphic.Initialize(false);
        }

        if (_skeletonGraphic.AnimationState != null)
        {
            _skeletonGraphic.AnimationState.ClearTrack(0);
            _skeletonGraphic.timeScale = 1.0f;
            _skeletonGraphic.AnimationState.SetAnimation(0, animationName, loop);
        }

        _skeletonGraphic.enabled = true;
    }

    public void AddAnimation(int trackIndex, string animationName, bool loop, float delay = 0.0f)
    {
        if (_skeletonGraphic.AnimationState == null)
        {
            _skeletonGraphic.Initialize(false);
        }

        if (_skeletonGraphic.AnimationState != null)
        {
            _skeletonGraphic.AnimationState.AddAnimation(0, animationName, loop, delay);
        }
    }

    public string GetCurrentAnimationName()
    {
        if (_skeletonGraphic.AnimationState == null)
        {
            _skeletonGraphic.Initialize(false);
        }

        if (_skeletonGraphic.AnimationState != null)
        {
            return _skeletonGraphic.AnimationState.GetCurrent(0).Animation.Name;
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

    public void SetSkin(string strSkinName)
    {
        if (string.IsNullOrEmpty(strSkinName))
        {
            return;
        }
        if(_skeletonGraphic == null)
            return;

        if(_skeletonGraphic.Skeleton == null)
        {
            _skeletonGraphic.Initialize(false);
        }
        
        _skeletonGraphic.Skeleton.SetSkin(strSkinName);
    }

    public void SetColor(Color color)
    {
        _skeletonGraphic.color = color;
    }
}
