using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyController : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private RoomController _roomController;
    [SerializeField] private GameObject _selectedLines;
    [SerializeField] private Animator _animator;
    [SerializeField] private HealthBarNPC _healthBar;
    [SerializeField] private int _moneyEarn = 100;
    private Vector3 _nextPos;
    private bool _isPosition = true;

    void Awake()
    {
        _selectedLines.SetActive(false);
        _healthBar.SetEnemyController(this);

        ChooseNextPosToPatrol();
    }

    public void GetDamage(int damage)
    {
        _healthBar.GetDamage(damage);

        StopAllCoroutines();
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

    private void ChooseNextPosToPatrol()
    {

        NavMeshHit hit;

        do {
            _nextPos = _roomController.GetNextPos();

        } while (Vector3.Distance(transform.position, _nextPos) < 6f);
        NavMesh.SamplePosition(_nextPos, out hit, 1.5f, NavMesh.AllAreas);

        _animator.SetBool("Walk", true);

        _agent.SetDestination(hit.position);

        _isPosition = false;
        StartCoroutine(NextPosMove());

        Debug.Log(_agent.isStopped);
    }

    IEnumerator NextPosMove()
    {
        Vector3 distanceToWalkPoint = transform.position - _nextPos;
        if (distanceToWalkPoint.magnitude < 1.3f)
        {

            _animator.SetBool("Walk", false);

            _agent.ResetPath();

            yield return new WaitForSeconds(2f);
            ChooseNextPosToPatrol();
        }
        else
        {
            yield return new WaitForSeconds(0.2f);

            StartCoroutine(NextPosMove());
        }
    }

    public void SetTarget(Transform target)
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            _animator.SetBool("Walk", false);

            _agent.ResetPath();

            StopAllCoroutines();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            ChooseNextPosToPatrol();
        }
    }
    public void Death()
    {
        PlayerStatistic.OnKillEnemys();
        PlayerStatistic.OnUpdateMoney(_moneyEarn);

        Destroy(this.gameObject);
    }
}

