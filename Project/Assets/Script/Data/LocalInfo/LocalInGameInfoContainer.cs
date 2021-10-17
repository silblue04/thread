using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using System.Linq;
using System.Numerics;


public class LocalInGameInfoContainer : LocalInfoContainer
{
    [JsonProperty] public LocalBattleInfo LocalBattleInfo   = new LocalBattleInfo();


    protected override void _InitFileName()
    {
        LOCAL_INFO_FILE_NAME = "LocalInGameInfo";
    }
    protected override void _InitContentsInfo()
    {

    }
    public override void ResetContents()
    {

    }

    protected override void _Copy(string jsonFile)
    {
        LocalInGameInfoContainer info = JsonConvert.DeserializeObject<LocalInGameInfoContainer>(jsonFile);
        if(info == null)
        {
            return;
        }

        this.LocalBattleInfo = info.LocalBattleInfo;

        base._CopyConnectTimeAndInitFirstConnectTime(info);
    }
}