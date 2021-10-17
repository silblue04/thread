using System.Collections.Generic;
using UnityEngine;

public class ObjectBasePoolManager<T> where T : ObjectBase
{
    protected Dictionary<int, ObjectBasePool> _poolDic = new Dictionary<int, ObjectBasePool>();

    private Transform _owner;

    public void Init(Transform owner)
    {
        _owner = owner;
    }

    public void RegistPool(T prefab, int defaultPoolSize = 5, bool increasementable = true, int increasementSize = 5)
    {
        RegistPool(prefab.GetInstanceID(), prefab, defaultPoolSize, increasementable, increasementSize);
    }

    public void RegistPool(int instanceKey, T instancePrefab, int defaultPoolSize = 5, bool increasementable = true, int increasementSize = 5)
    {
        if (_poolDic.ContainsKey(instanceKey) == false)
        {
            var poolInstance = ObjectBasePool.CreateInstancePool(_owner, instancePrefab, defaultPoolSize, true, defaultPoolSize / 2);
            _poolDic.Add(instanceKey, poolInstance);
        }
    }

    public ObjectBasePool BaseRegistPool(T prefab, int defaultPoolSize = 5, int increasementSize = 5)
    {
        return BaseRegistPool(prefab.GetInstanceID(), prefab, defaultPoolSize, increasementSize);
    }

    public ObjectBasePool BaseRegistPool(int instanceKey, T instancePrefab, int defaultPoolSize = 5, int increasementSize = 5)
    {
        if (_poolDic.ContainsKey(instanceKey) == false)
        {
            var poolInstance = ObjectBasePool.CreateInstancePool(_owner, instancePrefab, defaultPoolSize, true, increasementSize);
            _poolDic.Add(instanceKey, poolInstance);
        }

        return _poolDic[instanceKey];
    }

    public bool IsRegistered(int instanceKey)
    {
        return _poolDic.ContainsKey(instanceKey);
    }

    public bool IsRegistered(T prefab)
    {
        return _poolDic.ContainsKey(prefab.GetInstanceID());
    }

    public ObjectBasePool GetRegisteredPool(int instanceKey)
    {
        if (_poolDic.ContainsKey(instanceKey) == false)
        {
            return null;
        }

        return _poolDic[instanceKey];
    }

    public ObjectBasePool GetRegisteredPool(T prefab)
    {
        if (_poolDic.ContainsKey(prefab.GetInstanceID()) == false)
        {
            return null;
        }

        return _poolDic[prefab.GetInstanceID()];
    }

    public void RemovePool(int instanceKey)
    {
        if (_poolDic.ContainsKey(instanceKey))
        {
            _poolDic[instanceKey].RemovePool();
            _poolDic.Remove(instanceKey);
        }
    }

    public void RemovePool(T prefab)
    {
        if (_poolDic.ContainsKey(prefab.GetInstanceID()))
        {
            _poolDic[prefab.GetInstanceID()].RemovePool();
            _poolDic.Remove(prefab.GetInstanceID());
        }
    }

    public void RemovePool(ObjectBasePool poolInstance)
    {
        foreach (var pool in _poolDic)
        {
            if (pool.Value == poolInstance)
            {
                pool.Value.RemovePool();
                _poolDic.Remove(pool.Key);
                break;
            }
        }
    }

    public U Spawn<U>(int instanceKey) where U : ObjectBase
    {
        if (_poolDic.ContainsKey(instanceKey) == false)
        {
            Debug.LogErrorFormat("Pool에 등록 안된 Prefab 입니다. key : {0}, name : {1}", instanceKey, typeof(U));
            return null;
        }

        return _poolDic[instanceKey].Spawn() as U;
    }

    public U Spawn<U>(T prefab) where U : ObjectBase
    {
        // return Spawn<U>(prefab.GetInstanceID());
        if ( _poolDic.ContainsKey( prefab.GetInstanceID( ) ) == false )
        {
           Debug.LogErrorFormat( "Pool에 등록 안된 Prefab 입니다. key : {0}, name : {1}", prefab.GetInstanceID( ), prefab.name );
           return null;
        }

        return _poolDic[prefab.GetInstanceID( )].Spawn( ) as U;
    }

    public void DespawnAll(T prefab)
    {
        if ( _poolDic.ContainsKey( prefab.GetInstanceID( ) ) == false )
        {
           Debug.LogErrorFormat( "Pool에 등록 안된 Prefab 입니다. key : {0}, name : {1}", prefab.GetInstanceID( ), prefab.name );
        }

        _poolDic[prefab.GetInstanceID( )].DespawnAll();
    }

    public void DespawnAll()
    {
        foreach (var prefabPool in _poolDic)
        {
            prefabPool.Value.DespawnAll();
        }
    }

    public void Clear()
    {
        foreach (var prefabPool in _poolDic)
        {
            prefabPool.Value.RemovePool();
        }
    }

    public void RemoveAll()
    {
        Clear();
        _poolDic.Clear();
    }
}