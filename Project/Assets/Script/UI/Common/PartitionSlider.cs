using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PartitionSlider : Slider
{
    [SerializeField] private RectTransform _fillArea;

    [Header("Prefab")]
    [SerializeField] private Transform _partitionRoot;
    [SerializeField] private GameObject _partitionPrefab;

    private List<GameObject> _partitionList = new List<GameObject>();
    public float OnePartSizeX { get { return _fillArea.rect.width/this.maxValue; }}


    protected override void Awake()
    {
        base.Awake();
        
        _partitionPrefab.SetActive(false);
        _partitionList.Add(_partitionPrefab);
    }

    public void SetMaxValue(int maxValue)
    {
        this.maxValue = maxValue;


        int neededPartitionNum = maxValue - 1;
        float lenght = _fillArea.rect.width;
        float intervalX = lenght / this.maxValue;

        if(_partitionList.Count < neededPartitionNum)
        {
            for (int i = 0; i < _partitionList.Count; ++i)
            {
                _partitionList[i].transform.localPosition = new Vector3(
                    (i + 1) * intervalX
                    , _partitionList[i].transform.localPosition.y
                    , _partitionList[i].transform.localPosition.z
                );
                _partitionList[i].SetActive(true);
            }
            for (int i = _partitionList.Count; i < neededPartitionNum; ++i)
            {
                GameObject instance = Util.Instantiate(_partitionRoot, _partitionPrefab);
                instance.transform.localPosition = new Vector3(
                    (_partitionList.Count + 1) * intervalX
                    , instance.transform.localPosition.y
                    , instance.transform.localPosition.z
                );
                instance.SetActive(true);

                _partitionList.Add(instance);
            }
        }
        else
        {
            for (int i = 0; i < neededPartitionNum; ++i)
            {
                _partitionList[i].transform.localPosition = new Vector3(
                    (i + 1) * intervalX
                    , _partitionList[i].transform.localPosition.y
                    , _partitionList[i].transform.localPosition.z
                );
                _partitionList[i].SetActive(true);
            }
            for (int i = neededPartitionNum; i < _partitionList.Count; ++i)
            {
                _partitionList[i].SetActive(false);
            }
        }
    }
}
