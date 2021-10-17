using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using System.Linq;


public class LocalInfoConnecter : Singleton<LocalInfoConnecter>
{
    public LocalCommonInfoContainer LocalCommonInfoContainer = new LocalCommonInfoContainer();
    public LocalInventoryInfoContainer LocalInventoryInfoContainer = new LocalInventoryInfoContainer();
    public LocalInGameInfoContainer LocalInGameInfoContainer = new LocalInGameInfoContainer();


    public void Init()
    {
        LocalCommonInfoContainer.Init();
        LocalInventoryInfoContainer.Init();
        LocalInGameInfoContainer.Init();
    }

    public void SaveAllLocalInfo()
    {
        LocalCommonInfoContainer.Save();
        LocalInventoryInfoContainer.Save();
        LocalInGameInfoContainer.Save();
    }


    public LocalOptionInfo LocalOptionInfo          { get { return LocalCommonInfoContainer.LocalOptionInfo; } }

    public LocalBattleInfo LocalBattleInfo          { get { return LocalInGameInfoContainer.LocalBattleInfo; } }
}