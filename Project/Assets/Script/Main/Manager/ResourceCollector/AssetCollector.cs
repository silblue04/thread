using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif


[System.Serializable]
public class AssetCollectorDictionary : SerializableDictionary<string, Object> {}

public class AssetCollector : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private string _directory = "/NotLoadResources/";
    [SerializeField] private string _ContainedFileName = string.Empty;
    [SerializeField] private string _seachPattern = "*.asset";
    [SerializeField] private bool _recursive = true;
#endif

    [SerializeField] private AssetCollectorDictionary _assetCollectorDictionary = null;


    public T Get<T>(string key) where T : Object
    {
        return Get<T>(key, true);
    }

    public T Get<T>(string key, bool logging) where T : Object
    {
        if (string.IsNullOrEmpty(key) || _assetCollectorDictionary.Count == 0)
        {
            return null;
        }

        Object objectAsset = null;
        if (_assetCollectorDictionary.TryGetValue(key, out objectAsset))
        {
            T asset = objectAsset as T;
            return asset;
        }

        if (logging)
        {
            Debug.Log("Unknown asset : " + key);
        }


        return null;
    }

#if UNITY_EDITOR
    [ContextMenu("Build")]
    public void Build()
    {
        if(_directory.EndsWith("NONE"))
        {
            return;
        }
        if(Directory.Exists(Application.dataPath + _directory) == false)
        {
            return;
        }
        

        if(_assetCollectorDictionary != null)
        {
            _assetCollectorDictionary.Clear();
            _assetCollectorDictionary = null;
        }
        _assetCollectorDictionary = new AssetCollectorDictionary();


        string[] prefabFiles = Directory.GetFiles(Application.dataPath + _directory, _seachPattern, _recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

        for (int i = 0; i < prefabFiles.Length; ++i)
        {
            int idx = prefabFiles[i].IndexOf("Assets/");
            if (idx < 0)
            {
                Debug.LogError("error! ");
                continue;
            }
            string path = prefabFiles[i].Substring(idx);
            Object[] subObjects = AssetDatabase.LoadAllAssetsAtPath(path);

            for (int j = 0; j < subObjects.Length; j++)
            {
                if (subObjects[j] is Object)
                {
                    Object Asset = subObjects[j] as Object;
                    if(Asset.name.Contains(_ContainedFileName))
                    {
                        _assetCollectorDictionary.Add(Asset.name, Asset);
                    }
                }
            }
        }
        EditUtil.TouchPrefab();
    }
#endif
}
