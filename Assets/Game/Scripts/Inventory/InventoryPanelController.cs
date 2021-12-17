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
    private SlotContainer _nowSlot;
    private int _nowSlotIndex;

    public static Action<SlotContainer> OnSetActiveSlot;

    private void OnEnable()
    {
        OnSetActiveSlot += SetActiveSlot;
    }

    private void OnDisable()
    {
        OnSetActiveSlot -= SetActiveSlot;
    }

    private void SetActiveSlot(SlotContainer slot)
    {
        _nowSlot = slot;
        FindIndex();
    }

    public void Use()
    {

    }

    public void Equipment()
    {

    }

    public void Drop()
    {
        PlayerInventory.OnDropItem(_nowSlot.GetSlotType(), _nowSlotIndex);
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
