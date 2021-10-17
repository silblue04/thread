using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Numerics;


public class ChapterContainer : DataContainer
{
    public class Data : DataBase
    {
        readonly public string prefab_name;

        public Data
        (
            int idx // chapter_idx
            , string prefab_name
        )
        : base(idx)
        {
            this.prefab_name = prefab_name;
        }
    }

    private List<Data> _datas = new List<Data>();


    public Data GetData(int chapter_idx)
    {
        int index = chapter_idx;
        if(index < DefsDefault.VALUE_ZERO || index >= _datas.Count)
        {
            return null;
        }
        return _datas[index];
    }

    public int GetMaxChapterNum()
    {
        return _datas.Count;
    }

    public override void ClearDatas()
    {
        _datas.Clear();
    }

    public override void Parsing(List<Dictionary<string, string>> datas)
    {
        for (int i = 0; i < datas.Count; ++i)
        {
            int idx = 0;

            if (int.TryParse(datas[i]["chapter_idx"], out idx))
            {
                string prefab_name = datas[i]["prefab_name"];
                if (string.IsNullOrEmpty(prefab_name))
                {
                    Debug.LogErrorFormat("prefab_name doesn't exit idx: {0}", idx);
                }


                Data data = new Data
                (
                    idx
                    , prefab_name
                );
                this._datas.Add(data);
            }
            else
            {
                Debug.LogError("idx is null or empty");
            }
        }
    }
}