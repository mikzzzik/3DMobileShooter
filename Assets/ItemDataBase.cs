using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public class ItemDataBase : MonoBehaviour
{
    [SerializeField] private List<Item> _itemList;

    public static Action<List<SlotContainer>, List<SlotContainer>> OnSavePrefs;
    private void OnEnable()
    {
        OnSavePrefs += SavePrefs;
    }

    private void OnDisable()
    {

    }

    public Item GetItem(int id)
    {
        for(int i = 0; i < _itemList.Count; i++)
        {
            if(_itemList[i].GetInstanceID() == id)
            {
                return _itemList[i];
            }
        }
        return null;
    }

    private void SavePrefs(List<SlotContainer> _mainSlot, List<SlotContainer> _hotbarSlot)
    {
        for (int i = 0; i < _mainSlot.Count; i++)
        {
            if (_mainSlot[i].GetItem() == null)
                PlayerPrefs.SetInt("MainItemIndex_" + i, -1);
            else
                PlayerPrefs.SetInt("MainItemIndex_" + i, _mainSlot[i].GetItem().GetInstanceID());
            PlayerPrefs.SetInt("MainItemAmount_" + i, _mainSlot[i].GetAmount());
        }
        for (int i = 0; i < _hotbarSlot.Count; i++)
        {
            if (_hotbarSlot[i].GetItem() == null)
                PlayerPrefs.SetInt("HotbarItemIndex_" + i, -1);
            else
                PlayerPrefs.SetInt("HotbarItemIndex_" + i, _hotbarSlot[i].GetItem().GetInstanceID());
            PlayerPrefs.SetInt("HotbarItemAmount_" + i, _hotbarSlot[i].GetAmount());
        }
        OnSavePrefs -= SavePrefs;
    }
}
