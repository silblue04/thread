using System.Numerics;


public static class DefsItem
{
    //static public int MAX_BASIC_CURRENCY_TYPE { get; private set; }


    static public void InitDefineDatas()
    {
        //MAX_BASIC_CURRENCY_TYPE = Util.GetEnumMax<BasicCurrencyType>();
    }

    static public ItemType GetItemType(int item_idx)
    {
        ItemType itemType = ItemType.Currency;

        if(0 <= item_idx && item_idx <= 99)
        {
            itemType = ItemType.Currency;
        }
        // else if(10000 <= item_idx && item_idx <= 99999)
        // {
        //     itemType = ItemType.Projectile;
        // }
        // else if(100000 <= item_idx && item_idx <= 999999)
        // {
        //     itemType = ItemType.Costume;
        // }

        return itemType;
    }

    static public int ConvertBasicCurrencyTypeToItemIdx(CurrencyType basicCurrencyType)
    {
        return BitConvert.Enum32ToInt(basicCurrencyType);
    }

    static public CurrencyType ConvertItemIdxToBasicCurrencyType(int item_idx)
    {
        if(GetItemType(item_idx) != ItemType.Currency)
        {
            return CurrencyType.NONE;
        }
        return (CurrencyType)item_idx;
    }


    static public UnityEngine.Sprite GetItemIcon(CurrencyType basicCurrencyType)
    {
        return GetItemIcon(ConvertBasicCurrencyTypeToItemIdx(basicCurrencyType));
    } 

    static public UnityEngine.Sprite GetItemIcon(int item_idx)
    {
        var itemData = Metadata.ItemContainer.GetData(item_idx);
        if(itemData == null)
        {
            return null;
        }

        return null; // TODO : 아이템 아이콘
        //return ResourceManager.Instance.ItemIconCollector.Get(itemData.image_key);
    }

    static public string GetItemValueText(int item_idx, BigInteger amount)
    {
        ItemType itemType = GetItemType(item_idx);
        if(itemType == ItemType.Currency)
        {
            CurrencyType basicCurrencyType = ConvertItemIdxToBasicCurrencyType(item_idx);
            return GetItemValueText(basicCurrencyType, amount);
        }

        return amount.ToString();
    }
    static public string GetItemValueText(CurrencyType basicCurrencyType, BigInteger amount)
    {
        switch (basicCurrencyType)
        {
            case CurrencyType.Gold:
                return Util.ConvertNumberToShort(amount);
        }

        return amount.ToString();
    }

    static public Grade GetGrade(int item_idx)
    {
        var itemData = Metadata.ItemContainer.GetData(item_idx);
        if(itemData == null)
        {
            return DefsDefault.VALUE_ZERO;
        }

        return itemData.grade;
    }
    static public UnityEngine.Sprite GetGradeIcon(int item_idx)
    {
        Grade grade = GetGrade(item_idx);
        return ResourceManager.Instance.ItemGradeIconCollector.Get(DefsString.GradeString[BitConvert.Enum32ToInt(grade)]);
    }
}


public enum ItemType
{
    Currency = 0,

    MAX
}
public class ItemTypeComparer : System.Collections.Generic.IEqualityComparer<ItemType>
{ 
    bool System.Collections.Generic.IEqualityComparer<ItemType>.Equals(ItemType a, ItemType b) { return a == b; } 
    int System.Collections.Generic.IEqualityComparer<ItemType>.GetHashCode(ItemType obj) { return (int)obj; } 
}

public enum CurrencyType
{
    NONE = DefsDefault.VALUE_NONE,

    Gem = 0,
    Gold,

    MAX
}

public enum Grade
{
    Common,
    Rare,
    Epic,
    Unique,
    Legendary,

    MAX
}
