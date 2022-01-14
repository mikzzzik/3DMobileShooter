using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Networking;
using System;
public class MainMenuController : MonoBehaviour
{
    [SerializeField] private MainMenuInventoryController _inventoryController;
    [SerializeField] private string _setUrl;
    [SerializeField] private string _updateUrl;
    [SerializeField] private string _getUrl;
    [SerializeField] private ItemDataBase _itemDB;
    public static Action<List<SlotContainer>> OnUpdateSlots;

    void Start()
    {
        StartCoroutine(LoadInventory());
    }

    private void OnEnable()
    {
        OnUpdateSlots += UpdateInventory;
    }

    private void OnDisable()
    {
        OnUpdateSlots -= UpdateInventory;
        _inventoryController.SavePrefs();
    }

    private IEnumerator LoadInventory()
    {
        WWWForm form = new WWWForm();
        form.AddField("id", PlayerPrefs.GetInt("PlayerID"));
        UnityWebRequest www = UnityWebRequest.Post("http://m94820.hostua01.fornex.org/sql/CheckInventoryTable.php", form);

        yield return www.SendWebRequest();

        List<string> webRequest = www.downloadHandler.text.Split('\t').ToList();
        string status = webRequest[0];
        webRequest.RemoveAt(0);
        if(bool.Parse(status))
        {
            _inventoryController.LoadInventory(GetInventory);
        }
        else
        {
            _inventoryController.InitInventory(SetInventory);
        }
    }
    private void GetInventory(List<SlotContainer> inventory, List<SlotContainer> main, List<SlotContainer> hotbar)
    {

        StartCoroutine(GetSlot(inventory, _getUrl));
        StartCoroutine(GetSlot(main, _getUrl));
        StartCoroutine(GetSlot(hotbar, _getUrl));
    }

    private void SetInventory(List<SlotContainer> inventory, List<SlotContainer> main, List<SlotContainer> hotbar)
    {
        
        StartCoroutine(SetSlot(inventory, _setUrl));
        StartCoroutine(SetSlot(main, _setUrl));
        StartCoroutine(SetSlot(hotbar, _setUrl));
    }

    public void UpdateInventory(List<SlotContainer> slots)
    {
        StartCoroutine(SetSlot(slots, _updateUrl));
    }
    private IEnumerator GetSlot(List<SlotContainer> slots, string url)
    {

        WWWForm form = new WWWForm();
        UnityWebRequest www;

        form.AddField("id", PlayerPrefs.GetInt("PlayerID"));

        for (int i = 0; i < slots.Count; i++)
        {

            form.AddField("slot_id", i);
            form.AddField("slot_type", slots[i].GetSlotType().ToString());

            www = UnityWebRequest.Post(url, form);
            yield return www.SendWebRequest();
            List<string> webRequest = www.downloadHandler.text.Split('\t').ToList();

            string status = webRequest[0];

            webRequest.RemoveAt(0);
            Debug.Log(status);
            if (bool.Parse(status))
            {
                int id = int.Parse(webRequest[0]);
                if (id == -1)
                {
                    webRequest.Clear();
                    continue;
                }

                Item nowItem = _itemDB.GetItem(id);
                webRequest.RemoveAt(0);
                Debug.Log(nowItem);
                slots[i].UpdateSlot(nowItem, int.Parse(webRequest[0]));
                webRequest.RemoveAt(0);
            }
            else
            {
            }
        }
    }

    private IEnumerator SetSlot(List<SlotContainer> slots, string url)
    {

        WWWForm form = new WWWForm();
        UnityWebRequest www;

        form.AddField("id", PlayerPrefs.GetInt("PlayerID"));

        for (int i = 0; i < slots.Count; i++)
        {

            form.AddField("slot_id", i);
            if (slots[0].GetItem() == null)
            {
                form.AddField("item_id", -1);
            }
            else
            {
                form.AddField("item_id", slots[i].GetItem().GetInstanceID());
            }

            form.AddField("item_count", slots[i].GetAmount());
            form.AddField("slot_type", slots[i].GetSlotType().ToString());

            www = UnityWebRequest.Post(url, form);
            yield return www.SendWebRequest();
        }
    }

}
