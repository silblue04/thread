using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;


public static class DefsUI
{   
    public class Prefab
    {
        public const string INGAME_UI = "Prefabs/UI/Scene/InGameUI";
        public const string GAMEMODE_WEEK_UI = "Prefabs/UI/Scene/GameModeWeekUI";

        public const string POPUP_CONTENTS_GUIDE_PROJECTILE = "Prefabs/UI/Popup/PopupContentsGuideProjectile";
        public const string POPUP_CONTENTS_GUIDE_MERCENARY = "Prefabs/UI/Popup/PopupContentsGuideMercenary";
        public const string POPUP_CONTENTS_GUIDE_MERCENARY_BUFF = "Prefabs/UI/Popup/PopupContentsGuideMercenaryBuff";
        public const string POPUP_CONTENTS_GUIDE_TREASURE = "Prefabs/UI/Popup/PopupContentsGuideTreasure";

        public const string POPUP_BOTTOMTAB = "Prefabs/UI/Popup/PopupBottomTab";

        public const string POPUP_PRESET = "Prefabs/UI/Popup/PopupPreset";
        public const string POPUP_APPLY_EQUIPMENT = "Prefabs/UI/Popup/PopupApplyEquipment";
        public const string POPUP_EQUIPMENT_INFO = "Prefabs/UI/Popup/PopupEquipmentInfo";

        public const string POPUP_MERCENARY_EVOLUTION = "Prefabs/UI/Popup/PopupMercenaryEvolution";

        public const string POPUP_PRESTIGE = "Prefabs/UI/Popup/PopupPrestige";
        public const string POPUP_PRESTIGE_CALCULATATION = "Prefabs/UI/Popup/PopupPrestigeCalculation";
        public const string POPUP_PRESTIGE_SUCCESS = "Prefabs/UI/Popup/PopupPrestigeSuccess";

        public const string POPUP_QUEST = "Prefabs/UI/Popup/PopupQuest";
        public const string POPUP_MISSION = "Prefabs/UI/Popup/PopupMission";

        public const string POPUP_OPTION = "Prefabs/UI/Popup/PopupOption";
        public const string POPUP_LANGUAGE = "Prefabs/UI/Popup/PopupLanguage";

        public const string POPUP_GACHA = "Prefabs/UI/Popup/PopupGacha";
        public const string POPUP_GACHA_RESULT = "Prefabs/UI/Popup/PopupGachaResult";

        public const string POPUP_ADS = "Prefabs/UI/Popup/PopupAds";
        public const string POPUP_AD_FREE_FUNC = "Prefabs/UI/Popup/PopupAdFreeFunc";
        public const string POPUP_AD_REWARD_FREE_GEM = "Prefabs/UI/Popup/PopupAdRewardFreeGem";
        
        public const string POPUP_CLICK_INDUCTION = "Prefabs/UI/Popup/PopupClickInduction";
        public const string POPUP_SYSTEM_MESSAGE = "Prefabs/UI/Popup/PopupSystemMessage";

        public const string POPUP_REWARD = "Prefabs/UI/Popup/PopupReward";

        public const string POPUP_CONFIRM = "Prefabs/UI/Popup/PopupConfirm";
        public const string POPUP_YES_NO = "Prefabs/UI/Popup/PopupYesNo";
        public const string POPUP_HELP = "Prefabs/UI/Popup/PopupHelp";
        public const string POPUP_EXIT = "Prefabs/UI/Popup/PopupExit";

        public const string POPUP_INGAMEBOMB = "Prefabs/UI/Popup/PopupSuperBomb";
        public const string POPUP_PACKAGE = "Prefabs/UI/Popup/PopupGpecialOffer";

        public const string POPUP_HIGH_SCORE = "Prefabs/UI/Popup/PopupHighScore";
        public const string POPUP_NEED_GOLD = "Prefabs/UI/Popup/PopupGold";
		public const string POPUP_SPEED_BATTLE = "Prefabs/UI/Popup/PopupSpeedBattle";
	}

    public static class LocalizationKey
    {
        public const string LV = "lv";
        public const string MAX = "max";

        public const string GRADE = "grade";
        public const string LEVEL = "level";
        public const string EQUIPMENT_SET = "equipment_set";

        public const string TIMES = "times";
        public const string A_DAY = "a_day";

        public const string SYSTEM_MESSAGE_TO_BE_UPDATED = "system_message_to_be_updated";

        public const string SYSTEM_MESSAGE_ON_AD_FREE_FUNC = "system_message_on_ad_free_func";
        public const string SYSTEM_MESSAGE_OFF_AD_FREE_FUNC = "system_message_off_ad_free_func";

        public const string SYSTEM_MESSAGE_NOT_ENOUGH_BASIC_CURRNECY = "system_message_not_enough_basic_currnecy";
        public const string SYSTEM_MESSAGE_NOT_LOADED_REWARD_ADS = "system_message_not_loaded_reward_ads";
        
