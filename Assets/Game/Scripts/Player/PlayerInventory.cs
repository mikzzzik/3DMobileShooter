using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public enum SlotType
{ 
    Main,
    Hotbar,
}
public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private List<Item> _mainSlots;
    [SerializeField] private List<Item> _hotbarSlots;
    [SerializeField] private InventoryPanelController _inventoryPanelController;

    private Item _nowItem;

    public static Action<PickableObject> OnPickUpItem;
    public static Action<SlotType, int> OnDropItem;
    public static Action<SlotType, int> OnEquipmentItem;
    public static Action<SlotType, int> OnUseItem;
    
    
    private void OnEnable()
    {
        OnPickUpItem += PickUp;
        OnDropItem += Drop;
        OnEquipmentItem += Equipment;
        OnUseItem += Use;
    }

    private void OnDisable()
    {
        OnPickUpItem -= PickUp;
        OnDropItem -= Drop;
        OnEquipmentItem -= Equipment;
        OnUseItem -= Use;
    }
    public void PickUp(PickableObject item)
    {
        if(_mainSlots.Any(itemSlot => itemSlot == null))
        {

            for (int i = 0; i < _mainSlots.Count; i++)
            {
                if( _mainSlots[i]== null)
                {
                    _mainSlots[i]= item.GetItem();
                    break;
                }
            }
            _inventoryPanelController.UpdateInventoryUI(_mainSlots, _hotbarSlots);
            Destroy(item.gameObject);
        }
    }

    private void Use(SlotType type, int index)
    {
        GetItem(type, index);
       
    }

    private void Equipment(SlotType type, int index)
    {
        GetItem(type, index);
    }

    private void Drop(SlotType type, int index)
    {
        GetItem(type, index);

        var prefab = Instantiate(_nowItem.ItemObject);
        prefab.transform.position = transform.position+Vector3.up+this.transform.forward*0.2f;
        prefab.GetComponent<Rigidbody>().AddForce(this.transform.forward*2, ForceMode.Impulse);
        ClearSlot(type, index);
    }

    private void ClearSlot(SlotType type, int index)
    {
        if (type == SlotType.Main)
        {
            _mainSlots.RemoveAt(index);
        }
        else if (type == SlotType.Hotbar)
        {
            _hotbarSlots.RemoveAt(index);
        }
        _inventoryPanelController.UpdateInventoryUI(_mainSlots, _hotbarSlots);
    }

    private void GetItem(SlotType type, int index)
    {
        if(type == SlotType.Main)
        {
            _nowItem = _mainSlots[index];
        }
        else if (type == SlotType.Hotbar)
        {
            _nowItem = _hotbarSlots[index];
        }
    }
}
