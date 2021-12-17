using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour
{
    [SerializeField] private Item _item;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        PlayerInventory.OnPickUpItem(this);
    }

    public Item GetItem()
    {
        return _item;
    }
}
