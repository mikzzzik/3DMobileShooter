using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using System.Globalization;

public class MainMenuInventoryController : MonoBehaviour
{
    [SerializeField] private List<SlotContainer> _inventorySlot;
    [SerializeField] private List<SlotContainer> _mainSlot;
    [SerializeField] private List<SlotContainer> _hotbarSlot;
    [SerializeField] private Image _currentItemImg;
    [SerializeField] private Text _moneyText;
    private int _currentMoney = 0;
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

    private void SetTarget(SlotContainer slotContainer)
    {
       
        if (slotContainer == null)
        {
            if (_newSlot != null)
            {
                if (_newSlot.GetItem() == _currentItem)
                {
                    int amount = _currentItem.MaxAmount - _currentItemAmount;
                    if (_newSlot.GetAmount() + _currentItemAmount < _currentItem.MaxAmount && amount > 0)
                    {
                        Debug.Log(_newSlot.GetAmount() + " | " + _currentItemAmount + " | " + amount );
                        _newSlot.UpdateAmount(_newSlot.GetAmount() + _currentItemAmount);
                    }
                    else
                    {
                        Debug.Log(1);
                        _currentSlot.UpdateSlot(_currentItem, _currentItemAmount - (_currentItem.MaxAmount - _newSlot.GetAmount()));
                        _newSlot.UpdateAmount(_currentItem.MaxAmount);
                    }
                }
                else if(_newSlot.GetItem() == null)
                {
                    Debug.Log(2);
                    _newSlot.UpdateSlot(_currentItem, _currentItemAmount);
                }
                else
                {
                    Debug.Log(3);
                    _currentSlot.UpdateSlot(_currentItem, _currentItemAmount);
                }


            }
            else
            {
                Debug.Log(4);
                _currentSlot.UpdateSlot(_currentItem, _currentItemAmount);
            }
   
            _currentItemImg.enabled = false;
            
        }
        else
        {
            _currentSlot = slotContainer;

            _currentItem = _currentSlot.GetItem();
            _currentItemAmount = _currentSlot.GetAmount();
         
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

    public int GetMoney()
    {
        return _currentMoney;
    }

    public void AddMoney(int money)
    {
        _currentMoney += money;
        _moneyText.text = _currentMoney.ToString("C", CultureInfo.CreateSpecificCulture("en-US"));
    }

    public List<SlotContainer> GetSlots(SlotType type)
    {
        if(type == SlotType.Main)
        {
            return _mainSlot;
        }
        else if (type == SlotType.Hotbar)
        {
            return _hotbarSlot;
        }
        else
        {
            return _inventorySlot;
        }
    }

    public void SavePrefs()
    {
        ItemDataBase.OnSavePrefs(_mainSlot, _hotbarSlot);
    }
}
