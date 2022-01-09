using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerController _playerController;
    public static Action OnShoot;
    public static Action OnReloading;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }
    
    public void BeginUnEquipWeapon()
    {
        _animator.SetBool("UnEquip", true);
    }

    public void Attack()
    {
        if (OnShoot == null) return;
        OnShoot();
        _animator.SetBool("Attack", true);
    }
    public void UnEquipWeapon()
    {
        _animator.SetBool("UnEquip", false);
        _playerController.ChangeAnimatorController(null);
        WeaponController.OnWeaponHide();
    }

    public void EndAttack()
    {
        _animator.SetBool("Attack", false);
    }

    public void Reloading()
    {
        if (OnReloading == null) return;
        OnReloading();
       
    }
}
