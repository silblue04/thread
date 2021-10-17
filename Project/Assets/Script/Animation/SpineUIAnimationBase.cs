using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class SpineUIAnimationBase : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] protected SpineUIAnimationPlayer _animationPlayer;
    [SerializeField] private bool _isNeededEnableAutoPlay = false;
    public SpineUIAnimationPlayer AnimationPlayer { get { return _animationPlayer; } }
    public bool IsNeededEnableAutoPlay { get { return _isNeededEnableAutoPlay; } set {_isNeededEnableAutoPlay = value; } }

    protected string _curSkinName = string.Empty;
    protected string _setAnimationName = string.Empty;
    protected bool _setAnimationLoop = false;

    private AnimationType _curAnimationType = AnimationType.NONE;
    public AnimationType CurAnimationType { get { return _curAnimationType; } }
    
    
    protected virtual void Awake()
    {
        _curSkinName = _animationPlayer.SkeletonGraphic.initialSkinName;
        _setAnimationName = _animationPlayer.SkeletonGraphic.startingAnimation;
        _setAnimationLoop = _animationPlayer.SkeletonGraphic.startingLoop;

        if(_setAnimationName == null)
        {
            StopAnimation();
        }
    }

    protected virtual void OnEnable()
    {
        if(_isNeededEnableAutoPlay == false)
        {
            return;
        }

        _animationPlayer.SetAnimation(0, _setAnimationName, _setAnimationLoop);
    }

    protected virtual void OnDiable()
    {
        if(_isNeededEnableAutoPlay == false)
        {
            return;
        }

        StopAnimation();
    }

    public virtual void SetCallbackAnimationCompleted(AnimationType animType, System.Action callback)
    {
        _animationPlayer.SetAnimationCompleteCallback((entity) =>
        {
            string entityAnimationName = entity.Animation.Name;
            if(entityAnimationName.Equals(_GetAnimationName(animType)))
            {
                callback?.Invoke();
            }
        });
    }

    public virtual void SetCallbackAnimationKey(AnimationType animType, string key, System.Action callback)
    {
        _animationPlayer.SetAnimationHandleEventCallback((entity, e) =>
            {
                string entityAnimationName = entity.Animation.Name;
                if (entityAnimationName.Equals(_GetAnimationName(animType)))
                {
                    if (e.Data.Name == key)
                    {
                        callback?.Invoke();
                    }
                }
            });
    }

    public bool SetSkin(string name)
    {
        if(_curSkinName.Equals(name))
        {
            return false;
        }
        
        _animationPlayer.SetSkin(name);
        _curSkinName = name;
        return true;
    }

    public virtual void PlayAnimation()
    {
        _animationPlayer.SetAnimation(0, _setAnimationName, _setAnimationLoop);
    }

    public virtual bool PlayAnimation(AnimationType animType)
    {
        if(animType == AnimationType.NONE)
        {
            StopAnimation();
            return false;
        }
        
        if(_curAnimationType == animType)
        {
            return false;
        }

        _curAnimationType = animType;

        string animationName = _GetAnimationName(animType);
        bool loop = _GetAnimationLoopState(animType);

        _animationPlayer.SetAnimation(0, animationName, loop);

        return true;
    }

    public virtual bool PlayAnimation(AnimationType animType, AnimationType nextAnimType)
    {
        if(PlayAnimation(animType) == false)
        {
            return false;
        }

        if(nextAnimType == AnimationType.NONE)
        {
            return false;
        }

        _curAnimationType = nextAnimType;

        string animationName = _GetAnimationName(nextAnimType);
        bool loop = _GetAnimationLoopState(nextAnimType);
        _animationPlayer.AddAnimation(0, animationName, loop);

        return true;
    }

    protected virtual bool _GetAnimationLoopState(AnimationType animType)
    {
        return animType.Loop;
    }
    protected virtual string _GetAnimationName(AnimationType animType)
    {
        return animType.AnimationName;
    }

    public virtual void StopAnimation()
    {
        _animationPlayer.StopAnimation();
    }
}
