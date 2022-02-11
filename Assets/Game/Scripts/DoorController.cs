using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField] private Animator _anim;
    [SerializeField] private PlayerInteractionController _playerInteractionController;
    private void Awake()
    {
        if(null == _anim)
        {
            _anim = gameObject.GetComponent<Animator>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.tag == "Player")
        {
            if(null == _playerInteractionController)
            {
                _playerInteractionController = other.GetComponent<PlayerInteractionController>();
            }
            _playerInteractionController.DoorSet(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            _playerInteractionController.DoorDelete();
        }
    }
    public void DoorInteraction()
    {
        _anim.SetBool("Opened", !_anim.GetBool("Opened"));
    }
}
