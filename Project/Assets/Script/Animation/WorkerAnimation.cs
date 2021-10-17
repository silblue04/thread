using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerAnimationType : AnimationType
{
    public static readonly WorkerAnimationType Idle = new WorkerAnimationType("idle");
    public static readonly WorkerAnimationType Work = new WorkerAnimationType("walk");
    public static readonly WorkerAnimationType Tada = new WorkerAnimationType("jump");
    
    private WorkerAnimationType(string animationName, bool loop = true)
       : base(animationName, loop)
    {

    }
}

public class WorkerAnimation : SpineAnimationBase
{

}
