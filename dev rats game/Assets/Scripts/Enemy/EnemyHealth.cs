using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int enemyHealth = 5;
    [SerializeField] private int resetTime = 3;

    private GameManager _gameManager;

    private void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

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
            StartCoroutine(ResetGame());
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

    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(resetTime);
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
