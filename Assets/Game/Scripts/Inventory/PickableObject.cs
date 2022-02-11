using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PickableObject : MonoBehaviour
{
    [SerializeField] private Item _item;
    [SerializeField] private int _amount = 1;
    void Start()
    {
        if(_amount > _item.MaxAmount)
        {
            _amount = _item.MaxAmount;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            PlayerInventory.OnPickUpItem(this);
            PlayerStatistic.OnUpdateItemTake();
        }
    }

    public Item GetItem()
    {
        return _item;
    }

    public void UpdateAmount(int amount)
    {
        _amount = amount;
    }

    public int GetAmount()
    {
        return _amount;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.name);
        PlayerInventory.OnItemOnTriggerEnter(this);
    }

    private void OnTriggerExit(Collider other)
    {

        PlayerInventory.OnItemOnTriggerExit(this);
    }
}
