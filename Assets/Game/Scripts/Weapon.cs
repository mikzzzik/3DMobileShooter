using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "Items/weapon", order = 1)]

public class Weapon : Item
{
    public int MaxAmmo;
    public int FireRate;
    public int Damage;
    public bool AutoFire;
    public int ReloadingTime;
    public AudioClip ShootAudio;
    public AudioClip NoAmmoAudio;
    public AudioClip ReloadAudio;
}
