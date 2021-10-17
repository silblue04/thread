using System;
using System.Collections;
using System.Collections.Generic;


public class DualKeyDictionary<TK1, TK2, TValue> : IDictionary
{
    Dictionary<TK1, Dictionary<TK2, TValue>> mData = new Dictionary<TK1, Dictionary<TK2, TValue>>();

    public ICollection Keys
    {
        get
        {
            return mData.Keys;
        }
    }

    public ICollection InnerKeys
    {
        get
        {
            List<TK2> list = new List<TK2>();
            foreach (var pair in mData)
            {
                list.AddRange(pair.Value.Keys);
            }
            return list;
        }
    }

    public ICollection Values
    {
        get
        {
            List<TValue> list = new List<TValue>();
            foreach (var pair in mData)
            {
                foreach (var innerPair in pair.Value)
                {
                    list.Add(innerPair.Value);
                }
            }
            return list;
        }
    }

    public int Count
    {
        get
        {
            return mData.Count;
        }
    }

    public void Add(TK1 key1, TK2 key2, TValue value)
    {
        if (key1 == null) throw new ArgumentNullException("key1");
        if (key2 == null) throw new ArgumentNullException("key2");
        if (value == null) throw new ArgumentNullException("value");

        this[key1, key2] = value;
    }

    public void Clear()
    {
        foreach (var pair in mData)
        {
            pair.Value.Clear();
        }
        mData.Clear();
    }

    public bool ContainsKey(TK1 key)
    {
        return mData.ContainsKey(key);
    }

    public bool ContainsKey(TK1 k1, TK2 k2)
    {
        if (mData.ContainsKey(k1) == false)
        {
            return false;
        }

        return mData[k1].ContainsKey(k2);
    }

    public IDictionaryEnumerator GetEnumerator()
    {
        return mData.GetEnumerator();
    }

    public void Remove(TK1 key)
    {
        if (mData.ContainsKey(key) == false)
        {
            return;
        }

        mData.Remove(key);
    }

    public IDictionary<TK2, TValue> this[TK1 key]
    {
        get
        {
            return mData[key];
        }
    }

    public TValue this[TK1 key1, TK2 key2]
    {
        get
        {
            if (mData.ContainsKey(key1))
            {
                if (mData[key1].ContainsKey(key2))
                {
                    return mData[key1][key2];
                }
                else
                {
                    return default;
                }
            }
            else
            {
                return default;
            }
        }

        set
        {
            if (mData.ContainsKey(key1))
            {
                mData[key1].Add(key2, value);
            }
            else
            {
                var innerData = new Dictionary<TK2, TValue>();
                innerData.Add(key2, value);

                mData.Add(key1, innerData);
            }
        }
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    [Obsolete]
    public void Add(object key, object value)
    {
        throw new NotImplementedException();
    }

    [Obsolete]
    public bool Contains(object key)
    {
        throw new NotImplementedException();
    }

    [Obsolete]
    public void Remove(object key)
    {
        throw new NotImplementedException();
    }

    [Obsolete]
    public void CopyTo(Array array, int index)
    {
        throw new NotImplementedException();
    }

    [Obsolete]
    public bool IsFixedSize => throw new NotImplementedException();

    [Obsolete]
    public bool IsReadOnly => throw new NotImplementedException();

    [Obsolete]
    public bool IsSynchronized => throw new NotImplementedException();

    [Obsolete]
    public object SyncRoot => throw new NotImplementedException();

    [Obsolete]
    public object this[object key] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
}