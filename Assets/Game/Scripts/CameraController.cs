using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Transform _parent;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _parent.position = new Vector3(_target.position.x, _parent.position.y, _target.transform.position.z);
      
     //   _parent.eulerAngles = new Vector3(_parent.eulerAngles.x, _target.eulerAngles.y, _parent.eulerAngles.z);
    }
}
