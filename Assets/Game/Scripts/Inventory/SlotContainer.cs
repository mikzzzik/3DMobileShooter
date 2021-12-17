using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SlotContainer : EventTrigger
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _item_text_count;
    [SerializeField] private SlotType _slotType;
    private Item _item;
    public void UpdateSlot(Item item)
    {
        Debug.Log("gfdg");
        if (item != null)
        {
            _item = item;
            _itemImage.overrideSprite = _item.Icon;
            if(_item.MaxAmmount == 1)
            {
                _item_text_count.enabled = false;
            }
            else _item_text_count.enabled = true;
            _item_text_count.text = _item.Ammount.ToString();
            _itemImage.enabled = true;
        }
        else
        {
            _item_text_count.enabled = false;
            _item_text_count.text = "0";
            _itemImage.enabled = false;
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);
        InventoryPanelController.OnSetActiveSlot(this);
    }


    public Item GetItem()
    {
        return _item;
    }

    public SlotType GetSlotType()
    {
        return _slotType;
    }
}
