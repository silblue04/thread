using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAnimationType : AnimationType
{
    public static readonly HeroAnimationType Idle = new HeroAnimationType("idle");
    public static readonly HeroAnimationType Appear = new HeroAnimationType("walk_in");
    public static readonly HeroAnimationType Breath = new HeroAnimationType("breath", true);
    public static readonly HeroAnimationType Disappear = new HeroAnimationType("walk_out");
    
    private HeroAnimationType(string animationName, bool loop = false)
       : base(animationName, loop)
    {

    }
}

public class HeroAnimation : SpineAnimationBase
{

}
