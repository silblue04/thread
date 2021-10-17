using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class TabButtonStateInfo
{
    public Color color = Color.white;
    public Sprite sprite = null;
    public bool isReadOnly = false;
}

[RequireComponent(typeof(Graphic))]
public class TabButtonState : MonoBehaviour
{
    public TabButtonStateInfo selectedState;
    public TabButtonStateInfo pressedState;
    public TabButtonStateInfo normalState;

    private Graphic _targetGraphic = null;
    public Graphic Target
    {
        get
        {
            if (_targetGraphic == null)
            {
                _targetGraphic = GetComponent<Graphic>();
            }

            return _targetGraphic;
        }
    }

    private bool _initialized = false;

    public enum TabButtonStateType
    {
        Normal = 0,
        Pressed,
        Disabled,
    }

    public void InitSelectedState()
    {
        if (Target.GetType() == typeof(Image))
        {
            Image image = Target as Image;
            selectedState = new TabButtonStateInfo() { color = Target.color, sprite = image.sprite, isReadOnly = true };
        }
        else
        {
            selectedState = new TabButtonStateInfo() { color = Target.color, isReadOnly = true };
        }
    }

    public void InitNormalState()
    {
        if (Target.GetType() == typeof(Image))
        {
            Image image = Target as Image;
            image.color = normalState.color;
            if (normalState.sprite == null)
                image.sprite = selectedState.sprite;
            else
                image.sprite = normalState.sprite;
        }
        else
        {
            Target.color = normalState.color;
        }
    }


    public void SetSpriteToAllState(Sprite sprite)
    {
        selectedState.sprite = sprite;
        pressedState.sprite = sprite;
        normalState.sprite = sprite;
    }


    void Awake()
    {
        _Init( );
    }

    private void OnEnable()
    {
        selectedState.isReadOnly = true;
    }

    private void _Init()
    {
        if ( _initialized )
            return;

        InitNormalState( );

        _initialized = true;
    }

    public void DoStateTransition(TabButtonStateType state, bool instant, float colorDuration)
    {
        if ( _initialized == false )
        {
            _Init( );
        }

        TabButtonStateInfo currentStateInfo = _GetButtonState(state);
        Target.color = currentStateInfo.color;

        Image image = Target as Image;

        if (currentStateInfo.sprite != null)
        {
            image.sprite = currentStateInfo.sprite;
        }
        else
        {
            //sprite 변경이 없으면 selectedState에서 가져온다.
            if (selectedState.sprite != null)
                image.sprite = selectedState.sprite;
        }
    }

    private TabButtonStateInfo _GetButtonState(TabButtonStateType state)
    {
        switch (state)
        {
            case TabButtonStateType.Pressed:
                return pressedState;
            case TabButtonStateType.Disabled:
                return normalState;
            default:
                return selectedState;
        }
    }

#if UNITY_EDITOR
    [ContextMenu("Change To Selected")]
    public void ChangeToSelected()
    {
        DoStateTransition(TabButtonStateType.Disabled, true, 0);
    }

    [ContextMenu("Change To Pressed")]
    public void ChangeToPressed()
    {
        DoStateTransition(TabButtonStateType.Pressed, true, 0);
    }

    [ContextMenu("Change To Normal")]
    public void ChangeToNormal()
    {
        DoStateTransition(TabButtonStateType.Normal, true, 0);
    }
#endif
}
