using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;
using TMPro;

public class PopupOption : UIBase
{
    [Header("Part_Language")]
    [SerializeField] private TextObject _popupLanguageButton;

    [Header("Part_OptionButton")]
    [SerializeField] private OptionButton[] _optionButtonList = new OptionButton[BitConvert.Enum32ToInt(OptionType.MAX)];
    [SerializeField] private Button _restoreBtn = null;


    public override void OnOpenStart(Param param = null)
    {
        base.OnOpenStart(param);
        _Init();
    }

    private void _Init()
    {
        _popupLanguageButton.SetText(Localization.Get(Localization.Language));

        foreach(var optionButton in _optionButtonList)
        {
            optionButton.Init(_CallbackOnClickButton);
        }
    }

    public void ShopPopupLanguageButton()
    {
        UIManager.Instance.Show<PopupLanguage>(DefsUI.Prefab.POPUP_LANGUAGE);
    }

    private void _CallbackOnClickButton(OptionType type, OptionState state)
    {
        LocalOptionInfo LocalOptionInfo = LocalInfoConnecter.Instance.LocalOptionInfo;
        LocalOptionInfo.SetOptionState(type,state);

        switch(type)
        {
            case OptionType.BGM:
                {
                    SoundManager.Instance.OnBgm((state == OptionState.On));
                }
                break;

            case OptionType.SFX:
                {
                    SoundManager.Instance.OnSound((state == OptionState.On));
                }
                break;

            case OptionType.Effect:
                {
                    ObjectPoolManager.Instance.OnEffect((state == OptionState.On));
                }
                break;
        }
    }
}