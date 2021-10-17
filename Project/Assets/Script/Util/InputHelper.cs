using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.EventSystems;


public class InputHelper : MonoBehaviour
{
    private static TouchCreator lastFakeTouch;
// #if UNITY_EDITOR
//     private static int _pointerId = -1;
// #else
//     private static int _pointerId = 0;
// #endif

    public static List<Touch> GetTouches()
    {
        List<Touch> touches = new List<Touch>();

#if UNITY_EDITOR
        if(EventSystem.current.IsPointerOverGameObject())//(_pointerId))
        {
            return touches;
        }
        
        for (int i = 0; i < Input.touches.Length; ++i)
        {
            touches.Add(Input.touches[i]);
        }
#else
        // if(Input.touchCount > 0)
        // {
        //     if(EventSystem.current.IsPointerOverGameObject(Input.GetTouch(0).fingerId))//(_pointerId))
        //     {
        //         return touches;
        //     }
        // }

        for (int i = 0; i < Input.touches.Length; ++i)
        {
            if(IsPointerOverUIObject(Input.touches[i].position))
            {
                continue;
            }

            touches.Add(Input.touches[i]);
        }
#endif

        // for (int i = 0; i < Input.touches.Length; ++i)
        // {
        //     touches.Add(Input.touches[i]);
        // }

#if UNITY_EDITOR
        if (lastFakeTouch == null) lastFakeTouch = new TouchCreator();
        if (Input.GetMouseButtonDown(0))
        {
            lastFakeTouch.phase = TouchPhase.Began;
            lastFakeTouch.deltaPosition = new Vector2(0, 0);
            lastFakeTouch.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            lastFakeTouch.fingerId = 0;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            lastFakeTouch.phase = TouchPhase.Ended;
            Vector2 newPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            lastFakeTouch.deltaPosition = newPosition - lastFakeTouch.position;
            lastFakeTouch.position = newPosition;
            lastFakeTouch.fingerId = 0;
        }
        else if (Input.GetMouseButton(0))
        {
            lastFakeTouch.phase = TouchPhase.Moved;
            Vector2 newPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            lastFakeTouch.deltaPosition = newPosition - lastFakeTouch.position;
            lastFakeTouch.position = newPosition;
            lastFakeTouch.fingerId = 0;
        }
        else
        {
            lastFakeTouch = null;
        }
        if (lastFakeTouch != null) touches.Add(lastFakeTouch.Create());
#endif


        return touches;
    }

    public static bool IsPointerOverUIObject(Vector2 touchPos)
    {
        PointerEventData eventDataCurrentPosition
            = new PointerEventData(EventSystem.current);

        eventDataCurrentPosition.position = touchPos;

        List<RaycastResult> results = new List<RaycastResult>();


        EventSystem.current
            .RaycastAll(eventDataCurrentPosition, results);

        return results.Count > 0;
    }

}

