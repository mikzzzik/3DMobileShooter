    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Data", menuName = "Items/weapon", order = 1)]

public class Weapon : Item
{
    public int MaxAmmo;
    public int MaxLoaded;
    public int FireRate;
    public int Damage;
    public bool AutoFire;
    public int ReloadingTime;
    public RuntimeAnimatorController AnimatorController;
    public AudioClip ShootAudio;
    public AudioClip NoAmmoAudio;
    public AudioClip ReloadAudio;
    public WeaponType ItemWeaponType;
    public Сaliber AmmoType;
}

public enum WeaponType
{
    beretta_m92,
}