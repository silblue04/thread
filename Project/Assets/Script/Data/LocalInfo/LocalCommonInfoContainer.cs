using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using System.Linq;
using System.Numerics;


public class LocalCommonInfoContainer : LocalInfoContainer
{
    [JsonProperty] public LocalOptionInfo LocalOptionInfo   = new LocalOptionInfo();

    
    protected override void _InitFileName()
    {
        LOCAL_INFO_FILE_NAME = "LocalCommonInfo";
    }
    protected override void _InitContentsInfo()
    {
        LocalOptionInfo.Init();
    }

    public override void ResetContents()
    {
        
    }


    protected override void _Copy(string jsonFile)
    {
        LocalCommonInfoContainer info = JsonConvert.DeserializeObject<LocalCommonInfoContainer>(jsonFile);
        if(info == null)
        {
            return;
        }
        
        this.LocalOptionInfo = info.LocalOptionInfo;

        base._CopyConnectTimeAndInitFirstConnectTime(info);
    }
}