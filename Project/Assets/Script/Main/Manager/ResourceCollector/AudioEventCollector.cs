using UnityEngine;
using System.Collections;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
using System.IO;
#endif


[System.Serializable]
public class AudioEventCollectorDictionary : SerializableDictionary<string, AudioEvent> {}

public class AudioEventCollector : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField] private string _directory = "/Resources/Prefabs/AudioEvent/";
    [SerializeField] private bool _recursive = true;
#endif

    [SerializeField] private AudioEventCollectorDictionary _audioEventCollectorDictionary = null;


    public AudioEvent Get(string key)
    {
        return Get(key, true);
    }

    public AudioEvent Get(string key, bool logging)
    {
        if (string.IsNullOrEmpty(key) || _audioEventCollectorDictionary.Count == 0)
        {
            return null;
        }

        AudioEvent audioEvent = null;
        if (_audioEventCollectorDictionary.TryGetValue(key, out audioEvent))
        {
            return audioEvent;
        }

        if (logging)
        {
            Debug.Log("Unknown audio event : " + key);
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
        

        if(_audioEventCollectorDictionary != null)
        {
            _audioEventCollectorDictionary.Clear();
            _audioEventCollectorDictionary = null;
        }
        _audioEventCollectorDictionary = new AudioEventCollectorDictionary();


        string[] prefabFiles = Directory.GetFiles(Application.dataPath + _directory, "*.prefab", _recursive ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly);

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
                if (subObjects[j] is AudioEvent)
                {
                    AudioEvent audioEvent = subObjects[j] as AudioEvent;
                    _audioEventCollectorDictionary.Add(audioEvent.name, audioEvent);
                }
            }
        }
        EditUtil.TouchPrefab();
    }
#endif
}
