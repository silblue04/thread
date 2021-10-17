using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;
using UnityEngine.Events;


public class ItemSpreader : MonoBehaviour
{
    [Header("-- Base Information ------")]
    [SerializeField] private Ease _ease = Ease.InOutFlash;
    [SerializeField] private float _duration = 1.2f;
    [SerializeField] private float _maxDelay = 0.6f;
    [SerializeField] private float _minDelay = 0.0f;

    [Header("Prefab")]
    [SerializeField] private ItemSpreaderIcon _itemSpreaderIcon;
    private LinkedList<ItemSpreaderIcon> _idleItemSpreaderIconList = new LinkedList<ItemSpreaderIcon>();
    private LinkedList<ItemSpreaderIcon> _tweeningItemSpreaderIconList = new LinkedList<ItemSpreaderIcon>();


    public void Init(int capacity = 10)
    {
        _AddIdleItemTweenInfo(capacity);
    }

    private void _AddIdleItemTweenInfo(int amount)
    {
        for(int i = 0; i < amount; ++i)
        {
            ItemSpreaderIcon instance
                = Util.AddChildWithTrans<ItemSpreaderIcon>(this.gameObject, _itemSpreaderIcon);

            instance.Init();
            _idleItemSpreaderIconList.AddLast(instance);
        }
    }

    public void Spread
    (
        Vector2 startPos, Vector2 targetPos
        , int item_idx
        , int amount
        , float spreadRange
    )
    {
        if(amount == 0)
        {
            return;
        }
        if(amount > DefsInGame.MAX_ITEM_NUM_PER_SPREAD)
        {
            amount = DefsInGame.MAX_ITEM_NUM_PER_SPREAD;
        }

        if(_idleItemSpreaderIconList.Count < amount)
        {
            _AddIdleItemTweenInfo(amount - _idleItemSpreaderIconList.Count);
        }


        float middleSpreadRangeX = spreadRange * 2.0f;

        for(int i = 0; i < amount; ++i)
        {
            ItemSpreaderIcon idleItemIcon = _idleItemSpreaderIconList.First.Value;

            idleItemIcon.gameObject.SetActive(true);
            idleItemIcon.transform.position = startPos;

            List<Vector3> path = new List<Vector3>();

            Vector2 middlePath = new Vector2(
                    startPos.x + Random.Range(-middleSpreadRangeX, middleSpreadRangeX)
                    , startPos.y + Random.Range(-spreadRange, spreadRange));

            path.Add(middlePath);
            path.Add(targetPos);

            idleItemIcon.SetSprite(item_idx);
            idleItemIcon.SetPathTween(
                _tweeningItemSpreaderIconList.Count
                , path, _ease, _duration, _maxDelay, _minDelay
                , _RemoveTweeningItemIcon);

            _tweeningItemSpreaderIconList.AddLast(idleItemIcon);
            _idleItemSpreaderIconList.RemoveFirst();
        }
    }

    private void _RemoveTweeningItemIcon(ItemSpreaderIcon itemIcon)
    {
        _tweeningItemSpreaderIconList.Remove(itemIcon);
    }
}
