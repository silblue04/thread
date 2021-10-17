using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

using System.Collections.Generic;


public class MoveController : MonoBehaviour
{
    private enum TouchType
    {
        Released = 0,
        Pressed,
    }

    [Header("TouchType")]
    [SerializeField] private GameObjectSelector _touchTypeSelector;

    [Header("Joystick")]
    [SerializeField] private float _joystickRange = 100f;
    [SerializeField] private RectTransform _joystickTransform;

    [Header("Swipe")]
    [SerializeField] private float _swipeThresholdSpeed = 200f;

    private bool _isStartDrag = false;
    private bool _isSwiping = false;
    private Vector2 _lastDrag = Vector2.zero;

    public void OnBeginDrag(PointerEventData ped)
    {
        _isSwiping = false;
        _isStartDrag = true;
        _lastDrag = ped.position;

        _joystickTransform.anchoredPosition = Vector2.zero;
        _touchTypeSelector.Select(TouchType.Pressed);
    }

    public void OnDrag(PointerEventData ped)
    {
        if (_isStartDrag == false)
        {
            return;
        }

        
        Vector2 dragVector = ped.position - ped.pressPosition;
        float dragDistance = dragVector.magnitude;
        Vector2 dragDir = dragVector.normalized;


        float dragAngle = Mathf.Atan2(dragDir.y, dragDir.x) * Mathf.Rad2Deg;
        _joystickTransform.transform.localRotation = Quaternion.AngleAxis(dragAngle, Vector3.forward);

        bool isInJoystickRange = (dragDistance < _joystickRange);
        if(isInJoystickRange)
        {
            _joystickTransform.localPosition = dragDir * dragDistance;
        }
        else
        {
            _joystickTransform.localPosition = dragDir * _joystickRange;
        }

        _joystickTransform.localPosition = new Vector2
        (
            _joystickTransform.localPosition.x
            , _joystickTransform.localPosition.y
        );
        

        InGameLevelManager.Instance.OnDrag(dragVector);


        bool isNeededSwipingState = (_swipeThresholdSpeed < Vector2.Distance(_lastDrag, ped.position));
        if(isNeededSwipingState)
        {
            _isSwiping = true;
        }

        _lastDrag = ped.position;
    }

    public void OnEndDrag(PointerEventData ped)
    {
        if(true == _isSwiping)
        {
            //IngameUI.Instance.OnSwipe(_lastDrag - ped.pressPosition);
        }

        EndController();
    }

    public void EndController()
    {
        _joystickTransform.anchoredPosition = Vector2.zero;
        _touchTypeSelector.Select(TouchType.Released);

        InGameLevelManager.Instance.OnEndDrag();

        _isSwiping = false;
        _isStartDrag = false;
        _lastDrag = Vector2.zero;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(this.transform.position, _joystickRange);
    }
}
