using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int enemyHealth = 5;
    [SerializeField] private int resetTime = 3;

    private GameManager _gameManager;
    [FormerlySerializedAs("_enemyPhases")] [SerializeField] private EnemyPhases enemyPhases;
    
    [SerializeField] private int phaseOneHealth;
    [SerializeField] private int phaseTwoHealth;
    [SerializeField] private int phaseThreeHealth;

    [FormerlySerializedAs("_cinemachineVirtualCamera")] 
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private void Start()
    {
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        enemyPhases.GetComponent<EnemyPhases>();
        if (enemyPhases.IsPhasingEnabled())
        {
            enemyPhases.UpdatePhase(1);
        }
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
            Destroy(cinemachineVirtualCamera.gameObject);
            Player1Health player1Health = other.gameObject.GetComponent<Player1Health>();
            player1Health.SetPlayerHealth(0);
            StartCoroutine(ResetGame());
        }
    }

    public void ReduceEnemyHealth(int healthToReduce)
    {
        enemyHealth -= healthToReduce;
    }

   public void CheckEnemyHealth()
    {
        if (enemyPhases.IsPhasingEnabled())
        {
            if (enemyHealth >= 100)
            {
                enemyPhases.UpdatePhase(0);
            }
        
            else if (enemyHealth >= 71 && enemyHealth <= 79)
            {
                enemyPhases.UpdatePhase(1);
            }
        
            else if (enemyHealth >= 31 && enemyHealth <= 70)
            {
                enemyPhases.UpdatePhase(2);
            }
            else if (enemyHealth <= 30)
            {
                enemyPhases.UpdatePhase(3);
            }

            Debug.Log("Phase Enemy Health: " + enemyHealth);
            Debug.Log("Enemy Phase: " + enemyPhases.GetCurrentPhase());
        }
        
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
