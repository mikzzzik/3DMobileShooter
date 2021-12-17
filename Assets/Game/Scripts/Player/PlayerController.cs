using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _playerSpeed = 2.0f;
    [SerializeField] private FixedJoystick _joystick;
    [SerializeField] private Rigidbody _rigidbody;
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private Animator _animator;
    private float _gravity = 0;
    void Start()
    {

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        _gravity = 0;
        
        if (!_characterController.isGrounded) _gravity -= 9.81f * Time.deltaTime; ;
        _characterController.Move(new Vector3( _joystick.Horizontal * _playerSpeed, _gravity,  _joystick.Vertical * _playerSpeed));

        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
    
            float speed = Mathf.Abs(_joystick.Horizontal) + Mathf.Abs(_joystick.Vertical) > 1 ? 1: Mathf.Abs(_joystick.Horizontal) + Mathf.Abs(_joystick.Vertical);

            speed = speed > 0.6f ? speed : 0.6f;
            _animator.speed = speed;
            _animator.SetBool("Walk", true);
            transform.rotation = Quaternion.LookRotation(new Vector3(_joystick.Horizontal , 0, _joystick.Vertical));
        } else
        {
            _animator.SetBool("Walk", false);
            _animator.speed = 1f;
        }
    }
}
