using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CommonUIAnimationType : AnimationType
{
    public static readonly CommonUIAnimationType Gacha_Idle = new CommonUIAnimationType("gacha_idle");
    public static readonly CommonUIAnimationType Gacha_Lever = new CommonUIAnimationType("gacha_lever", false);
    public static readonly CommonUIAnimationType Gacha_Capsule_Start = new CommonUIAnimationType("gacha_capsule_1", false);
    public static readonly CommonUIAnimationType Gacha_Capsule_Next = new CommonUIAnimationType("gacha_capsule_2", false);

    private CommonUIAnimationType(string animationName, bool loop = true)
       : base(animationName, loop)
    {
        
    }
}

public class CommonUIAnimation : SpineUIAnimationBase
{

}
