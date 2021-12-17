using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomController : MonoBehaviour
{
    [SerializeField] private List<Transform> _areaList;
    [SerializeField] private List<Transform> _areaToCalc;
    void Start()
    {
        _areaToCalc.InsertRange(0, _areaList);
        _areaToCalc.Add(_areaList[0]);
    }
    
    void Update()
    {
        
    }

    private void OnDrawGizmos()
    {
  

        for (int i = 0; i < _areaList.Count - 1; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_areaList[i].position, _areaList[i + 1].position);

            /*if (i < _areaList.Count - 2)
            {
                Gizmos.color = Color.grey;
                Gizmos.DrawLine(_areaList[i].position, _areaList[i + 2].position);
            }*/
        }
        Gizmos.DrawLine(_areaList[_areaList.Count-1].position, _areaList[0].position);
    }

    public Vector3 GetNextPos()
    {
        float x = Random.Range(_areaList[1].position.x*100, _areaList[2].position.x * 100);
        float z = Random.Range(_areaList[0].position.z * 100, _areaList[1].position.z * 100);
        Vector3 nextPos;
        nextPos = new Vector3(x/100,0, z/100);
    
        return nextPos;
    }

}
