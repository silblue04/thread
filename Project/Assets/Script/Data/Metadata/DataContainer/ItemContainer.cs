using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Numerics;


public class ItemContainer : DataContainer
{
    public class Data : DataBase
    {
        readonly public Grade grade;

        readonly public string image_name;
        readonly public string name_key;
        public string Name
        {
            get
            {
                return Localization.Get(name_key);
            }
        }
        readonly public string desc_key;
        public string Desc
        {
            get
            {
                return Localization.Get(desc_key);
            }
        }

        public Data
        (
            int idx // item_idx
            , Grade grade

            , string image_name
            , string name_key
            , string desc_key
        )
        : base(idx)
        {
            this.grade = grade;

            this.image_name = image_name;
            this.name_key = name_key;
            this.desc_key = desc_key;
        }
    }

    private Dictionary<int, Data>[] _datasByItemType = new Dictionary<int, Data>[BitConvert.Enum32ToInt(ItemType.MAX)];


    public Data GetData(int item_idx)
    {
        ItemType itemType = DefsItem.GetItemType(item_idx);
        return GetData(itemType, item_idx);
    }

    public Data GetData(ItemType itemType, int item_idx)
    {
        int itemTypeIndex = BitConvert.Enum32ToInt(itemType);
        if(itemTypeIndex < DefsDefault.VALUE_ZERO || itemTypeIndex >= _datasByItemType.Length)
        {
            return null;
        }

        Dictionary<int, Data> datas = _datasByItemType[itemTypeIndex];
        if(datas == null)
        {
            return null;
        }
        
        if(datas.ContainsKey(item_idx) == false)
        {
            Debug.LogErrorFormat
            (
                "[ERROR] data doesn't exist item_idx : {0} --> {1}"
                , item_idx
                , System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName
            );
            return null;
        }
        return datas[item_idx];
    }

    public Dictionary<int, Data>.Enumerator GetEnumeratorByItemType(ItemType itemType)
    {
        int itemTypeIndex = BitConvert.Enum32ToInt(itemType);
        if(itemTypeIndex < DefsDefault.VALUE_ZERO || itemTypeIndex >= _datasByItemType.Length)
        {
            return new Dictionary<int, Data>().GetEnumerator();
        }

        Dictionary<int, Data> datas = _datasByItemType[itemTypeIndex];
        if(datas == null)
        {
            return new Dictionary<int, Data>().GetEnumerator();
        }

        return datas.GetEnumerator();
    }

    public override void ClearDatas()
    {
        foreach(var dicElem in _datasByItemType)
        {
            if(dicElem == null)
            {
                continue;
            }
            dicElem.Clear();
        }
    }

    public override void Parsing(List<Dictionary<string, string>> datas)
    {
        for (int i = 0; i < datas.Count; ++i)
        {
            int idx = 0;
            
            if (int.TryParse(datas[i]["item_idx"], out idx))
            {
                Grade grade;
                if (!System.Enum.TryParse(datas[i]["grade"], out grade))
                {
                    Debug.LogErrorFormat("grade doesn't exit idx: {0}", idx);
                }

                string image_name = datas[i]["image_name"];
                if (string.IsNullOrEmpty(image_name))
                {
                    Debug.LogErrorFormat("image_name doesn't exit idx: {0}", idx);
                }
                string name_key = datas[i]["name_key"];
                if (string.IsNullOrEmpty(name_key))
                {
                    Debug.LogErrorFormat("name_key doesn't exit idx: {0}", idx);
                }
                string desc_key = datas[i]["desc_key"];
                if (string.IsNullOrEmpty(desc_key))
                {
                    Debug.LogErrorFormat("desc_key doesn't exit idx: {0}", idx);
                }


                Data data = new Data
                (
                    idx
                    , grade

                    , image_name
                    , name_key
                    , desc_key
                );

                ItemType itemType = DefsItem.GetItemType(idx);
                int itemTypeIndex = BitConvert.Enum32ToInt(itemType);

                if(this._datasByItemType[itemTypeIndex] == null)
                {
                    this._datasByItemType[itemTypeIndex] = new Dictionary<int, Data>();
                }
                this._datasByItemType[itemTypeIndex].Add(idx, data);
            }
            else
            {
                Debug.LogError("idx is null or empty");
            }
        }
    }
}