using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "Items/Ammo", order = 1)]

public class Ammo : Item
{
    public Ñaliber AmmoType;
}

public enum Ñaliber
{
    ammo_9mm,
    ammo_5_56,
    ammo_7_62,
    ammo_12,
}
