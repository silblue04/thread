using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class PopupLanguage : UIBase
{
    [Header("TabButtonGroup")]
    [SerializeField] private TabButtonGroup _tabButtonGroup;

    [Header("Prefab")]
    [SerializeField] private TextObject _selectLanguageButtonPrefab;


    public override void OnOpenStart(Param param = null)
    {
        base.OnOpenStart(param);
        _Init();
    }

    private void _Init()
    {
        string[] languages = Metadata.Languages;
        for (int i = 0; i < languages.Length; ++i)
        {
            TextObject instance = Util.Instantiate<TextObject>(_tabButtonGroup.transform, _selectLanguageButtonPrefab);
            instance.SetFont(i);
            instance.SetText(Localization.Get(languages[i]));
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)_tabButtonGroup.transform);
        
        _selectLanguageButtonPrefab.transform.SetAsLastSibling();
        _selectLanguageButtonPrefab.gameObject.SetActive(false);

        _tabButtonGroup.Init(_OnClickSelectLanguageButton, false);
    }

    private void _OnClickSelectLanguageButton(int langegeIndex)
    {
        LocalOptionInfo LocalOptionInfo = LocalInfoConnecter.Instance.LocalOptionInfo;
        LocalOptionInfo.SetLanguage(langegeIndex);

        SceneManager.Instance.ChangeScene(SceneType.InGame);
    }
}