        public const string SYSTEM_MESSAGE_IAP_THERE_IS_NO_PRODUCT = "system_message_iap_there_is_no_product";
        public const string SYSTEM_MESSAGE_IAP_FAIL_PURCHASE_PRODUCT = "system_message_iap_fail_purchase_product";
        public const string SYSTEM_MESSAGE_IAP_FAIL_DUPLICATE_PRODUCT = "system_message_iap_fail_duplicate_product";

        public const string SYSTEM_MESSAGE_PRODUCT_SOLD_OUT = "system_message_product_sold_out";
        public const string SYSTEM_MESSAGE_PRODUCT_FAILED_TO_RECEIVE_PRICE_INFORMATION = "system_message_product_failed_to_receive_price_information";
        
        public const string BOMB_SYSTEM_MESSAGE_01 = "bomb_system_message_01";
        public const string BOMB_SYSTEM_MESSAGE_02 = "bomb_system_message_02";
        public const string BOMB_SYSTEM_MESSAGE_03 = "bomb_system_message_03";
        public const string BOMB_SYSTEM_MESSAGE_04 = "bomb_system_message_04";

        public const string HELP_AD_FREE_FUNC = "help_ad_free_func";
        public const string HELP_GACHA = "help_gacha";
        public const string HELP_AD_REWARD_FREEGEM = "help_ad_reward_freegem";
        public const string HELP_AD_REWARD_BOMB = "help_ad_reward_bomb";

        public const string POPUP_YESNO_MESSAGE_PARTY_PRESET_NOT_SAVED = "popup_yesno_message_party_preset_not_saved";

        public static string[] PROJECTILE_SPEED = { "show_speed_0", "show_speed_1", "show_speed_2", "show_speed_3", "show_speed_4" };
    
        public const string POPUP_REWARD_DESC = "popup_reward_desc";
    

        public const string TIME_DAY   = "time_day";
        public const string TIME_HOUR  = "time_hour";
        public const string TIME_MIN   = "time_min";
        public const string TIME_SEC   = "time_sec";
    }

    static public string GetLevelText(int level)
    {
        string fmt = Localization.Get(LocalizationKey.LV);
        return string.Format(fmt, level);
    }
}


public enum UIType
{
    HUD,
    Popup,
}

public class InstanceUIData
{
    public string prefabPath = string.Empty;
    public bool isForwardTopMenu = true;
    public bool isNeededSafeArea = true;

    public InstanceUIData(string prefabPath, bool isForwardTopMenu = true, bool isNeededSafeArea = true)
    {
        this.prefabPath = prefabPath;
        this.isForwardTopMenu = isForwardTopMenu;
        this.isNeededSafeArea = isNeededSafeArea;
    }
}

public class IntrinsicUIData
{
    public string name = string.Empty;
    public int id = DefsDefault.VALUE_NONE;
    public Param uiParam = null;

    public Action<UIBase> callBackOpenStart;
    public Action<UIBase> callBackOpenEnd;
    public Action<UIBase> callBackCloseStart;
    public Action<UIBase> callBackCloseEnd;

    public IntrinsicUIData(string name, Action<UIBase> callBackOpenStart = null, Action<UIBase> callBackOpenEnd = null, Action<UIBase> callBackCloseStart = null, Action<UIBase> callBackCloseEnd = null)
    {
        this.name = name;
        this.id = DefsDefault.VALUE_NONE;
        this.uiParam = null;
        this.callBackOpenStart = callBackOpenStart;
        this.callBackOpenEnd = callBackOpenEnd;
        this.callBackCloseStart = callBackCloseStart;
        this.callBackCloseEnd = callBackCloseEnd;
    }

    public IntrinsicUIData(string name, Param uiParam, Action<UIBase> callBackOpenStart = null, Action<UIBase> callBackOpenEnd = null, Action<UIBase> callBackCloseStart = null, Action<UIBase> callBackCloseEnd = null)
    {
        this.name = name;
        this.id = DefsDefault.VALUE_NONE;
        this.uiParam = uiParam;
        this.callBackOpenStart = callBackOpenStart;
        this.callBackOpenEnd = callBackOpenEnd;
        this.callBackCloseStart = callBackCloseStart;
        this.callBackCloseEnd = callBackCloseEnd;
    }

    public void Remove()
    {
        this.name = string.Empty;
        this.id = DefsDefault.VALUE_NONE;
        this.uiParam = null;
        this.callBackOpenStart = null;
        this.callBackOpenEnd = null;
        this.callBackCloseStart = null;
        this.callBackCloseEnd = null;
    }
}

public readonly struct SliderAnimationData
{
    public float min { get; }
    public float max { get; }

    public float startValue { get; }
    public float endValue { get; }

    public SliderAnimationData
    (
        float min, float max
        , float startValue, float endValue
    )
    {
        this.min = min;
        this.max = max;

        this.startValue = startValue;
        this.endValue = endValue;
    }
}