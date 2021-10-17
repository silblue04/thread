using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Numerics;


public class AttackerContainer : DataContainer
{
    public class Data : DataBase
    {
        readonly public AttackerClassType attackter_class_type;
        readonly public string prefab_renderer_name;
        readonly public int attacker_base_stat_idx;

        public Data
        (
            int idx // attackter_idx
            , AttackerClassType attackter_class_type
            , string prefab_renderer_name
            , int attacker_base_stat_idx
        )
        : base(idx)
        {
            this.attackter_class_type = attackter_class_type;
            this.prefab_renderer_name = prefab_renderer_name;
            this.attacker_base_stat_idx = attacker_base_stat_idx;
        }
    }

    private Dictionary<int, Data> _datas = new Dictionary<int, Data>();


    public Data GetData(int attackter_idx)
    {
        if(_datas.ContainsKey(attackter_idx) == false)
        {
            Debug.LogErrorFormat
            (
                "[ERROR] data doesn't exist attackter_idx : {0} --> {1}"
                , attackter_idx
                , System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName
            );
            return null;
        }
        return _datas[attackter_idx];
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
            
            if (int.TryParse(datas[i]["attackter_idx"], out idx))
            {
                Data data = null;
                if (this._datas.TryGetValue(idx, out data) == false)
                {
                    AttackerClassType attackter_class_type;
                    if (!System.Enum.TryParse(datas[i]["attackter_class_type"], out attackter_class_type))
                    {
                        Debug.LogErrorFormat("attackter_class_type doesn't exit idx: {0}", idx);
                    }


                    string prefab_renderer_name = datas[i]["prefab_renderer_name"];
                    if (string.IsNullOrEmpty(prefab_renderer_name))
                    {
                        Debug.LogErrorFormat("prefab_renderer_name doesn't exit idx: {0}", idx);
                    }
                    

                    int attacker_base_stat_idx;
                    if (!int.TryParse(datas[i]["attacker_base_stat_idx"], out attacker_base_stat_idx))
                    {
                        Debug.LogErrorFormat("attacker_base_stat_idx doesn't exit idx: {0}", idx);
                    }


                    data = new Data
                    (
                        idx
                        , attackter_class_type
                        , prefab_renderer_name
                        , attacker_base_stat_idx
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