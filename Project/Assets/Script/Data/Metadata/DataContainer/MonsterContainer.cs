using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Numerics;


public class MonsterContainer : DataContainer
{
    public class Data : DataBase
    {
        readonly public MonsterType monster_type;
        readonly public string prefab_renderer_name;
        readonly public int monster_base_stat_idx;

        public Data
        (
            int idx // monster_idx
            , MonsterType monster_type
            , string prefab_renderer_name
            , int monster_base_stat_idx
        )
        : base(idx)
        {
            this.monster_type = monster_type;
            this.prefab_renderer_name = prefab_renderer_name;
            this.monster_base_stat_idx = monster_base_stat_idx;
        }
    }

    private Dictionary<int, Data> _datas = new Dictionary<int, Data>();


    public Data GetData(int monster_idx)
    {
        if(_datas.ContainsKey(monster_idx) == false)
        {
            Debug.LogErrorFormat
            (
                "[ERROR] data doesn't exist monster_idx : {0} --> {1}"
                , monster_idx
                , System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName
            );
            return null;
        }
        return _datas[monster_idx];
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
            
            if (int.TryParse(datas[i]["monster_idx"], out idx))
            {
                Data data = null;
                if (this._datas.TryGetValue(idx, out data) == false)
                {
                    MonsterType monster_type;
                    if (!System.Enum.TryParse(datas[i]["monster_type"], out monster_type))
                    {
                        Debug.LogErrorFormat("monster_type doesn't exit idx: {0}", idx);
                    }


                    string prefab_renderer_name = datas[i]["prefab_renderer_name"];
                    if (string.IsNullOrEmpty(prefab_renderer_name))
                    {
                        Debug.LogErrorFormat("prefab_renderer_name doesn't exit idx: {0}", idx);
                    }
                    

                    int monster_base_stat_idx;
                    if (!int.TryParse(datas[i]["monster_base_stat_idx"], out monster_base_stat_idx))
                    {
                        Debug.LogErrorFormat("monster_base_stat_idx doesn't exit idx: {0}", idx);
                    }


                    data = new Data
                    (
                        idx
                        , monster_type
                        , prefab_renderer_name
                        , monster_base_stat_idx
                    );
                    this._datas.Add(idx, data);
                }
                else
                {
                    Debug.LogErrorFormat("already exist data, idx: {0}", idx);
                }
            }
            else
            {
                Debug.LogError("idx is null or empty");
            }
        }
    }
}