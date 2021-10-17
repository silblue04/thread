using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;


public class InGameCamera : CameraBase
{
    [Header("-- Base Information ------")]
    [SerializeField] private Camera _camera;

    public void Init()
    {
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;
        float curScreenRatio = screenHeight/screenWidth;

        float MIN_SCREEN_RATIO_NOT_NEEDED_TO_ADJUST = 1.55f;
        if(curScreenRatio < MIN_SCREEN_RATIO_NOT_NEEDED_TO_ADJUST)
        {
            return;
        }

        float defaultWidth      = DefsDefault.DEFAULT_SCREEN_WIDTH;
        float defaultHeight     = DefsDefault.DEFAULT_SCREEN_HEIGHT;
        float defaultScreenRatio    = defaultHeight/defaultWidth;

        _camera.orthographicSize    = _camera.orthographicSize * curScreenRatio / defaultScreenRatio;
    }

}
