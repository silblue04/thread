using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using System.Linq;
using System.Numerics;


public class LocalInventoryInfoContainer : LocalInfoContainer
{
    protected override void _InitFileName()
    {
        LOCAL_INFO_FILE_NAME = "LocalInventoryInfo";
    }
    protected override void _InitContentsInfo()
    {
        
    }
    public override void ResetContents()
    {
        
    }

    protected override void _Copy(string jsonFile)
    {
        LocalInventoryInfoContainer info = JsonConvert.DeserializeObject<LocalInventoryInfoContainer>(jsonFile);
        if(info == null)
        {
            return;
        }


        base._CopyConnectTimeAndInitFirstConnectTime(info);
    }
}