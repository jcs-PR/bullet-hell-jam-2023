using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int enemyHealth = 5;
    [SerializeField] private int resetTime = 3;


    private void Update()
    {
        DestroyEnemy();
    }

    void DestroyEnemy()
    {
        if (enemyHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player1Health player1Health = other.gameObject.GetComponent<Player1Health>();
            player1Health.SetPlayerHealth(0);
        }
    }

    public void ReduceEnemyHealth(int healthToReduce)
    {
        enemyHealth -= healthToReduce;
    }

    public void SetEnemyHealth(int healthToSet)
    {
        enemyHealth = healthToSet;
    }

    public int GetEnemyHealth()
    {
        return enemyHealth;
    }
}
