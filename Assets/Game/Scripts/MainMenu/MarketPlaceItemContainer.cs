using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
public class MarketPlaceItemContainer : MonoBehaviour
{
    [SerializeField] private Text _nameText;
    [SerializeField] private Text _descriptionText;
    [SerializeField] private Text _priceText;
    [SerializeField] private Text _amountText;
    [SerializeField] private Image _iconImage;
    [SerializeField] private MainMenuInventoryController _mainMenuInventoryController;
    private Item _item;
    private int _playerId;
    private int _amount;
    private int _price;
    private int _id;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetItem(int id, Item item, int idPlayer, int amount, int price, MainMenuInventoryController controller)
    {
        _id = id;
        _item = item;
        _playerId = idPlayer;
        _amount = amount;
        _price = price;
        Debug.Log(item);
        _nameText.text = item.Name;
        _descriptionText.text = item.Description;

        _priceText.text = price.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
        _amountText.text = amount.ToString();

        _iconImage.sprite = item.Icon;
        _mainMenuInventoryController = controller;

    }

    public void Buy()

    {
        if (_mainMenuInventoryController.GetMoney() < _price) return;
        StartCoroutine(BuyRequest());

    }
    private IEnumerator BuyRequest()
    {

        List<SlotContainer> slots = _mainMenuInventoryController.GetSlots(SlotType.Inventory);
        int index = -1;
        bool haveSlot = false;
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].GetItem() == null)
            {
                index = i;
                haveSlot = true;
                break;
            }
        }
        if (!haveSlot) yield return null;
        WWWForm form = new WWWForm();
        UnityWebRequest www;

        form.AddField("id", PlayerPrefs.GetInt("PlayerID"));
        form.AddField("marketplace_id", _id);
        form.AddField("idSeller", _playerId);
        form.AddField("item_id", _item.GetInstanceID());
        form.AddField("item_count", _amount);
        form.AddField("item_price", _price);
        form.AddField("slot_id", index);

        www = UnityWebRequest.Post("http://m94820.hostua01.fornex.org/sql/MarketplaceBuyItem.php", form);
        yield return www.SendWebRequest();
        List<string> webRequest = www.downloadHandler.text.Split('\t').ToList();
        for(int i =0; i < webRequest.Count;i++)
        {
            Debug.Log(webRequest[i]);
        }
        string status = webRequest[0];

        webRequest.RemoveAt(0);
        Debug.Log(status);
        if (bool.Parse(status))
        {
            _mainMenuInventoryController.AddMoney(-_price);
            slots[index].UpdateSlot(_item, _amount);
            Destroy(gameObject);

        }
        else
        {
            Debug.Log(webRequest[0]);
            MarketPlaceController.OnLoadMarketplace();
        }
    }

}

