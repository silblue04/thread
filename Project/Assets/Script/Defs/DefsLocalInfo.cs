using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;
using CodeStage.AntiCheat.ObscuredTypes;
using CodeStage.AntiCheat.Storage;
using System.Linq;
using System.Numerics;


public abstract class LocalInfoContainer
{
    [JsonIgnore] protected string LOCAL_INFO_FILE_NAME;

    [JsonProperty] private long _firstConnectTime = DefsDefault.VALUE_ZERO;
    [JsonProperty] private long _lastConnectTime = DefsDefault.VALUE_ZERO;

    
    protected abstract void _InitFileName();
    public virtual void Init()
    {
        _InitFileName();
        _LoadLocalFile();

        _InitContentsInfo();
    }

    public virtual void UpdateTimer()
    {

    }
    protected abstract void _InitContentsInfo();
    public abstract void ResetContents();


    protected void _LoadLocalFile()
    {
#if UNITY_EDITOR
        string jsonFile = UtilJson.LoadJsonFile(UnityEngine.Application.dataPath, LOCAL_INFO_FILE_NAME);
#else
        string jsonFile = ObscuredPrefs.GetString(LOCAL_INFO_FILE_NAME);
#endif

        bool isLocalInfoExist = (string.IsNullOrEmpty(jsonFile) == false);
        if (isLocalInfoExist)
        {
#if APRIL11TH_DEV
            Debug.LogFormat
            (
                "[LOAD] {0} --> {1}"
                , jsonFile
                , System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName
            );
#endif

            
            _Copy(jsonFile);
        }
        else
        {
#if APRIL11TH_DEV
            Debug.Log("[Progress] jsonFile is null");
#endif     
        }
    }

    protected abstract void _Copy(string jsonFile);
    
    protected void _CopyConnectTimeAndInitFirstConnectTime(LocalInfoContainer info)
    {
        this._firstConnectTime = info._firstConnectTime;
        this._lastConnectTime = info._lastConnectTime;

        _firstConnectTime = System.DateTime.UtcNow.Ticks;
    }

    public int GetIntervalOfLastConnectTimeToCurTime()
    {
        return UtilTime.GetTimeInterval(DateTime.UtcNow.Ticks, _lastConnectTime);
    }

    public void Save()
    {
        this._lastConnectTime = DateTime.UtcNow.Ticks;
        try
        {
            string json = JsonConvert.SerializeObject(this);
#if APRIL11TH_DEV
            Debug.LogFormat
            (
                "[SAVE] {0} --> {1}"
                , json
                , System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName
            );
#endif
            if (string.IsNullOrEmpty(json) == false)
            {
#if UNITY_EDITOR
                UtilJson.CreateJsonFile(UnityEngine.Application.dataPath, LOCAL_INFO_FILE_NAME, json);
#else
                ObscuredPrefs.SetString(LOCAL_INFO_FILE_NAME, json);
#endif
            }
        }
        catch (Exception e)
        {
            Debug.LogError(e.Data);
            Debug.LogError(e.Message);
            Application.Quit();
        }
    }
}


public class ItemInfo
{
    [JsonProperty] public int item_idx { get; protected set; } = DefsDefault.VALUE_NONE;
    [JsonIgnore] protected ObscuredInt _level   = DefsDefault.VALUE_ONE;
    [JsonIgnore] protected ObscuredInt _num     = DefsDefault.VALUE_ZERO;
    // [JsonProperty] public bool isFirstInPossession { get; protected set; } = true;

    // [JsonIgnore] public int MaxLevel { get { return ItemData.max_level; } }
    // [JsonIgnore] public bool IsMaxLevel { get { return (ItemData.max_level <= Level); } }

    [JsonProperty] public int Level
    {
        get
        {
            return _level;
        }
        protected set
        {
            _level = value;
        }
    }
    [JsonProperty] public int Num
    {
        get
        {
            return _num;
        }
        protected set
        {
            _num = value;
        }
    }

    [JsonIgnore] private ItemContainer.Data _itemData = null;
    [JsonIgnore] public ItemContainer.Data ItemData
    {
        get
        {
            if(_itemData == null)
            {
                _itemData = Metadata.ItemContainer.GetData(item_idx);
            }
            return _itemData;
        }
    }
    

    public ItemInfo() // for json
    {
        this.item_idx   = DefsDefault.VALUE_NONE;
        this.Level      = DefsDefault.VALUE_ONE;
        this.Num        = DefsDefault.VALUE_ZERO;
        //this.isFirstInPossession = true;
    }

    public ItemInfo(int item_idx)
    {
        this.item_idx   = item_idx;
        this.Level      = DefsDefault.VALUE_ONE;
        this.Num        = DefsDefault.VALUE_ZERO;
        //this.isFirstInPossession = true;
    }

    public ItemInfo(ItemInfo itemInfo)
    {
        this.item_idx   = itemInfo.item_idx;
        this.Level      = itemInfo.Level;
        this.Num        = itemInfo.Num;
        //this.isFirstInPossession = itemInfo.isFirstInPossession;
    }

    // public bool Enhance()
    // {
    //     if(IsMaxLevel)
    //     {
    //         return false;
    //     }

    //     Level += DefsDefault.VALUE_ONE;
    //     return true;
    // }
    
    public void AddNum(int amount)
    {
        Num += amount;
    }

    // public bool GetFirstInProssessionReward()
    // {
    //     if(isFirstInPossession == false)
    //     {
    //         return false;
    //     }
    //     isFirstInPossession = false;
    //     return true;
    // }
}


public abstract class LocalItemInfo<T> where T : ItemInfo
{
    [JsonProperty] protected Dictionary<int, T> _itemInfoDic = new Dictionary<int, T>();
    [JsonIgnore] public Action<int> OnAddNewItem = null;


    public bool IsInPossession(int item_idx)
    {
        return _itemInfoDic.ContainsKey(item_idx);
    }

    public bool IsItemEnough(int item_idx, BigInteger amount)
    {
        if(IsInPossession(item_idx) == false)
        {
            return false;
        }

        int amountInt = (int)amount;

        T itemInfo = _itemInfoDic[item_idx];
        return (itemInfo.Num >= amountInt);
    }

    public BigInteger GetItemNum(int item_idx)
    {
        if(IsInPossession(item_idx) == false)
        {
            return BigInteger.Zero;
        }

        T itemInfo = _itemInfoDic[item_idx];
        return itemInfo.Num;
    }

    public T GetItemInfo(int item_idx)
    {
        if (IsInPossession(item_idx) == false)
        {
            return null;
        }

        return _itemInfoDic[item_idx];
    }

    public Dictionary<int, T>.Enumerator GetItemInfoDicEnumerator()
    {
        return _itemInfoDic.GetEnumerator();
    }

    public abstract void AddItem(int item_idx, BigInteger amount);//, LogRefType logRefType);

    // public bool GetFirstInProssessionReward(int item_idx)
    // {
    //     if(IsInPossession(item_idx) == false)
    //     {
    //         return false;
    //     }
    //     return _itemInfoDic[item_idx].GetFirstInProssessionReward();
    // }

    // public bool Enhance(int item_idx)
    // {
    //     if(IsInPossession(item_idx) == false)
    //     {
    //         return false;
    //     }
    //     return _itemInfoDic[item_idx].Enhance();
    // }
}