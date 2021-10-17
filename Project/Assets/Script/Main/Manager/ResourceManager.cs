using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;


// TODO : 아래처럼 이미지 가져오지 맙시다
public class ResourceManager : Singleton<ResourceManager>
{
    public void Release()
    {
        _ReleaseFont();
    }


    [SerializeField] public TMP_FontAsset[] fontAssetList = new TMP_FontAsset[Metadata.Languages.Length];

    public void ChangeFallbackFont(int languageIndex)
    {
        int defaultLanguageIndex = DefsDefault.VALUE_ZERO;
        if(languageIndex == defaultLanguageIndex)
        {
            return;
        }
        if(languageIndex < DefsDefault.VALUE_ZERO || languageIndex >= fontAssetList.Length)
        {
            return;
        }

        TMP_FontAsset defaultFont = fontAssetList[defaultLanguageIndex];
        if(defaultFont.fallbackFontAssetTable == null)
        {
            defaultFont.fallbackFontAssetTable = new List<TMP_FontAsset>();
        }
        defaultFont.fallbackFontAssetTable.Clear();
        defaultFont.fallbackFontAssetTable.Add(fontAssetList[languageIndex]);
    }
    private void _ReleaseFont()
    {
        int defaultLanguageIndex = DefsDefault.VALUE_ZERO;

        TMP_FontAsset defaultFont = fontAssetList[defaultLanguageIndex];
        if(defaultFont.fallbackFontAssetTable != null)
        {
            defaultFont.fallbackFontAssetTable.Clear();
        }
    }

    [Space]
    [SerializeField] private MaterialCollector _materialCollector = null;
    public MaterialCollector MaterialCollector
    {
        get
        {
            return _materialCollector;
        }
    }


    // [Space]
    // private const string PAK_SKIN_COLLECTOR_PREFAB_FILE_NAME_FORMAT = "Prefabs/ResourceCollector/PakSkinCollector/{0}";
    // private PakImageCollector _pakSkinCollector = null;
    // public PakImageCollector GetPakSkinCollector(int pak_idx)
    // {
    //     if (_pakSkinCollector == null)
    //     {
    //         var pakData         = Metadata.PakContainer.GetData(pak_idx);
    //         _pakSkinCollector   = Util.LoadResources<PakImageCollector>(this.transform, string.Format(PAK_SKIN_COLLECTOR_PREFAB_FILE_NAME_FORMAT, pakData.pak_skin_collector_name));
    //     }
    //     else if (_pakSkinCollector.Pak_idx != pak_idx)
    //     {
    //         Destroy(_pakSkinCollector);

    //         var pakData         = Metadata.PakContainer.GetData(pak_idx);
    //         _pakSkinCollector   = Util.LoadResources<PakImageCollector>(this.transform, string.Format(PAK_SKIN_COLLECTOR_PREFAB_FILE_NAME_FORMAT, pakData.pak_skin_collector_name));
    //     }
    //     return _pakSkinCollector;
    // }

    
    // private const string MONSTER_IMAGE_COLLECTOR_PREFAB_FILE_NAME_FORMAT = "Prefabs/ResourceCollector/MonsterImageCollector/{0}";
    // private PakImageCollector _monsterImageCollector = null;
    // public PakImageCollector GetMonsterImageCollector(int pak_idx)
    // {
    //     if (_monsterImageCollector == null)
    //     {
    //         var pakData                 = Metadata.PakContainer.GetData(pak_idx);
    //         _monsterImageCollector      = Util.LoadResources<PakImageCollector>(this.transform, string.Format(MONSTER_IMAGE_COLLECTOR_PREFAB_FILE_NAME_FORMAT, pakData.monster_image_collector_name));
    //     }
    //     else if (_monsterImageCollector.Pak_idx != pak_idx)
    //     {
    //         Destroy(_monsterImageCollector);

    //         var pakData                 = Metadata.PakContainer.GetData(pak_idx);
    //         _monsterImageCollector      = Util.LoadResources<PakImageCollector>(this.transform, string.Format(MONSTER_IMAGE_COLLECTOR_PREFAB_FILE_NAME_FORMAT, pakData.monster_image_collector_name));
    //     }
    //     return _monsterImageCollector;
    // }

    [SerializeField] private PixelImageCollector _pixelImageCollector = null;
    public PixelImageCollector PixelImageCollector
    {
        get
        {
            return _pixelImageCollector;
        }
    }

    [SerializeField] private MaterialCollector _unitSpineMaterialCollector = null;
    public MaterialCollector UnitSpineMaterialCollector
    {
        get
        {
            return _unitSpineMaterialCollector;
        }
    }
    [SerializeField] private AssetCollector _unitSpineSkeletonDataCollector = null;
    public AssetCollector UnitSpineSkeletonDataCollector
    {
        get
        {
            return _unitSpineSkeletonDataCollector;
        }
    }

    [SerializeField] private SpriteCollector _itemGradeIconCollector = null;
    public SpriteCollector ItemGradeIconCollector
    {
        get
        {
            return _itemGradeIconCollector;
        }
    }
    [SerializeField] private SpriteCollector _itemImageCollector = null;
    public SpriteCollector ItemIconCollector
    {
        get
        {
            return _itemImageCollector;
        }
    }

    [SerializeField] private SpriteCollector _equipmentIconCollector = null;
    public SpriteCollector EquipmentIconCollector
    {
        get
        {
            return _equipmentIconCollector;
        }
    }
    [SerializeField] private SpriteCollector _equipmentSetCollector = null;
    public SpriteCollector EquipmentSetCollector
    {
        get
        {
            return _equipmentSetCollector;
        }
    }

    [SerializeField] private AudioEventCollector _projectileAudioEventCollector = null;
    public AudioEventCollector ProjectileAudioEventCollector
    {
        get
        {
            return _projectileAudioEventCollector;
        }
    }

    [SerializeField] private MaterialCollector _pakSpineMaterialCollector = null;
    public MaterialCollector PakSpineMaterialCollector
    {
        get
        {
            return _pakSpineMaterialCollector;
        }
    }

    [SerializeField] private AssetCollector _pakSpineSkeletonDataCollector = null;
    public AssetCollector PakSpineSkeletonDataCollector
    {
        get
        {
            return _pakSpineSkeletonDataCollector;
        }
    }

    [SerializeField] private SpriteCollector _addOnIconCollector = null;
    public SpriteCollector AddOnIconCollector
    {
        get
        {
            return _addOnIconCollector;
        }
    }
    [SerializeField] private SpriteCollector _buffIconCollector = null;
    public SpriteCollector BuffIconCollector
    {
        get
        {
            return _buffIconCollector;
        }
    }
    
    [SerializeField] private SpriteCollector _treasureIconCollector = null;
    public SpriteCollector TreasureIconCollector
    {
        get
        {
            return _treasureIconCollector;
        }
    }

    [SerializeField] private SpriteCollector _productIconCollector = null;
    public SpriteCollector ProductIconCollector
    {
        get
        {
            return _productIconCollector;
        }
    }


    private SpriteCollector _gachaIconCollector = null;
    public SpriteCollector GachaIconCollector
    {
        get
        {
            _gachaIconCollector = Util.LoadResources<SpriteCollector>(this.transform, "Prefabs/ResourceCollector/GachaIconCollector");
            return _gachaIconCollector;
        }
    }
}
