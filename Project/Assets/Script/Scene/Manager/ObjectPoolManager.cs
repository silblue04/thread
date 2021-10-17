using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EffectType
{
    MAX
}
// 아래 순으로 뎁스값이 _depthInterval 만큼 더해짐

public enum ObjectType
{
    Essence,

    MAX
}



public class ObjectPoolManager : Singleton<ObjectPoolManager>
{
    [Header("Effect Prefab")]
    [SerializeField] private EffectBase[] _effectPrefabList = new EffectBase[BitConvert.Enum32ToInt(EffectType.MAX)];
    [SerializeField] private float _depthInterval = -0.01f;

    [Header("Object Prefab")]
    [SerializeField] private ObjectBase[] _objectPrefabList = new EffectBase[BitConvert.Enum32ToInt(ObjectType.MAX)];

    [Header("Root")]
    [SerializeField] private Transform _effectRoot;
    [SerializeField] private Transform _objectRoot;

    private bool _onEffect = true;


    public void Init()
    {
        InstancePoolManager.Instance.Init();
        EffectManager.Instance.Init();

        _InitOption();
        _InitRegistPool();
    }
    public void Release()
    {
        EffectManager.DestroyInstance();
        InstancePoolManager.Instance.DespawnAll();
        InstancePoolManager.Instance.RemoveAll();
        InstancePoolManager.DestroyInstance();
    }

    private void _InitOption()
    {
        LocalOptionInfo LocalOptionInfo = LocalInfoConnecter.Instance.LocalOptionInfo;

        OptionState optionState = LocalOptionInfo.GetOptionState(OptionType.Effect);
        OnEffect((optionState == OptionState.On));
    }
    private void _InitRegistPool()
    {
        foreach(var effectBase in _effectPrefabList)
        {
            InstancePoolManager.Instance.RegistPool(effectBase);
        }
        foreach(var objectBase in _objectPrefabList)
        {
            InstancePoolManager.Instance.RegistPool(objectBase);
        }
    }
    

    public void OnEffect(bool on)
    {
        _onEffect = on;
    }


    public EffectBase CreateEffect(EffectType type)
    {
        return CreateEffect(type, Vector2.zero);
    }
    public EffectBase CreateEffect(EffectType type, Vector2 pos)
    {
        if(_onEffect == false)
        {
            return null;
        }

        int typeIndex = BitConvert.Enum32ToInt(type);
        EffectBase instance = EffectManager.Instance.PlayParticle
        (
            _effectPrefabList[typeIndex]
            , _effectRoot
            , new Vector3(pos.x, pos.y, typeIndex * _depthInterval)
        );
        return instance;
    }
    public T CreateEffect<T>(EffectType type, Vector2 pos) where T : EffectBase
    {
        if(_onEffect == false)
        {
            return null;
        }

        T instance = CreateEffect(type, pos) as T;
        return instance;
    }

    public EffectBase CreateEffect(EffectType type, Vector2 pos, Vector3 scale)
    {
        if(_onEffect == false)
        {
            return null;
        }

        EffectBase instance = CreateEffect(type, pos);
        if(instance == null)
        {
            return null;
        }
        
        instance.transform.localScale = scale;
        return instance;
    }


    public T CreateObject<T>(ObjectType type) where T : ObjectBase
    {
        T instance = CreateObject(type) as T;
        return instance;
    }
    public ObjectBase CreateObject(ObjectType type)
    {
        int typeIndex = BitConvert.Enum32ToInt(type);

        ObjectBase instance = InstancePoolManager.Instance.Spawn(_objectPrefabList[typeIndex]);
        instance.transform.SetParent(_objectRoot);
        Util.InitLocalTransform(instance.transform);

        return instance;
    }


    public void DespawnAll(EffectType type)
    {
        InstancePoolManager.Instance.DespawnAll(_effectPrefabList[BitConvert.Enum32ToInt(type)]);
    }
    public void DespawnAll(ObjectType type)
    {
        InstancePoolManager.Instance.DespawnAll(_objectPrefabList[BitConvert.Enum32ToInt(type)]);
    }
}
