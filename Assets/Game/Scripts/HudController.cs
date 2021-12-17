using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
public class HudController : MonoBehaviour
{

    [Header("Weapons")]
    [SerializeField]private GameObject _weaponHud;
    [SerializeField] private TextMeshProUGUI _ammoText;

    public static Action<int,int> OnAmmoUpdate;
    public static Action<bool> OnWeaponActive;
    private void OnEnable()
    {
        OnAmmoUpdate += UpdateAmmo;
        OnWeaponActive += ChangeWeaponHudStatus;
    }

    private void OnDisable()
    {
        OnAmmoUpdate -= UpdateAmmo;
        OnWeaponActive -= ChangeWeaponHudStatus;
    }

    private void ChangeWeaponHudStatus(bool status)
    {
        _weaponHud.SetActive(status);
    }

    private void UpdateAmmo(int current, int max)
    {
        _ammoText.text = current.ToString() + "/" + max.ToString();
    }
}
