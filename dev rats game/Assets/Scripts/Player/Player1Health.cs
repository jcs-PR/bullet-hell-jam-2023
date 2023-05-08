using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player1Health : MonoBehaviour
{
    [SerializeField] private int playerHealth = 5;
    private int _playerStartingHealth;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerStartingHealth = playerHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerHealth <= 0)
        {
            PlayerDeath();
        }

        if (playerHealth < 0)
        {
            playerHealth = 0;
        }
    }

    void PlayerDeath()
    {
        Destroy(this.gameObject);
    }

    public void SetPlayerHealth(float healthToSet)
    {
        playerHealth = (int) healthToSet;
    }

    public int GetPlayerStartingHealth()
    {
        return _playerStartingHealth;
    }

    public int GetPlayerCurrentHealth()
    {
        return playerHealth;
    }

    public void ReduceHealth(int healthToReduce)
    {
        playerHealth -= healthToReduce;
    }
}
