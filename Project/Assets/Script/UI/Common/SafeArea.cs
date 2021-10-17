using UnityEngine;

public class SafeArea : MonoBehaviour
{
    private RectTransform _rectTrans;
    //private Rect _lastSafeArea = new Rect(0, 0, 0, 0);

    private void Awake()
    {
        _rectTrans = GetComponent<RectTransform>();
    }

    private void Start()
    {
        _ApplySafeArea();
    }

    private void _ApplySafeArea()
    {
        Rect safeArea = Screen.safeArea;
        Vector2 minAnchor = safeArea.position;
        Vector2 maxAnchor = minAnchor + safeArea.size;

        minAnchor.x /= Screen.width;
        minAnchor.y /= Screen.height;
        maxAnchor.x /= Screen.width;
        maxAnchor.y /= Screen.height;

        _rectTrans.anchorMin = minAnchor;
        _rectTrans.anchorMax = maxAnchor;
    }
}