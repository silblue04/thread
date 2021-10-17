using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class EffectManager : Singleton<EffectManager>
{
    public struct EffectAttachInfo
    {
        public EffectAttachInfo(Transform trans)
        {
            transform = trans;
            position = Vector3.zero;
            rotate = Vector3.zero;
        }

        Transform transform;
        Vector3 position;
        Vector3 rotate;
    }

    public void Init()
    {
        
    }

    public EffectBase PlayParticle(ObjectBase effect, Transform parent)
    {
        return PlayParticle(effect, parent, Vector3.zero, Quaternion.identity, false, 0.0f);
    }

    public EffectBase PlayParticle(ObjectBase effect, Transform parent, Vector3 position)
    {
        return PlayParticle(effect, parent, position, Quaternion.identity, false, 0.0f);
    }

    public EffectBase PlayParticle(ObjectBase effect, Transform parent, Vector3 position, Quaternion rotation)
    {
        return PlayParticle(effect, parent, position, rotation, false, 0.0f);
    }

    public EffectBase PlayParticle(ObjectBase effect, Transform parent, Vector3 position, Quaternion rotation, bool worldPositionStays, float customDuration = 0.0f)
    {
        if (effect == null)
            return null;

        ObjectBase pivotElement = null;
        EffectBase effectInstance = null;
        if (InstancePoolManager.Instance.IsRegistered(effect) == true)
        {
            // 풀생성해서 사용하는 이펙트!!
            ObjectBase poolElement = InstancePoolManager.Instance.Spawn(effect);
            effectInstance = poolElement.GetComponent<EffectBase>();

            // 부모가 있다는것은 postion이 로칼이라는것 이다.
            if (parent != null)
            {
                if (worldPositionStays == false)
                {
                    effectInstance.transform.SetParent(parent, false);
                    effectInstance.transform.localPosition = position;
                    effectInstance.transform.localRotation = rotation;
                }
                else
                {
                    effectInstance.transform.position = position;
                    effectInstance.transform.rotation = rotation;
                    effectInstance.transform.SetParent(parent, true);
                }
            }
            else
            {
                effectInstance.transform.position = position;
                effectInstance.transform.rotation = rotation;
            }

            if (false == effectInstance.isPermanent)
            {
                if (0.0f < customDuration)
                {
                    poolElement.Despawn(customDuration);
                    if (pivotElement != null)
                        pivotElement.Despawn(customDuration);
                }
                else
                {
                    poolElement.Despawn(effectInstance.duration);
                    if (pivotElement != null)
                        pivotElement.Despawn(effectInstance.duration);
                }
            }
        }
        else
        {
            GameObject clone = Instantiate(effect.gameObject);
            effectInstance = clone.GetComponent<EffectBase>();

            if (null == effectInstance)
            {
                Destroy(clone);
                return null;
            }

            if (parent != null)
            {
                if (worldPositionStays == false)
                {
                    effectInstance.transform.SetParent(parent, false);
                    effectInstance.transform.localPosition = position;
                    effectInstance.transform.localRotation = rotation;
                }
                else
                {
                    effectInstance.transform.position = position;
                    effectInstance.transform.rotation = rotation;
                    effectInstance.transform.SetParent(parent, true);
                }
            }
            else
            {
                effectInstance.transform.position = position;
                effectInstance.transform.rotation = rotation;
            }

            if (false == effectInstance.isPermanent)
            {
                if (0f < customDuration)
                {
                    Destroy(effectInstance, customDuration);
                    if (pivotElement != null)
                        pivotElement.Despawn(customDuration);
                }
                else
                {
                    Destroy(effectInstance, effectInstance.duration);
                    if (pivotElement != null)
                        pivotElement.Despawn(effectInstance.duration);
                }
            }
        }

        return effectInstance;
    }
}
