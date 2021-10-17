using System.Collections.Generic;
using UnityEngine;

public class DefineContainer : DataContainer
{
    private Dictionary<string, float> _defineDatas = new Dictionary<string, float>();
    private Dictionary<string, float[]> _defineArrDatas = new Dictionary<string, float[]>();


    public override void Parsing(List<Dictionary<string, string>> datas)
    {
        for (int i = 0; i < datas.Count; i++)
        {
            string key = datas[i]["idx"].Trim();
            if (!string.IsNullOrEmpty(key))
            {
                if (FirebaseHelper.IsRemoteConfigValue(key))
                {
                    float remote_value = FirebaseHelper.GetRemoteConfigValue(key);

                    if (!_defineDatas.ContainsKey(key))
                    {
                        _defineDatas.Add(key, remote_value);
                    }
                    continue;
                }


                string value = datas[i]["value"];
                if (!string.IsNullOrEmpty(value))
                {
                    string[] values = value.Split(',');
                    if (values.Length > 1)
                    {
                        float[] inputValue = new float[values.Length];
                        for (int j = 0; j < inputValue.Length; j++)
                        {
                            float.TryParse(values[j], out inputValue[j]);
                        }

                        if (!_defineArrDatas.ContainsKey(key))
                        {
                            _defineArrDatas.Add(key, inputValue);
                        }
                        else
                        {
                            Debug.LogErrorFormat("duplicate value, key: {0}, value: {1}", key, inputValue.Length);
                        }
                    }
                    else
                    {
                        float inputValue = 0.0f;
                        if (float.TryParse(value, out inputValue))
                        {
                            if (!_defineDatas.ContainsKey(key))
                            {
                                _defineDatas.Add(key, inputValue);
                            }
                            else
                            {
                                Debug.LogErrorFormat("duplicate value, key: {0}, value: {1}", key, inputValue);
                            }
                        }
                    }
                }
                else
                {
                    Debug.LogErrorFormat("this is not float type, key: {0}, value: {1}", key, value);
                }
            }
            else
            {
                Debug.LogErrorFormat("define, key is null or empty");
            }
        }
    }

    public float GetValue(string key)
    {
        float retVal = -1;

        if (!_defineDatas.TryGetValue(key, out retVal))
        {
            Debug.LogErrorFormat("{0} dont exist in defineDatas", key);
        }
        return retVal;
    }

    public float[] GetValueArr(string key)
    {
        float[] retVal = null;
        if (!_defineArrDatas.TryGetValue(key, out retVal))
        {
            Debug.LogErrorFormat("{0} dont exist in defineArrDatas", key);
        }
        return retVal;
    }

    public override void ClearDatas()
    {
        _defineDatas.Clear();
    }

    public int GetCount()
    {
        return _defineDatas.Count;
    }
}
