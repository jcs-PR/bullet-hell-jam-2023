using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player1Movement : MonoBehaviour
{
    [FormerlySerializedAs("_p1Speed")] [SerializeField] private float p1Speed = 5f;
    [Tooltip("Amount of time takes input to transition")]
    [Range(0f, 2f)]
    [FormerlySerializedAs("_smoothSpeed")] [SerializeField] private float smoothSpeed = 0.5f;

    Rigidbody2D _rigidbody2D;
    Vector2 targetInput;
    Vector2 currentInput;
    Vector2 smoothVelocity;

    void Start() => _rigidbody2D = GetComponent<Rigidbody2D>();

    void Update() => SmoothInput();

    void FixedUpdate() => MovePlayer();

    private void GetMovementAxis()
    {
        targetInput.x = Input.GetAxisRaw("Horizontal");
        targetInput.y = Input.GetAxisRaw("Vertical");
    }

    private void SmoothInput()
    {
        GetMovementAxis();
        currentInput = Vector2.SmoothDamp(currentInput, targetInput,
            ref smoothVelocity, smoothSpeed);
    }

    private void MovePlayer() => _rigidbody2D.velocity = currentInput * p1Speed * Time.deltaTime;

}
