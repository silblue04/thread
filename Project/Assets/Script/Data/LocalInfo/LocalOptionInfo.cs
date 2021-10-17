using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using System.Linq;
using System.Numerics;


public class LocalOptionInfo
{
    [JsonProperty] private int _languageIndex = DefsDefault.VALUE_NONE;

    [JsonProperty] private List<OptionState> _optionStateList = new List<OptionState>();


    public void Init()
    {
        _InitOptionStateList();
        _InitLangauge();
    }

    private void _InitOptionStateList()
    {
        int MAX_OPTION_TPYE = BitConvert.Enum32ToInt(OptionType.MAX); 
        if(_optionStateList.Count < MAX_OPTION_TPYE)
        {
            for(int i = _optionStateList.Count; i < MAX_OPTION_TPYE; ++i)
            {
                _optionStateList.Add(OptionState.On);
            }
        }
    }

    private void _InitLangauge()
    {
        int languageIndex = _languageIndex;
        if (_languageIndex == DefsDefault.VALUE_NONE)
        {
            languageIndex = DefsDefault.VALUE_ZERO;

            SystemLanguage sl = Localization.GetSystemLanguage();
            string strSl = sl.ToString();
            
            for (int i = DefsDefault.VALUE_ONE; i < Metadata.Languages.Length; ++i)
            {
                if (strSl == Metadata.Languages[i])
                {
                    languageIndex = i;
                    break;
                }
            }
        }
        
        SetLanguage(languageIndex);
    }

    public void SetLanguage(int languageIndex)
    {
        _languageIndex = languageIndex;
        ResourceManager.Instance.ChangeFallbackFont(languageIndex);

        Localization.Language = Metadata.Languages[_languageIndex];
    }
    
#if UNITY_EDITOR
    public int test_GetLanguageIndex()
    {
        return _languageIndex;
    }
#endif

    public void SetOptionState(OptionType type, OptionState state)
    {
        int typeIndex = BitConvert.Enum32ToInt(type);
        _optionStateList[typeIndex] = state;
    }

    public OptionState GetOptionState(OptionType type)
    {
        int typeIndex = BitConvert.Enum32ToInt(type);
        return _optionStateList[typeIndex];
    }
}