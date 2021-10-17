using UnityEngine;
using System.Numerics;


public enum LogRefType
{
    NONE = -1,

    EvolveMercenary = 0,
    Gacha,
    UpgradeTreasure,
    GetPrestige,
    Combinataion,
}

public enum LogAdRewardType
{
    Fariy = 0,
    ShopProduct,
    FreeFunc,
    GetPrestige,

    FairyUI,

    Gacha = 5,
    Offline,

    Bomb,
    SpeedBattle,

    UpgradeTreasure,
}