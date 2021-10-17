﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif


[System.Serializable]
public class SpriteCollectorDictionary : SerializableDictionary<string, Sprite> {}

public class SpriteCollector : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private string _directory = "/NotLoadResources/Sprite/";
    [SerializeField] private bool _recursive = true;
#endif

    [SerializeField] private SpriteCollectorDictionary _spriteCollectorDictionary = null;


    public Sprite Get(string key)
    {
        return Get(key, true);
    }

    public Sprite Get(string key, bool logging)
    {
        if (string.IsNullOrEmpty(key) || _spriteCollectorDictionary.Count == 0)
        {
            return null;
        }

        Sprite image = null;
        if (_spriteCollectorDictionary.TryGetValue(key, out image))
        {
            return image;
        }

        if (logging)
        {
            Debug.Log("Unknown sprite : " + key);
        }


        return null;
    }

#if UNITY_EDITOR
    [ContextMenu("Build")]
    public virtual bool Build()
    {
        if(_directory.EndsWith("NONE"))
        {
            return false;
        }

        if(Directory.Exists(Application.dataPath + _directory) == false)
        {
            return false;
        }
        

        if(_spriteCollectorDictionary != null)
        {
            _spriteCollectorDictionary.Clear();
            _spriteCollectorDictionary = null;
        }
        _spriteCollectorDictionary = new SpriteCollectorDictionary();

        string[] pngFiles = Directory.GetFiles(Application.dataPath + _directory, "*.png", _recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);
        string[] jpgFiles = Directory.GetFiles(Application.dataPath + _directory, "*.jpg", _recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

        string[] allFiles = new string[pngFiles.Length + jpgFiles.Length];

        System.Array.Copy(pngFiles, 0, allFiles, 0, pngFiles.Length);
        System.Array.Copy(jpgFiles, 0, allFiles, pngFiles.Length, jpgFiles.Length);

        for (int i = 0; i < allFiles.Length; ++i)
        {
            int idx = allFiles[i].IndexOf("Assets/");
            if (idx < 0)
            {
                Debug.LogError("error! ");
                continue;
            }
            string path = allFiles[i].Substring(idx);
            Object[] subObjects = AssetDatabase.LoadAllAssetsAtPath(path);

            for (int j = 0; j < subObjects.Length; j++)
            {
                if (subObjects[j] is Sprite)
                {
                    Sprite spr = subObjects[j] as Sprite;
                    _spriteCollectorDictionary.Add(spr.name, spr);
                }
            }
        }

        EditUtil.TouchPrefab();
        return true;
    }
#endif
}
