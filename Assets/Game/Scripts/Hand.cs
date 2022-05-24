using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        if (other.tag == "Enemy")
        {
            if (PlayerWeaponController.OnHit != null)
                PlayerWeaponController.OnHit(other.GetComponent<EnemyController>());
        }
    }
}
