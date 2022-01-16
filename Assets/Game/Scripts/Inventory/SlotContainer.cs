using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class SlotContainer : EventTrigger
{
    [SerializeField] private Image _itemImage;
    [SerializeField] private TextMeshProUGUI _item_text_count;
    [SerializeField] private SlotType _slotType;
    private Item _item;
    private int _amount = 0;
    private bool _equipment;

    public static Action<SlotContainer> OnSetActiveSlot;

    public void UpdateSlot(Item item, int amount = 1)
    {
        if (item != null)
        {
            _item = item;
            _itemImage.overrideSprite = _item.Icon;
            if(_item.MaxAmount == 1)
            {
                _item_text_count.enabled = false;
            }
            else _item_text_count.enabled = true;

            _amount = amount;

            _item_text_count.text = _amount.ToString();

            _itemImage.enabled = true;

            if(_slotType == SlotType.Hotbar)
            {
                _equipment = true;
            }
        }
        else
        {
            ClearSlot();

            _item_text_count.enabled = true;
            
            _item_text_count.text = "0";

            _itemImage.enabled = false;

            if (_slotType == SlotType.Hotbar)
            {
                _equipment = false;
            }
        }
    }

    public int AddAmount(int amount)
    {
        int tempAmount = 0;
        if (_amount + amount > _item.MaxAmount)
        {
            tempAmount = amount - (_item.MaxAmount - _amount);
            _amount = _item.MaxAmount;
        }
        else
        {
            _amount += amount;
        }

        _item_text_count.text = _amount.ToString();

        return tempAmount;
    }
    public void UpdateAmount(int amount)
    {
        _amount = amount;

        _item_text_count.text = _amount.ToString();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (_item == null) return;
        base.OnPointerClick(eventData);
        OnSetActiveSlot(this);
    }

    private void ClearSlot()
    {
        _item = null;
        _amount = 0;
    }

    public Item GetItem()
    {
        return _item;
    }

    public int GetAmount()
    {
        return _amount;
    }

    public SlotType GetSlotType()
    {
        return _slotType;
    }

    public bool GetEquipment()
    {
        return _equipment;
    }
}
