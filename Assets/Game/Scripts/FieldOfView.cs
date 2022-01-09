using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FieldOfView : MonoBehaviour {

    [SerializeField] private int _countLines = 30;
    [Range(0,90)]
    [SerializeField] private int _angle;
    [Range(0, 20)]
    [SerializeField] private float _range = 1;
    private RaycastHit hit;

    private EnemyController _target;
    private void Start()
    {
        
    }

    private void Update()
    {
        
    }
    
   
    
    private void OnDrawGizmos()
    {
      // Handles.DrawWireArc(transform.position, Vector3.up,Vector3.forward,360,_range);
        Gizmos.DrawLine(transform.position, transform.position + GetDir(_angle) * _range);
        Gizmos.DrawLine(transform.position, transform.position + GetDir(-_angle) * _range);
        float step = _angle*2 / (float)_countLines;
      
        for(int i = 0; i < _countLines;i++)
        {
        //    Gizmos.DrawLine(transform.position, transform.position + new Vector3(Mathf.Sin((-_angle + step * i) * Mathf.Deg2Rad), 0, 1) * _range);
        }
    }

    private Vector3 GetDir(float viewAngle)
    {
        viewAngle += transform.eulerAngles.y;
        return new Vector3(Mathf.Sin(viewAngle * Mathf.Deg2Rad), 0, Mathf.Cos(viewAngle * Mathf.Deg2Rad));
    }

    private void FixedUpdate()
    {
        EnemyController tempTarget = null;
        float step = _angle * 2 / (float)_countLines;
        bool getTarget = false;
        for (int i = 0; i < _countLines; i++)
        {
          if(Physics.Raycast(transform.position,  GetDir(_angle-step*i) * _range,out hit, _range))
            {
              
                if (hit.transform.tag == "Enemy" )
                {
                   
                    if (_target == null)
                    {
                        _target = hit.transform.gameObject.GetComponent<EnemyController>();
                        _target.SetTarget(true);
                        getTarget = true;
                    }
                    else
                    {
                        if (_target == hit.transform.gameObject.GetComponent<EnemyController>())
                        {
                            tempTarget = null;
                            getTarget = true;
                            break;
                        }
                        else
                        {
                            tempTarget = hit.transform.gameObject.GetComponent<EnemyController>();
                        }
                    }
                }

            }
            Debug.DrawRay(transform.position,  GetDir(_angle - step * i) * _range, Color.red);
        }
        if (!getTarget && _target != null)
        {
            _target.SetTarget(false);
            _target = null;
        }

        if(tempTarget != null)
        {
            tempTarget.SetTarget(true);
        }


    }


}
