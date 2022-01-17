using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Networking;
using System.Linq;
public class ItemDataBase : MonoBehaviour
{
    [SerializeField] private List<Item> _itemList;

    public static Action<List<SlotContainer>, List<SlotContainer>> OnSavePrefs;
    public static Action<List<SlotContainer>> OnUpdateInventory;

    private void OnEnable()
    {
        OnSavePrefs += SavePrefs;
        OnUpdateInventory += UpdateInventoryDataBase;

    }

    private void OnDestroy()
    {
        OnSavePrefs -= SavePrefs;
        OnUpdateInventory -= UpdateInventoryDataBase; 
    }

    public Item GetItem(int id)
    {
        for(int i = 0; i < _itemList.Count; i++)
        {   
            if(_itemList[i].ItemId == id)
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
                PlayerPrefs.SetInt("MainItemIndex_" + i, _mainSlot[i].GetItem().ItemId);
            PlayerPrefs.SetInt("MainItemAmount_" + i, _mainSlot[i].GetAmount());
        }
        for (int i = 0; i < _hotbarSlot.Count; i++)
        {
            if (_hotbarSlot[i].GetItem() == null)
                PlayerPrefs.SetInt("HotbarItemIndex_" + i, -1);
            else
                PlayerPrefs.SetInt("HotbarItemIndex_" + i, _hotbarSlot[i].GetItem().ItemId);
            PlayerPrefs.SetInt("HotbarItemAmount_" + i, _hotbarSlot[i].GetAmount());
        }
        OnSavePrefs -= SavePrefs;
    }

    private void UpdateInventoryDataBase(List<SlotContainer> slots)
    {
        StartCoroutine(UpdateInventory(slots));
    }

    private IEnumerator UpdateInventory(List<SlotContainer> slots)
    {
        WWWForm form = new WWWForm();
        UnityWebRequest www;

        form.AddField("id", PlayerPrefs.GetInt("PlayerID"));

        for (int i = 0; i < slots.Count; i++)
        {
            form.AddField("slot_id", i);
            form.AddField("slot_type", slots[i].GetSlotType().ToString());
            Item item = slots[i].GetItem();
            if (item == null)
            {
                form.AddField("item_id", -1);
            }
            else
            {
                form.AddField("item_id", item.ItemId);
            }
            form.AddField("item_count", slots[i].GetAmount());

            www = UnityWebRequest.Post("http://m94820.hostua01.fornex.org/sql/SaveInventory.php", form);
            yield return www.SendWebRequest();
            List<string> webRequest = www.downloadHandler.text.Split('\t').ToList();

            string status = webRequest[0];

            webRequest.RemoveAt(0);
            Debug.Log(status);
            if (bool.Parse(status))
            {
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                
            }
            else
            {
                Debug.Log(webRequest[0]);
            }
        }
    }
}
