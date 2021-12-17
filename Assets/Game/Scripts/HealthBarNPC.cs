using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBarNPC : MonoBehaviour
{
    [SerializeField] private TextMesh _textMesh;
    [SerializeField] private int _maxHealth;
    private int _nowHealth = 100;
    private EnemyController _enemyController;
    void Start()
    {
        _nowHealth = _maxHealth;
        UpdateHP(_nowHealth);
    }

    public void GetDamage(int damage)
    {
        UpdateHP(_nowHealth - damage);
    }

    private void UpdateHP(int hp)
    {
        _nowHealth = hp;

        if (_nowHealth <= 0) _enemyController.Death();
        _textMesh.text = _nowHealth.ToString() + "/" + _maxHealth.ToString();
    }

    public void SetEnemyController(EnemyController controller)
    {
        _enemyController = controller;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
