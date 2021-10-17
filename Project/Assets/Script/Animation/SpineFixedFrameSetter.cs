using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Spine;
using Spine.Unity;


public class SpineFixedFrameSetter : MonoBehaviour
{
	[Range(0,1)]
	public float frameRatio = 0.0f;
    [SerializeField] private Spine.Unity.SkeletonGraphic _target;

    public void Awake()
    {
        SetFrame();
    }

    public void SetFrame()
    {
        Util.SetSkeletonGraphicAnimationFixedFrame(_target, frameRatio);
    }
}
