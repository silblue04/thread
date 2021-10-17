using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Numerics;


public class StageContainer : DataContainer
{
    public class Data : DataBase
    {
        readonly public IList<int> monsterIdxList;

        public Data
        (
            int idx // stage_idx
            , List<int> monsterIdxList
        )
        : base(idx)
        {
            this.monsterIdxList  = monsterIdxList.AsReadOnly();
        }
    }

    private List<Data> _datas = new List<Data>();


    public Data GetData(int stage_idx)
    {
        int index = stage_idx;
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

            if (int.TryParse(datas[i]["stage_idx"], out idx))
            {
                string monster_idx_list = datas[i]["monster_idx_list"];
                if (string.IsNullOrEmpty(monster_idx_list))
                {
                    Debug.LogErrorFormat("monster_idx_list doesn't exit idx: {0}", idx);
                }

                string[] splitStringList = monster_idx_list.Split(';');
                List<int> monsterIdxList = new List<int>();
                for(int j = 0; j < splitStringList.Length; ++j)
                {
                    int monster_idx = int.Parse(splitStringList[j]);
                    if(monster_idx == DefsDefault.VALUE_NONE)
                    {
                        continue;
                    }
                    monsterIdxList.Add(monster_idx);
                }


                Data data = new Data
                (
                    idx
                    , monsterIdxList
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