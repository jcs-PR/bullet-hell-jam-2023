using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class EnemyMovement : MonoBehaviour
{
    [FormerlySerializedAs("_rigidbody2D")] [SerializeField] private Rigidbody2D rigidbody2D;

    private float _enemyTimer = 0;
    [SerializeField] private float startingEnemyTimer = 10;
    
    [SerializeField] private float minimumMovement = 1;
    [SerializeField] private float maximumMovement = 3;

    [SerializeField] private float movementSpeed = 1f;
    // Start is called before the first frame update
    void Start()
    {
        _enemyTimer = startingEnemyTimer;
    }

    private void Update()
    {
        if (_enemyTimer > 0)
        {
            _enemyTimer -= Time.deltaTime;
        }
        else
        {
            MoveEnemy();
        }
    }

    private void MoveEnemy()
    {
        Vector2 currentPosition = rigidbody2D.position;
       Vector2 newPosition = new Vector2(Random.Range(minimumMovement, maximumMovement), Random.Range(minimumMovement, maximumMovement)); // Placeholder, to be revised. 
       rigidbody2D.MovePosition(currentPosition + newPosition * movementSpeed * Time.deltaTime);
       _enemyTimer = startingEnemyTimer;
    }
    
    
    
}
