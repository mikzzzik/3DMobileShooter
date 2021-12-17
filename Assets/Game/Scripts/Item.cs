using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "Items/item", order = 1)]
public class Item : ScriptableObject
{
    public string Name;
    public string Description;
    public int MaxAmmount = 1;
    public int Ammount = 1;
    public Sprite Icon;
    public GameObject ItemObject;
    public bool Equipment;

}
