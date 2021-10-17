using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;


public class VirtualPad
    : MonoBehaviour
    , IPointerDownHandler, IPointerUpHandler
    , IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private MoveController _moveController;

    private Vector2 _initialJoysticPosition = Vector2.zero;
    

    public void OnEnable()
    {
        _initialJoysticPosition = _moveController.transform.position;
    }


    public virtual void OnPointerDown(PointerEventData ped)
    {
        _moveController.transform.position = ped.position;
    }
    public virtual void OnPointerUp(PointerEventData ped)
    {
        _moveController.transform.position = _initialJoysticPosition;
    }


    public void OnBeginDrag(PointerEventData ped)
    {
        _moveController.OnBeginDrag(ped);
    }
    public void OnDrag(PointerEventData ped)
    {
        _moveController.OnDrag(ped);
    }
    public void OnEndDrag(PointerEventData ped)
    {
        _moveController.OnEndDrag(ped);
    }


    public void EndController()
    {
        _moveController.EndController();
    }
}
