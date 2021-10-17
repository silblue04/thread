using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameObjectSelector : MonoBehaviour
{
    private int _prevSelectedIndex = DefsDefault.VALUE_NONE;
    private int _curSelectedIndex = DefsDefault.VALUE_NONE;
    private int _CurSelectedIndex
    {
        get { return _curSelectedIndex; }
        set
        {
            _prevSelectedIndex = _curSelectedIndex;
            _curSelectedIndex = value;
        }
    }
    
    public int PrevSelectedIndex { get { return _prevSelectedIndex; } }
    public int CurSelectedIndex { get { return _CurSelectedIndex; } }

    public int ChildCount { get { return this.transform.childCount; } }
    public int LastIndex { get { return this.transform.childCount - 1; } }

    private GameObject _lastSelectedObject = null;


    public GameObject Select(bool selectedBoolen)
    {
        int selectIndex = selectedBoolen ? DefsDefault.VALUE_ONE : DefsDefault.VALUE_ZERO;
        return Select(selectIndex);
    }

    public GameObject Select<T>(T stateEnum)  where T : struct
    {
        int selectIndex = BitConvert.Enum32ToInt(stateEnum);
        return Select(selectIndex);
    }

    public GameObject Select(int selectIndex)
    {
        if (_CurSelectedIndex == selectIndex)
        {
            return _lastSelectedObject;
        }

        for ( int i = 0; i < this.transform.childCount; ++i )
        {
            if ( i == selectIndex )
            {
                _lastSelectedObject = this.transform.GetChild( i ).gameObject;
                _lastSelectedObject.SetActive( true );
                _CurSelectedIndex = i;
            }
            else
            {
                this.transform.GetChild( i ).gameObject.SetActive( false );
            }
        }

        return _lastSelectedObject;
    }

    public void SelectOverlap(int selectIndex)
    {
        for ( int i = 0; i < this.transform.childCount; ++i )
        {
            if ( i == selectIndex )
            {
                GameObject selectedObject = this.transform.GetChild( i ).gameObject;
                selectedObject.SetActive( true );
                break;
            }
        }
    }

    public void SelectOverlap(List<int> selectIndex)
    {
        if(selectIndex == null || selectIndex.Count == 0)
        {
            return;
        }


        List<int> constSelectIndex = selectIndex;

        for ( int i = 0; i < this.transform.childCount; ++i )
        {
            bool isFound = false;
            for(int j = 0; j < constSelectIndex.Count; ++j)
            {
                if (i == constSelectIndex[j])
                {
                    GameObject selectedObject = this.transform.GetChild(i).gameObject;
                    selectedObject.SetActive(true);

                    constSelectIndex.RemoveAt(j);
                    isFound = true;
                    break;
                }
            }
            
            if(isFound == false)
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
            }
        }
    }

    public void Deselect(int deselectIndex)
    {
        for ( int i = 0; i < this.transform.childCount; ++i )
        {
            if ( i == deselectIndex )
            {
                this.transform.GetChild(i).gameObject.SetActive(false);
                break;
            }
        }

        if(_CurSelectedIndex == deselectIndex)
        {
            _CurSelectedIndex = DefsDefault.VALUE_NONE;
            _lastSelectedObject = null;
        }
    }

    public void Deselect()
    {
        for (int i = 0; i < this.transform.childCount; ++i)
        {
            this.transform.GetChild(i).gameObject.SetActive(false);
        }

        _CurSelectedIndex = DefsDefault.VALUE_NONE;
        _lastSelectedObject = null;
    }

    public T Select<T>( int selectIndex ) where T : Component
    {
        var selectObject = Select( selectIndex );

        if ( null == selectObject )
            return null;
        else
        {
            return selectObject.GetComponent<T>( );
        }
    }

    public GameObject Get( int getIndex )
    {
        for ( int i = 0; i < this.transform.childCount; ++i )
        {
            if ( i == getIndex )
            {
                return this.transform.GetChild( i ).gameObject;
            }
        }

        return null;
    }

    public T Get<T>( int getIndex ) where T : Component
    {
        var getObject = Get( getIndex );

        if ( null == getObject )
            return null;
        else
        {
            return getObject.GetComponent<T>( );
        }
    }
}
