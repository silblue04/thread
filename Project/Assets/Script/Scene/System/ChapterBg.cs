using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public class ChapterBg : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject _chapterBgPrefabRoot;
    private GameObject _curSetChapterBgInstance = null;


    public void Init()
    {

    }
    public void Release()
    {
        _RemoveChapterBgInstance();
    }


    public void ChangeChapterBg(Action callback = null)
    {
        _StartCoChangeChapterBg(callback);
    }

    private IEnumerator _coChangeChapterBg    = null;
    protected virtual void _StartCoChangeChapterBg(Action callback = null)
    {
        _StopCoChangeChapterBg();

        _coChangeChapterBg = _CoChangeChapterBg(callback);
        StartCoroutine(_coChangeChapterBg);
    }
    protected virtual void _StopCoChangeChapterBg()
    {
        if(_coChangeChapterBg == null)
        {
            return;
        }

        StopCoroutine(_coChangeChapterBg);
        _coChangeChapterBg = null;
    }
    private IEnumerator _CoChangeChapterBg(Action callback = null)
    {
        _RemoveChapterBgInstance();
        yield return YieldInstructionCache.WaitForEndOfFrame;

        _CreateChapterBgInstance();
        yield return YieldInstructionCache.WaitForEndOfFrame;

        callback?.Invoke();
    }

    private void _CreateChapterBgInstance()
    {
        var chaptrtData = LocalInfoConnecter.Instance.LocalBattleInfo.ChapterData;

        GameObject prefab = Resources.Load<GameObject>(chaptrtData.prefab_name);
        _curSetChapterBgInstance = Util.Instantiate(_chapterBgPrefabRoot.transform, prefab);
    }
    private void _RemoveChapterBgInstance()
    {
        if(_curSetChapterBgInstance != null)
        {
            Util.DestroyChild(_chapterBgPrefabRoot.transform);
        }
        _curSetChapterBgInstance = null;
    }
}
