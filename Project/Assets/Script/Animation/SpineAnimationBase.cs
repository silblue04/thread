using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SerializeField] public class AnimationType
{
    public static readonly AnimationType NONE = new AnimationType("NONE");

    public override string ToString()
    {
        return AnimationName;
    }

    protected AnimationType(string animationName, bool loop = false)
    {
        this.AnimationName = animationName;
        this.Loop = loop;
    }

    public string AnimationName { get; private set; }
    public bool Loop { get; private set; }
}

public abstract class SpineAnimationBase : MonoBehaviour
{
    [Header("Animation")]
    [SerializeField] protected SpineAnimationPlayer _animationPlayer;
    [SerializeField] private bool _isNeededEnableAutoPlay = false;
    public SpineAnimationPlayer AnimationPlayer { get { return _animationPlayer; } }
    public bool IsNeededEnableAutoPlay { get { return _isNeededEnableAutoPlay; } set {_isNeededEnableAutoPlay = value; } }

    private string _curSkinName = string.Empty;
    private string _setAnimationName = string.Empty;
    private bool _setAnimationLoop = false;

    private AnimationType _curAnimationType = AnimationType.NONE;
    public AnimationType CurAnimationType { get { return _curAnimationType; } }
    
    
    protected virtual void Awake()
    {
        _curSkinName = _animationPlayer.SkeletonAnimation.initialSkinName;
        _setAnimationName = _animationPlayer.SkeletonAnimation.AnimationName;
        _setAnimationLoop = _animationPlayer.SkeletonAnimation.loop;

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

    public bool IsAlreadyAppliedSkin(string name)
    {
        return _curSkinName.Equals(name);
    }

    public bool SetSkin(string name)
    {
        if(IsAlreadyAppliedSkin(name))
        {
            return false;
        }
        
        _animationPlayer.SetSkin(name);
        _curSkinName = name;
        return true;
    }

    public virtual void SetRightOrLeft(bool isRight)
    {
        this.gameObject.transform.localScale = new Vector3(
            Mathf.Abs(this.gameObject.transform.localScale.x) * (isRight ? 1.0f : -1.0f)
            , this.gameObject.transform.localScale.y
            , this.gameObject.transform.localScale.z);
    }

    public void PlayAnimation()
    {
        _animationPlayer.SetAnimation(0, _setAnimationName, _setAnimationLoop);        
    }

    public void PlayAnimation(System.Action OnEnd)
    {
        _animationPlayer.SetAnimationCompleteCallback((arg) => OnEnd?.Invoke());
        _animationPlayer.SetAnimation(0, _setAnimationName, _setAnimationLoop);
    }

    public virtual bool PlayAnimation(AnimationType animType)
    {
        if(animType == AnimationType.NONE)
        {
            StopAnimation();
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
