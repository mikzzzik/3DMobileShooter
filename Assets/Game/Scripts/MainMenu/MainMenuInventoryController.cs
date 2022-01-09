using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class MainMenuInventoryController : MonoBehaviour
{
    [SerializeField] private List<SlotContainer> _inventorySlot;
    [SerializeField] private List<SlotContainer> _mainSlot;
    [SerializeField] private List<SlotContainer> _hotbarSlot;
    [SerializeField] private Item _item;
    [SerializeField] private Image _currentItemImg;
    [SerializeField] private Canvas _canvas;
    private Item _currentItem;
    private int _currentItemAmount;
    private SlotContainer _currentSlot;
    private SlotContainer _newSlot;
    private void OnEnable()
    {
        SlotDrag.OnSetTargetItem += SetTarget;
        SlotDrag.OnDragitem += DragItem;
        SlotDrag.OnSetNewSlot += SetNewSlot;
    }

    private void OnDisable()
    {
        SlotDrag.OnSetTargetItem -= SetTarget;
        SlotDrag.OnDragitem -= DragItem;
        SlotDrag.OnSetNewSlot -= SetNewSlot;
    }
    private void Awake()
    {
        
    }

    public void InitInventory(Action<List<SlotContainer>,List<SlotContainer>,List<SlotContainer>> action)
    {
        Debug.Log(_inventorySlot.Count);
        action(_inventorySlot, _mainSlot,_hotbarSlot);

    }
    
    public void LoadInventory(Action<List<SlotContainer>, List<SlotContainer>, List<SlotContainer>> action)
    {
        action(_inventorySlot, _mainSlot, _hotbarSlot);
    }

    public void UpdateInventory()
    {
        MainMenuController.OnUpdateSlots(_mainSlot);
        MainMenuController.OnUpdateSlots(_inventorySlot);
        MainMenuController.OnUpdateSlots(_hotbarSlot);
    }

    void Start()
    {
        _inventorySlot[0].UpdateSlot(_item);
        Debug.Log(_item.GetInstanceID());
    }

    private void SetTarget(SlotContainer slotContainer)
    {
        Debug.Log(_currentItem + " | " + _currentItemAmount);
        Debug.Log(_newSlot);
       
        if (slotContainer == null)
        {
            if(_newSlot != null)
            {
                _newSlot.UpdateSlot(_currentItem, _currentItemAmount);
                
            }
            else
            {
                _currentSlot.UpdateSlot(_currentItem, _currentItemAmount);
            }
            _currentItemImg.enabled = false;
        }
        else
        {
            _currentSlot = slotContainer;

            _currentItem = _currentSlot.GetItem();
            _currentItemAmount = _currentSlot.GetAmount();
            Debug.Log(_currentItem + " | " + _currentItemAmount);
            _currentItemImg.enabled = true;
            _currentItemImg.overrideSprite = slotContainer.GetItem() .Icon;
        }
    }

    private void SetNewSlot(SlotContainer newSlot)
    {
        _newSlot = newSlot;
    }

    private void DragItem(Vector3 pos)
    {
        _currentItemImg.transform.position = pos;
    }

    private void OnDestroy()
    {

    }

    public void SavePrefs()
    {
        ItemDataBase.OnSavePrefs(_mainSlot, _hotbarSlot);
    }
}
