using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObjectSize : MonoBehaviour
{
    [Header("GizmosColor")]
    [SerializeField] private Color _gizmosColor = Color.green;

    [Header("Size")]
    [SerializeField] private Vector3 _offset = Vector3.zero;
    [SerializeField] private Vector3 _size = Vector3.zero;

    public Vector3 Pos { get { return this.transform.position + _offset; } }
    public Vector3 Offset { get { return _offset; } }
    public Vector3 Size { get { return _size; } }
    public Rect Rect { get { return new Rect(Pos, Size); } }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = _gizmosColor;
        Gizmos.DrawWireCube(Pos, Size);
    }
}
