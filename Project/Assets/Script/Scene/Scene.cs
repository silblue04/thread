using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scene : MonoBehaviour
{
    [Header("-- Base Information ------")]
    [SerializeField] protected SceneType _type = SceneType.NONE;
    public SceneType Type { get { return _type;} }
    public string SceneName { get { return DefsScene.GetSceneName(_type); } }

    protected virtual void Start()
    {
        _OnSceneStart();
    }

    protected virtual void OnDestroy()
    {
        _OnSceneEnd();
    }

    protected virtual void _OnSceneStart()
    {
        _RegistCurrentScene();
    }

    protected virtual void _OnSceneEnd()
    {

    }

    protected void _RegistCurrentScene()
    {
        SceneManager.Instance.SetCurScene(this);
    }
}
