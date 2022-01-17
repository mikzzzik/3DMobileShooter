using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.Linq;
using UnityEngine.UI;
using System.Globalization;
using System;

public class MarketPlaceController : MonoBehaviour
{
    [SerializeField] private Transform _viewport;
    [SerializeField] private GameObject _containerPrefab;
    [SerializeField] private MarketPlaceItemContainer _itemPrefab;
    [SerializeField] private MainMenuInventoryController _inventoryController;
    [SerializeField] private List<SlotContainer> _slots;
    [SerializeField] private ItemDataBase _itemData;
    [SerializeField] private ScrollRect _scrollRect;
    [SerializeField] private Text _moneyText;
    private int _currentMoney;
    private RectTransform _container;
    private int index;

    public static Action OnLoadMarketplace;

    private void OnEnable()
    {
        LoadSlots();
        UpdateMoney();
        _container = Instantiate(_containerPrefab, _viewport).transform as RectTransform;
        _scrollRect.content = _container;
        LoadMarketItem();
        OnLoadMarketplace += LoadMarketItem;
    }
  

    private void OnDisable()
    {
        OnLoadMarketplace -= LoadMarketItem;
        Destroy(_container.gameObject);
    }
    private void LoadSlots()
    {
        List<SlotContainer> slots = _inventoryController.GetSlots(SlotType.Inventory);
        for(int i =0; i < slots.Count; i++)
        {
            if (slots[i] != null)
                _slots[i].UpdateSlot(slots[i].GetItem(), slots[i].GetAmount());
            
        }
    }

    public List<SlotContainer> GetSlots()
    {
        return _slots;
    }

    public void LoadMarketItem()
    {
        StartCoroutine(LoadMarketplaceItem());
        Destroy(_container.gameObject);
        _container = Instantiate(_containerPrefab, _viewport).transform as RectTransform;
        _scrollRect.content = _container;

    }

    public void UpdateMoney()
    {
        _currentMoney = _inventoryController.GetMoney();
        _moneyText.text = _currentMoney.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
    }    

    public void SaveInventory()
    {
        ItemDataBase.OnUpdateInventory(_slots);
        _inventoryController.GetSlots(SlotType.Inventory)[index].UpdateSlot(_slots[index].GetItem(), _slots[index].GetAmount());
   
    }
    private IEnumerator LoadMarketplaceItem()
    {
        WWWForm form = new WWWForm();
        UnityWebRequest www;

/*        form.AddField("id", PlayerPrefs.GetInt("PlayerID"));
        form.AddField("slot_id", _marketPlaceController.GetSlotIndex(_slotContainer));
        form.AddField("item_id", _item.GetInstanceID());
        form.AddField("item_count", _sliderAmount.value.ToString());
        form.AddField("item_price", _priceField.text);
*/

        www = UnityWebRequest.Post("http://m94820.hostua01.fornex.org/sql/MarketplaceGetItemAmount.php", form);
        yield return www.SendWebRequest();

        int amount = int.Parse(www.downloadHandler.text);

        if (amount > 0)
        {
            www = UnityWebRequest.Post("http://m94820.hostua01.fornex.org/sql/MarketPlaceGetItem.php", form);
            yield return www.SendWebRequest();
            List<string> webRequest = www.downloadHandler.text.Split('\t').ToList();

            string status = webRequest[0];
            
            webRequest.RemoveAt(0);
            Debug.Log(status);
            if (bool.Parse(status))
            {
               
                _container.sizeDelta = new Vector2(0, 200 * (webRequest.Count / 5));
        
                for (int i = 0 ; i < webRequest.Count/5; i++)
                {
                    int Id = int.Parse(webRequest[5 * i ]);
                    Item item = _itemData.GetItem(int.Parse(webRequest[5 * i +1]));
                    Debug.Log(webRequest[5 * i] + "|" + webRequest[5 * i + 1]);
                    Debug.Log(item);
                    int playerId = int.Parse(webRequest[5 * i +2]);
                    int amountItem = int.Parse(webRequest[5 * i + 3]);
                    int priceItem = int.Parse(webRequest[5 * i + 4]);
                    InstanceNewItem(Id,item, playerId, amountItem, priceItem);
                   
                }
            }
        }
    }

    public int GetSlotIndex(SlotContainer slots)
    {
        index = _slots.IndexOf(slots);
        return index;
   
    }

    private void InstanceNewItem(int id ,Item item,int playerId, int amount, int price)
    {
        MarketPlaceItemContainer marketPlaceItemContainer = Instantiate(_itemPrefab,_container.transform) as MarketPlaceItemContainer;
        marketPlaceItemContainer.SetItem(id,item, playerId, amount, price, _inventoryController,this);
    }
}
