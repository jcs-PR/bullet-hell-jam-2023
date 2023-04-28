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
        
    }

    void PlayerDeath()
    {
        
    }
}
