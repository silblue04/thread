using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DefsString 
{
    public static class Sound
    {
        public const string MASTER_VOLUME        = "MasterVolume";
        public const string BGM_VOLUME           = "BGMVolume";
        public const string SFX_VOLUME           = "SFXVolume";
    }

    public static class Material
    {
        public const string GRAY_SCALE     = "GrayScale";
    }

    public static class Spine
    {
        public const string DEFAULT_SKIN_NAME     = "default";
        public const string ANIMATION_KEY_HIT     = "hit";
    }

    public static class TextFormat
    {
        public const string DEFAULT_TEXT_FORMAT     = "{0}";
        public const string PLUS_VALUE_TEXT_FORMAT   = "+{0}";
        public const string MINUS_VALUE_TEXT_FORMAT   = "-{0}";
        public const string MULTI_VALUE_TEXT_FORMAT = "x{0}";
        public const string COLON_VALUE_TEXT_FORMAT = ": {0}";
        public const string CUR_VALUE_SLASH_MAX_VALUE_TEXT_FORMAT = "{0}/{1}";
        public const string CUR_VALUE_COLON_SLASH_MAX_VALUE_TEXT_FORMAT = ": {0}/{1}";
        public const string SLASH_MAX_VALUE_TEXT_FORMAT     = "/{0}";
        public const string PRECENTAGE_VALUE_TEXT_FORMAT    = "{0}%";

        public const string PREVIEW_ITEM_NUM_FORMAT     = "{0} ({1})";

        public const string TIME_STRING_FORMAT         = "{0}{1} {2}{3}";
        public const string TIME_STRING_SHORT_FORMAT   = "{0}{1}";
    }

    static public string[] GradeString { get; private set; }


    static public void InitDefineDatas()
    {
        var enumStringList = System.Enum.GetNames(typeof(Grade));
        GradeString = new string[enumStringList.Length];
        for(int i = 0; i < enumStringList.Length; ++i)
        {
            GradeString[i] = enumStringList[i];
        }
    }
}