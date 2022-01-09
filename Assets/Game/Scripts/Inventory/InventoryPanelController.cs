using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class InventoryPanelController : MonoBehaviour
{
    [SerializeField] private List<SlotContainer> _mainSlots;
    [SerializeField] private List<SlotContainer> _hotbarSlots;
    [SerializeField] private GameObject _inventoryPanel;
    [SerializeField] private GameObject _actionPanel;
    [SerializeField] private GameObject _useButton;
    [SerializeField] private GameObject _unEquipButton;
    [SerializeField] private GameObject _equipButton;
    [SerializeField] private GameObject _dropButton;

    private SlotContainer _nowSlot;
    private int _nowSlotIndex;


    private void Awake()
    {
        _actionPanel.SetActive(false);
    }

    private void OnEnable()
    {
        SlotContainer.OnSetActiveSlot += SetActiveSlot;
    }

    private void OnDisable()
    {
        SlotContainer.OnSetActiveSlot -= SetActiveSlot;
    }

    private void SetActiveSlot(SlotContainer slot)
    {
        _nowSlot = slot;

        Item nowItem = slot.GetItem();

        FindIndex();

        _actionPanel.SetActive(true);
                
        HideButtons();

        if (nowItem.ItemType == ItemType.weapon)
        {
            if(slot.GetSlotType() == SlotType.Hotbar)
            {
                _unEquipButton.SetActive(true);
            }
            else
            {
                _equipButton.SetActive(true);
            }
            
            _dropButton.SetActive(true);
        }
        else if(nowItem.ItemType == ItemType.noneUse)
        {
            _dropButton.SetActive(true);
        }
        else if(nowItem.ItemType == ItemType.med || nowItem.ItemType == ItemType.ammo)
        {
            _useButton.SetActive(true);
            _dropButton.SetActive(true);
        }
    }

    private void HideButtons()
    {
        _useButton.SetActive(false);
        _equipButton.SetActive(false);
        _unEquipButton.SetActive(false);
        _dropButton.SetActive(false);
    }

    public void Use()
    {
        PlayerInventory.OnUseItem(_nowSlot.GetSlotType(), _nowSlotIndex);
        _actionPanel.SetActive(false);
    }

    public void Equip()
    {
        PlayerInventory.OnEquipItem(_nowSlot.GetSlotType(), _nowSlotIndex);
        _actionPanel.SetActive(false);
    }

    public void UnEquip()
    {
        PlayerInventory.OnUnEquipItem(_nowSlot.GetSlotType(), _nowSlotIndex);
        _actionPanel.SetActive(false);
    }

    public void Drop()
    {
        PlayerInventory.OnDropItem(_nowSlot.GetSlotType(), _nowSlotIndex);
        _actionPanel.SetActive(false);
    }

    public void ShowPanel()
    {
        _inventoryPanel.SetActive(true);
    }

    public void ClosePanel()
    {
        _inventoryPanel.SetActive(false);
    }

    public void UpdateInventoryUI(List<Item> main, List<Item> equipment)
    {
        for(int i = 0;i < main.Count;i++)
        {
            if(i < equipment.Count)
            {
                _hotbarSlots[i].UpdateSlot(equipment[i]);
            }
            _mainSlots[i].UpdateSlot(main[i]);
        }
    }

    private void FindIndex()
    {
        if(_nowSlot.GetSlotType() == SlotType.Main)
        {
            _nowSlotIndex =  _mainSlots.IndexOf(_nowSlot);
        }
        else if (_nowSlot.GetSlotType() == SlotType.Hotbar)
        {
            _nowSlotIndex =_hotbarSlots.IndexOf(_nowSlot);
        }
    }
}
