using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
public enum SlotType
{ 
    Main,
    Hotbar,
    Inventory,
}
public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private List<SlotContainer> _mainSlots;
    [SerializeField] private List<SlotContainer> _hotbarSlots;
    [SerializeField] private InventoryPanelController _inventoryPanelController;
    [SerializeField] private List<WeaponController> _weaponControllerList;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerWeaponController _playerWeaponController;
    [SerializeField] private ItemDataBase _itemDB;
    private Item _nowItem;
    private SlotContainer _nowSlot;
    private WeaponController _activeWeapon;

    public static Action<PickableObject> OnPickUpItem;
    public static Action<SlotType, int> OnDropItem;
    public static Action<SlotType, int> OnEquipItem;
    public static Action<SlotType, int> OnUnEquipItem;
    public static Action<SlotType, int> OnUseItem;
    
    
    private void OnEnable()
    {
        OnPickUpItem += PickUp;
        OnDropItem += Drop;
        OnEquipItem += Equip;
        OnUnEquipItem += UnEquip;
        OnUseItem += Use;

        for (int i = 0; i < _mainSlots.Count; i++)
        {
            int id = PlayerPrefs.GetInt("MainItemIndex_" + i);
            if (id != -1)
            {
                Item nowItem = _itemDB.GetItem(id);
                _mainSlots[i].UpdateSlot(nowItem, PlayerPrefs.GetInt("MainItemAmount_" + i));

            }
        }
        for (int i = 0; i < _hotbarSlots.Count; i++)
        {
            int id = PlayerPrefs.GetInt("HotbarItemIndex_" + i);
            Debug.Log(id);
            if (id != -1)
            {
                Item nowItem = _itemDB.GetItem(id);
                _hotbarSlots[i].UpdateSlot(nowItem, PlayerPrefs.GetInt("HotbarItemAmount_" + i));
                if (i == 0)
                    Equip(SlotType.Hotbar, 0);
            }
        }
    }

    private void OnDisable()
    {
        OnPickUpItem -= PickUp;
        OnDropItem -= Drop;
        OnEquipItem -= Equip;
        OnUnEquipItem -= UnEquip;
        OnUseItem -= Use;

        ItemDataBase.OnSavePrefs(_mainSlots, _hotbarSlots);
    }

    public void PickUp(PickableObject item)
    {

        SlotContainer emptySlot = null;
        for (int i = 0; i < _mainSlots.Count; i++)
        {


            if (_mainSlots[i].GetItem() == null && emptySlot == null)
                emptySlot = _mainSlots[i];
            if (item.GetItem() == _mainSlots[i].GetItem())
            {
                item.UpdateAmount(_mainSlots[i].AddAmount(item.GetAmount()));

                if (item.GetAmount() == 0)
                {
                    Destroy(item.gameObject);

                    return;
                }
            }
        }
        Destroy(item.gameObject);
        emptySlot.UpdateSlot(item.GetItem(), item.GetAmount());
    }

    private void Use(SlotType type, int index)
    {
        GetItem(type, index);

        if (_nowItem == null) return;

        if(_nowItem.ItemType == ItemType.ammo)
        {
            UseAmmo(type, index);
        }

        _nowItem = null;
    }

    private void UseAmmo(SlotType type, int index)
    {
        Ammo ammo = _nowItem as Ammo;
        if(_hotbarSlots[0] != null)
        {
            Debug.Log(ammo.AmmoType);
           if ((_hotbarSlots[0].GetItem() as Weapon).AmmoType == ammo.AmmoType)
            {
                _nowSlot.UpdateAmount(_activeWeapon.AddAmmo(_nowSlot.GetAmount()));

                if(_nowSlot.GetAmount() == 0)
                {
                    ClearSlot(type, index);
                }
            }
        }
    }

    private void Equip(SlotType type, int index)
    {
        GetItem(type, index);
        if (_nowItem == null) return;
        if (_nowItem.ItemType == ItemType.weapon)
        {
            ClearSlot(type, index);
           
            EquipWeapon();
       
        }

        _nowItem = null;
        _nowSlot = null;
    }

    private void UnEquip(SlotType type, int index)
    {
        GetItem(type, index);
        if (_nowItem == null) return;

        UnEquipWeapon(index);
        _nowItem = null;
        _nowSlot = null;
    }

    private void Drop(SlotType type, int index)
    {
        GetItem(type, index);

       

        if (_nowItem == null) return;

        if(type == SlotType.Hotbar)
        {
            WeaponController.OnWeaponHide();
            _playerController.ChangeAnimatorController(null);
        }

        var prefab = Instantiate(_nowItem.ItemObject);
        
        prefab.transform.position = transform.localPosition+Vector3.up+this.transform.forward*0.2f;
        prefab.GetComponent<Rigidbody>().AddForce(this.transform.forward*2, ForceMode.Impulse);

        ClearSlot(type, index);

        _nowItem = null;
        _nowSlot = null;
    }

    private void EquipWeapon()
    {
        for(int i = 0; i < _hotbarSlots.Count;i++)
        {
            if(_hotbarSlots[i].GetItem() == null || _nowSlot.GetSlotType() == SlotType.Hotbar)
            {
                _hotbarSlots[i].UpdateSlot(_nowItem);

                Weapon _nowWeapon = _nowItem as Weapon;
           
                for(int j = 0; j <_weaponControllerList.Count;j++)
                {
                    if(_weaponControllerList[j].GetWeaponType() == _nowWeapon.ItemWeaponType)
                    {

                        _activeWeapon = _weaponControllerList[j];
                        _activeWeapon.gameObject.SetActive(true);
                        
                        _playerController.ChangeAnimatorController(_nowWeapon.AnimatorController);
                    }
                }
               // _inventoryPanelController.UpdateInventoryUI(_mainSlots, _hotbarSlots);
            }
        }
    }

    private void UnEquipWeapon(int index)
    {
        _playerWeaponController.BeginUnEquipWeapon();
        for (int i = 0; i < _mainSlots.Count; i++)
        {
            if (_mainSlots[i].GetItem() == null)
            {
                _mainSlots[i].UpdateSlot(_nowItem);
                ClearSlot(SlotType.Hotbar, index);
                
                return;
            }
        }
    }



    private void ClearSlot(SlotType type, int index)
    {
        if (type == SlotType.Main)
        {
            _mainSlots[index].UpdateSlot(null);
        }
        else if (type == SlotType.Hotbar)
        {
            _hotbarSlots[index].UpdateSlot(null);
        }
      //  _inventoryPanelController.UpdateInventoryUI(_mainSlots, _hotbarSlots);
        
    }

    private void GetItem(SlotType type, int index)
    {
        if(type == SlotType.Main)
        {
            
            _nowSlot = _mainSlots[index];
            
        }
        else if (type == SlotType.Hotbar)
        {
            _nowSlot = _hotbarSlots[index];
        }

        _nowItem = _nowSlot.GetItem();
    }
}
