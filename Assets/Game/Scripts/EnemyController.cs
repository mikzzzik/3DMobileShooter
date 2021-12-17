using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private RoomController _roomController;
    [SerializeField] private GameObject _selectedLines;
    [SerializeField] private HealthBarNPC _healthBar;
    private Vector3 _nextPos;
    private bool _isPosition = true;
    void Awake()
    {
        _selectedLines.SetActive(false);
        _healthBar.SetEnemyController(this);
    }

    public void GetDamage(int damage)
    {
        _healthBar.GetDamage(damage);
    }

    void Update()
    {
        Patroling();
    }

    public void SetTarget(bool status)
    {

        if (WeaponController.OnTarget != null)
        {
            if (status)
            {
                WeaponController.OnTarget(this);
            }
            else WeaponController.OnTarget(null);

        }
            
        _selectedLines.SetActive(status);
    }

    private void Patroling()
    {
        if (_isPosition)
        {
            NavMeshHit hit;
            _nextPos = _roomController.GetNextPos();
            NavMesh.SamplePosition(_nextPos, out hit, 1.5f, NavMesh.AllAreas);
            _agent.SetDestination(hit.position) ;
            _isPosition = false;
        }

        Vector3 distanceToWalkPoint = transform.position - _nextPos;

        if (distanceToWalkPoint.magnitude < 1.3f)
            _isPosition = true;

    }

    public void Death()
    {
        Destroy(this.gameObject);
    }
}
