using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.Networking;

public class MarketplaceSelectedItemController : MonoBehaviour
{

    [SerializeField] private GameObject _slectedPanel;
    [SerializeField] private Text _nameText;
    [SerializeField] private InputField _priceField;
    [SerializeField] private Text _amountText;
    [SerializeField] private Image _iconImage;
    [SerializeField] private Slider _sliderAmount;
    [SerializeField] private MarketPlaceController _marketPlaceController;
    private Item _item;
    private SlotContainer _slotContainer;

    private void OnEnable()
    {
        _slectedPanel.SetActive(false);
        SlotContainer.OnSetActiveSlot += SelectItem;
    }

    private void OnDisable()
    {
        SlotContainer.OnSetActiveSlot -= SelectItem;
    }
    private void SelectItem(SlotContainer slotContainer)
    {
        _slectedPanel.SetActive(true);
        _slotContainer = slotContainer;
        _item = slotContainer.GetItem();
        _nameText.text = _item.Name;    
        _iconImage.sprite = _item.Icon;
        _priceField.text = _item.MinPrice.ToString();
        if (_item.MaxAmount > 1)
        {
            _sliderAmount.maxValue = slotContainer.GetAmount();
            _amountText.text = _sliderAmount.value.ToString();
            _sliderAmount.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            _sliderAmount.value = 1;    
            _sliderAmount.transform.parent.gameObject.SetActive(false);
        }

    }

    public void SliderChangeValue()
    {
        _amountText.text = _sliderAmount.value.ToString();
    }

    public void SellItem()
    {
        if(_slotContainer.GetAmount() > _sliderAmount.value)
        {
            _slotContainer.AddAmount((int)-_sliderAmount.value);
        }
        else
        _slotContainer.UpdateSlot(null);
        StartCoroutine(Sell());


    }

    private IEnumerator Sell()
    {

        WWWForm form = new WWWForm();
        UnityWebRequest www;

        form.AddField("id", PlayerPrefs.GetInt("PlayerID"));
        form.AddField("slot_id", _marketPlaceController.GetSlotIndex(_slotContainer));
        form.AddField("item_id", _item.ItemId);
        form.AddField("item_count", _sliderAmount.value.ToString());
        form.AddField("item_price", _priceField.text);

        www = UnityWebRequest.Post("http://m94820.hostua01.fornex.org/sql/MarketplaceSellItem.php", form);

        _slectedPanel.SetActive(false);

        yield return www.SendWebRequest();
        _marketPlaceController.SaveInventory();
    }
}
