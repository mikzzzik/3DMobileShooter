﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class WeaponController : MonoBehaviour
{
    [SerializeField] private Weapon _weapon;
    [SerializeField] private ParticleSystem _particle;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private WeaponType _weaponType;
    private int _ammo = 0;
    private int _currentAmmo = -1;
    private bool _canShoot = true;

    private EnemyController _target;

    public static Action<EnemyController> OnTarget;
    public static Action OnWeaponHide;
    private void Awake()
    {
       
        
    }
    private void OnEnable()
    {
        PlayerWeaponController.OnShoot += Shoot;
        PlayerWeaponController.OnReloading += Reloading;
        OnWeaponHide += HideWeapon;
        OnTarget += SetTarget;

        if (HudController.OnWeaponActive != null)
            HudController.OnWeaponActive(true);

        if (_currentAmmo == -1)
        {
            _currentAmmo = _weapon.MaxLoaded;
            _ammo = _currentAmmo * 2;
        }

        if (HudController.OnAmmoUpdate != null)
            HudController.OnAmmoUpdate(_currentAmmo, _ammo);
    }

    private void OnDisable()
    {
        PlayerWeaponController.OnShoot -= Shoot;
        PlayerWeaponController.OnReloading -= Reloading;
        OnWeaponHide -= HideWeapon;
        OnTarget -= SetTarget;
        if (HudController.OnWeaponActive != null)
            HudController.OnWeaponActive(false);
    }

    private void HideWeapon()
    {
        this.gameObject.SetActive(false);
    }

    private void SetTarget(EnemyController target)
    {
        _target = target;
    }

    private void Shoot()
    {
        if (!_canShoot) return;
        if (_currentAmmo <= 0)
        {
            PlayAudio(_weapon.NoAmmoAudio);
        }
        else
        {
            _currentAmmo--;
            if (_target != null)
                _target.GetDamage(_weapon.Damage);
            StartCoroutine(Shooted());
            _particle.Play();
            PlayAudio(_weapon.ShootAudio);
            if (HudController.OnAmmoUpdate != null)
                HudController.OnAmmoUpdate(_currentAmmo, _ammo);
        }

    }
    public int AddAmmo(int ammo)
    {
        int tempAmmo;

        Debug.Log(_ammo);

        if (_ammo + ammo > _weapon.MaxAmmo)
        {
     
            tempAmmo = ammo - (_weapon.MaxAmmo - _ammo);
            _ammo = _weapon.MaxAmmo; 
        }
        else
        {
            _ammo += ammo;
            tempAmmo =  0;
        }

        Debug.Log(_ammo + "|" + tempAmmo);
        if (HudController.OnAmmoUpdate != null)
            HudController.OnAmmoUpdate(_currentAmmo, _ammo);

        return tempAmmo;
    }

    IEnumerator Shooted()
    {
        _canShoot = false;

        yield return new WaitForSeconds((float)60 / _weapon.FireRate);
        _canShoot = true;
    }

    private void PlayAudio(AudioClip clip)
    {
        _audioSource.clip = clip;
        _audioSource.Play();
    }

    private void Reloading()
    {
        if (_ammo == 0 || _currentAmmo == _weapon.MaxLoaded) return;
        if (_weapon.MaxLoaded < _currentAmmo+_ammo)
        {
            _ammo -= (_weapon.MaxLoaded - _currentAmmo);
            _currentAmmo = _weapon.MaxLoaded;
        }
        else
        {
            _currentAmmo += _ammo;
            _ammo = 0;
        }
        if (HudController.OnAmmoUpdate != null)
            HudController.OnAmmoUpdate(_currentAmmo, _ammo);
    }

    public WeaponType GetWeaponType()
    {
        return _weaponType;
    }
}