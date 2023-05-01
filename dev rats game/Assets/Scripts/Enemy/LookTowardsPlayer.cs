using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class LookTowardsPlayer : MonoBehaviour
{
    [SerializeField] private float maximumTurnSpeed;
    
    [SerializeField] private Transform player;

    private float _offset = 0;

    private Rigidbody2D _rigidbody2D;

    private void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            var playerPos = player.position - transform.position;
            playerPos = Quaternion.Euler(0, 0, _offset) * playerPos;
            transform.up = Vector3.RotateTowards(transform.up, playerPos,
                maximumTurnSpeed * Time.deltaTime, 0);

        }

    }
}
