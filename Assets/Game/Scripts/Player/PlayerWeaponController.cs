 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerWeaponController : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private PlayerController _playerController;

    public static Action<Animator> OnShoot;
    public static Action OnReloading;
    public static Action<EnemyController> OnHit;

    private bool _boxing = false;

    private void OnEnable()
    {
        OnHit += HitEnemy;
    }

    private void OnDisable()
    {
        OnHit -= HitEnemy;
    }



    public void BeginUnEquipWeapon()
    {
        _animator.SetBool("UnEquip", true);
    }


    public void Attack()
    {
        if(_boxing) return; 
        if (OnShoot == null) return;
        OnShoot(_animator);
        
    }

    public void Boxing()
    {
        if (_animator.GetBool("Boxing")) return;
        _boxing = true;

        int index = UnityEngine.Random.Range(0, 3);

        _animator.SetFloat("BoxingType", index);

        _animator.SetBool("Boxing", true);

        

        _playerController.ChangeMoveStatus(false);
    }

    private void HitEnemy(EnemyController target)
    {
        if (_boxing)
        {
            target.GetDamage(10);
            _boxing = false;
        }
    }
    public void EndBoxing()
    {
        _animator.SetBool("Boxing", false);

        _boxing = false;

        _playerController.ChangeMoveStatus(true);
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
