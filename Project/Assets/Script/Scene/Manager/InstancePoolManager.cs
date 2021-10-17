

public class InstancePoolManager : Singleton<InstancePoolManager>
{
    private ObjectBasePoolManager<ObjectBase> _instancePool;


    public void Init()
    {
        _instancePool = new ObjectBasePoolManager<ObjectBase>();
        _instancePool.Init(this.transform);
    }

    public ObjectBasePool BaseRegistPool(ObjectBase prefab, int defaultPoolSize = 5, int increasementSize = 5)
    {
        return _instancePool.BaseRegistPool(prefab, defaultPoolSize, increasementSize);
    }

    public ObjectBasePool BaseRegistPool(int instanceKey, ObjectBase instancePrefab, int defaultPoolSize = 5, int increasementSize = 5)
    {
        return _instancePool.BaseRegistPool(instanceKey, instancePrefab, defaultPoolSize, increasementSize);
    }

    public void RegistPool(ObjectBase prefab, int defaultPoolSize = 5, bool increasementable = true, int increasementSize = 5)
    {
        _instancePool.RegistPool(prefab, defaultPoolSize, increasementable, increasementSize);
    }

    public void RegistPool(int instanceKey, ObjectBase instancePrefab, int defaultPoolSize = 5, bool increasementable = true, int increasementSize = 5)
    {
        _instancePool.RegistPool(instanceKey, instancePrefab, defaultPoolSize, increasementable, increasementSize);
    }

    public bool IsRegistered(int instanceKey)
    {
        return _instancePool.IsRegistered(instanceKey);
    }

    public bool IsRegistered(ObjectBase prefab)
    {
        return _instancePool.IsRegistered(prefab);
    }

    public ObjectBasePool GetRegisteredPool(int instanceKey)
    {
        return _instancePool.GetRegisteredPool(instanceKey);
    }

    public ObjectBasePool GetRegisteredPool(ObjectBase prefab)
    {
        return _instancePool.GetRegisteredPool(prefab);
    }

    public void RemovePool(int instanceKey)
    {
        _instancePool.RemovePool(instanceKey);
    }

    public void RemovePool(ObjectBase prefab)
    {
        _instancePool.RemovePool(prefab);
    }

    public void RemovePool(ObjectBasePool poolInstance)
    {
        _instancePool.RemovePool(poolInstance);
    }

    public T Spawn<T>(int instanceKey) where T : ObjectBase
    {
        return _instancePool.Spawn<T>(instanceKey);
    }

    public T Spawn<T>(T prefab) where T : ObjectBase
    {
        return _instancePool.Spawn<T>(prefab);
    }

    public void RemoveAll()
    {
        _instancePool.RemoveAll();
    }

    public void DespawnAll<T>(T prefab) where T : ObjectBase
    {
        _instancePool.DespawnAll(prefab);
    }

    public void DespawnAll()
    {
        _instancePool.DespawnAll();
    }
}

