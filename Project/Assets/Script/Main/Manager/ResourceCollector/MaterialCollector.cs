using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif


[System.Serializable]
public class MaterialCollectorDictionary : SerializableDictionary<string, Material> {}

public class MaterialCollector : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private string _directory = "/NotLoadResources/Spine/";
    [SerializeField] private bool _recursive = true;
#endif

    [SerializeField] private MaterialCollectorDictionary _materialCollectorDictionary = null;


    public Material Get(string key)
    {
        return Get(key, true);
    }

    public Material Get(string key, bool logging)
    {
        if (string.IsNullOrEmpty(key) || _materialCollectorDictionary.Count == 0)
        {
            return null;
        }

        Material material = null;
        if (_materialCollectorDictionary.TryGetValue(key, out material))
        {
            return material;
        }

        if (logging)
        {
            Debug.Log("Unknown material : " + key);
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
        

        if(_materialCollectorDictionary != null)
        {
            _materialCollectorDictionary.Clear();
            _materialCollectorDictionary = null;
        }
        _materialCollectorDictionary = new MaterialCollectorDictionary();


        string[] prefabFiles = Directory.GetFiles(Application.dataPath + _directory, "*.mat", _recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

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
                if (subObjects[j] is Material)
                {
                    Material Material = subObjects[j] as Material;
                    _materialCollectorDictionary.Add(Material.name, Material);
                }
            }
        }
        EditUtil.TouchPrefab();
    }
#endif
}
