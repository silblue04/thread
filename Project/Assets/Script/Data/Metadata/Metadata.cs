using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;


public class Metadata
{
    [JsonIgnore] static public DefineContainer Define = new DefineContainer();

    [JsonIgnore] static public ChapterContainer ChapterContainer = new ChapterContainer();
    [JsonIgnore] static public StageContainer StageContainer = new StageContainer();

    [JsonIgnore] static public AttackerContainer AttackerContainer = new AttackerContainer();

    [JsonIgnore] static public ItemContainer ItemContainer = new ItemContainer();


    [JsonIgnore] static public string[] Languages = new string[] { "English", "Korean", "ChineseSimplified", "ChineseTraditional", /*, "Japanese"*/ };
    

    static public void GetCommonDataParsingListQueue(ref Queue<Action> parsingListQueue)
    {
        parsingListQueue = new Queue<Action>();
        parsingListQueue.Enqueue(() => { Define.Parsing(CSVReader.Read("Define")); Debug.Log("[Complete] Parsing Define"); });
        parsingListQueue.Enqueue(() => { _InitDefineDatas(); Debug.Log("[Complete] _InitDefineDatas"); });
    
        parsingListQueue.Enqueue(() => { ChapterContainer.Parsing(CSVReader.Read("Chapter")); Debug.Log("[Complete] Parsing Chapter");});
        parsingListQueue.Enqueue(() => { StageContainer.Parsing(CSVReader.Read("Stage")); Debug.Log("[Complete] Parsing Stage");});

        parsingListQueue.Enqueue(() => { AttackerContainer.Parsing(CSVReader.Read("Attacker")); Debug.Log("[Complete] Parsing Attacker");});

        parsingListQueue.Enqueue(() => { ItemContainer.Parsing(CSVReader.Read("Item")); Debug.Log("[Complete] Parsing Item");});
    }

    static private void _InitDefineDatas()
    {
        DefsMetadata.InitDefineDatas();
        DefsItem.InitDefineDatas();
    }

    static public void ClearDatas()
    {
        Define.ClearDatas();

        ChapterContainer.ClearDatas();
        StageContainer.ClearDatas();

        ItemContainer.ClearDatas();

        AttackerContainer.ClearDatas();
    }
}
