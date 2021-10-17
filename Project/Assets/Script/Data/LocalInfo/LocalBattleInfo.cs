using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using System.Linq;
using System.Numerics;

 
public class LocalBattleInfo
{
    [JsonProperty] public int stage_idx { get; private set; } = DefsDefault.VALUE_ZERO;
    [JsonIgnore] public int chapter_idx { get { return DefsInGame.ConvertStageIdxToChapterIdx(stage_idx); } }

    [JsonIgnore] private ChapterContainer.Data _chapterData = null;
    [JsonIgnore] public ChapterContainer.Data ChapterData
    {
        get
        {
            if(_chapterData == null)
            {
                _chapterData = Metadata.ChapterContainer.GetData(chapter_idx);
            }
            else if(_chapterData.idx != chapter_idx)
            {
                _chapterData = Metadata.ChapterContainer.GetData(chapter_idx);
            }
            return _chapterData;
        }
    }

    [JsonIgnore] private StageContainer.Data _stageData = null;
    [JsonIgnore] public StageContainer.Data StageData
    {
        get
        {
            if(_stageData == null)
            {
                _stageData = Metadata.StageContainer.GetData(stage_idx);
            }
            else if(_stageData.idx != stage_idx)
            {
                _stageData = Metadata.StageContainer.GetData(stage_idx);
            }
            return _stageData;
        }
    }


    public void Init()
    {

    }
}