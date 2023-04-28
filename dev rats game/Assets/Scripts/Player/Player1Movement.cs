using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player1Movement : MonoBehaviour
{
    private Rigidbody2D _rigidbody2D;

    private Vector2 _p1Movement;

    [FormerlySerializedAs("_p1Speed")] [SerializeField] private float p1Speed = 5;
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        GetMovementAxis();
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }
    private void GetMovementAxis()
    {
        _p1Movement.x = Input.GetAxisRaw("Horizontal");
        _p1Movement.y = Input.GetAxisRaw("Vertical");
    }

    private void MovePlayer()
    {
        Vector2 newVelocity = _rigidbody2D.velocity + _p1Movement * p1Speed;
        _rigidbody2D.velocity = newVelocity;
    }
}
