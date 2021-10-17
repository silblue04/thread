using System.Collections.Generic;


public class DefsAds
{
    static public bool IsValiedMulitValue(int multi_value_ad)
    {
        return (multi_value_ad > DefsDefault.VALUE_ONE);
    }
}


public enum AdsType
{
    Fairy,
    FromFairyUI,
}