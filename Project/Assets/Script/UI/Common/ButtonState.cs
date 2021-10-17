using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

[System.Serializable]
public class ButtonStateInfo
{
    public Color color = Color.white;
    public Sprite sprite = null;
    public bool isReadOnly = false;
}

[RequireComponent(typeof( Graphic ) )]
public class ButtonState : MonoBehaviour
{
    public ButtonStateInfo normalState;
    public ButtonStateInfo pressedState;
    public ButtonStateInfo disabledState;

    private bool _isInitialized = false;
    private ButtonStateType _currentStateType = ButtonStateType.Normal;

    private Graphic _targetGraphic = null;
    public Graphic Target
    {
        get
        {
            if ( _targetGraphic == null )
            {
                _targetGraphic = GetComponent<Graphic>( );
            }

            return _targetGraphic;
        }
    }

    public enum ButtonStateType
    {
        Normal = 0,
        Pressed,
        Disabled,
    }
    
    public void InitNormalState()
    {
        if ( Target.GetType( ) == typeof( Image ) )
        {
            Image image = Target as Image;            
            normalState = new ButtonStateInfo( ) { color = Target.color, sprite = image.sprite, isReadOnly = true };
        }
        else
        {
            normalState = new ButtonStateInfo( ) { color = Target.color, isReadOnly = true };
        }
    }

    void Awake ()
    {
        _Initialize( );
	}
    
    private void _Initialize( )
    {
        if ( _isInitialized == true )
            return;

        InitNormalState( );
        _isInitialized = true;
    }

    private void OnEnable( )
    {
        normalState.isReadOnly = true;
        DoStateTransition(_currentStateType, true, 0);
    }

    public void DoStateTransition( ButtonStateType state, bool instant, float colorDuration )
    {
        //awake 전에 세팅하려고 하면 먼저 normalstate init해줌
        if( _isInitialized == false)
        {
            _Initialize( );
        }

        _currentStateType = state;
        ButtonStateInfo currentStateInfo = _GetButtonState( state );
        Target.DOColor(currentStateInfo.color, colorDuration).SetUpdate(true);
        if( Target is Image )
        {
            Image image = Target as Image;

            if (currentStateInfo.sprite != null)
            {
                image.sprite = currentStateInfo.sprite;
            }
            else
            {
                //sprite 변경이 없으면 normalState에서 가져온다.
                if (normalState.sprite != null)
                    image.sprite = normalState.sprite;
            }
        }
        else if( Target is RawImage )
        {
            RawImage image = Target as RawImage;

            if (currentStateInfo.sprite != null)
            {
                image.texture = currentStateInfo.sprite.texture;
            }
            else
            {
                //sprite 변경이 없으면 normalState에서 가져온다.
                if (normalState.sprite != null)
                    image.texture = normalState.sprite.texture;
            }
        }
    }

    private ButtonStateInfo _GetButtonState( ButtonStateType state )
    {
        switch( state )
        {
            case ButtonStateType.Pressed:
                return pressedState;
            case ButtonStateType.Disabled:
                return disabledState;
            default:
                return normalState;
        }
    }

    public void SetSpriteImage( Sprite sprite )
    {
        normalState.sprite = disabledState.sprite = pressedState.sprite = sprite;
    }

    public void SetSpriteImage( ButtonStateType state, Sprite sprite )
    {
        switch( state )
        {
            case ButtonStateType.Pressed:   pressedState.sprite = sprite;   break;
            case ButtonStateType.Disabled:  disabledState.sprite = sprite;  break;
            default:                        normalState.sprite = sprite;    break;
        }
    }
    
#if UNITY_EDITOR
    [ContextMenu( "Change To Normal" )]
    public void ChangeToNormal( )
    {
        DoStateTransition( ButtonStateType.Normal, true, 0 );
    }

    [ContextMenu( "Change To Pressed" )]
    public void ChangeToPressed( )
    {
        DoStateTransition( ButtonStateType.Pressed, true, 0 );
    }

    [ContextMenu( "Change To Disabled" )]
    public void ChangeToDisabled( )
    {
        DoStateTransition( ButtonStateType.Disabled, true, 0 );
    }

    // [MenuItem("Tools/ChanageButtonState")]
    // public static void ChangeToTabButtonState()
    // {
    //     if (UnityEditor.Selection.objects.Length != 0)
    //     {
    //         GameObject parentObj = UnityEditor.Selection.objects[0] as GameObject;
    //         parentObj.AddComponent<TabButtonState>();
    //         parentObj.GetComponent<TabButtonState>().selectedState = new TabButtonStateInfo();
    //         parentObj.GetComponent<TabButtonState>().pressedState = new TabButtonStateInfo();
    //         parentObj.GetComponent<TabButtonState>().normalState = new TabButtonStateInfo();
    //         parentObj.GetComponent<TabButtonState>().selectedState.color = parentObj.GetComponent<ButtonState>().normalState.color;
    //         parentObj.GetComponent<TabButtonState>().pressedState.color = parentObj.GetComponent<ButtonState>().pressedState.color;
    //         parentObj.GetComponent<TabButtonState>().normalState.color = parentObj.GetComponent<ButtonState>().disabledState.color;
    //         parentObj.GetComponent<TabButtonState>().selectedState.sprite = parentObj.GetComponent<ButtonState>().normalState.sprite;
    //         parentObj.GetComponent<TabButtonState>().pressedState.sprite = parentObj.GetComponent<ButtonState>().pressedState.sprite;
    //         parentObj.GetComponent<TabButtonState>().normalState.sprite = parentObj.GetComponent<ButtonState>().disabledState.sprite;

    //         Destroy(parentObj.GetComponent<ButtonState>());
    //     }
    // }
#endif
}
