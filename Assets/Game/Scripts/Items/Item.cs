using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "Items/item", order = 1)]

public class Item : ScriptableObject
{
    public int ItemId;
    public string Name;
    public string Description;
    public int MaxAmount = 1;
    public Sprite Icon;
    public GameObject ItemObject;
    public ItemType ItemType;
    public int MinPrice;

}
public enum ItemType
{
    noneUse,
    weapon,
    ammo,
    med,
}
