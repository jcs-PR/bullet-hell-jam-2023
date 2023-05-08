using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Player1Movement : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [FormerlySerializedAs("_p1Speed")][SerializeField] private float p1Speed = 500f;
    [Range(0f, 2f)]
    [Tooltip("Amount of time takes input to transition")]
    [FormerlySerializedAs("_smoothSpeed")]
    [SerializeField] private float inputSmoothing = 0.2f;
    [Range(0f, 2f)]
    [FormerlySerializedAs("_dashSmoothing")]
    [SerializeField] private float dashSmoothing = 0.1f;
    [FormerlySerializedAs("_dashLength")]
    [SerializeField] private float dashLength = 5f;
    [Tooltip("Time it takes do dash again")]
    [FormerlySerializedAs("_dashCharge")]
    [Range(0f, 5f)][SerializeField] private float dashCharge = 1f;
    [SerializeField] private LayerMask dashLayer;

    //Private variables
    Rigidbody2D _rigidbody2D;
    Player1Combat shootingScript;
    Vector2 targetInput;
    Vector2 currentInput;
    //smoothing velocities
    Vector3 inputVelocity;
    Vector3 dashVelocity;
    Vector3 shootVelocity;

    Vector2 playerSize;
    Vector2 dashTarget;
    bool canDash;
    float curDashCharge;

    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        playerSize = this.GetComponent<CircleCollider2D>().bounds.extents;
        shootingScript = GetComponent<Player1Combat>();
        curDashCharge = dashCharge;
        canDash = false;
    }

    void Update()
    {
        CheckForDash();
        SmoothInput();
        if (targetInput.magnitude != 0 && !shootingScript.aiming)
            RotateWithMovement();
        else if (shootingScript.aiming)
            RotateToMousePos();
    }
    void FixedUpdate()
    {
        if (canDash)
            Dash();
        else
            MovePlayer();
    }

    private void GetMovementAxis() => targetInput = new Vector2(Input.GetAxisRaw("Horizontal"),
        Input.GetAxisRaw("Vertical"));


    private void SmoothInput()
    {
        GetMovementAxis();
        currentInput = Vector3.SmoothDamp(currentInput, targetInput,
            ref inputVelocity, inputSmoothing);
    }

    private void MovePlayer() => _rigidbody2D.velocity = currentInput * p1Speed * Time.deltaTime;

    private void RotateWithMovement()
    {
        Vector3 targetPos = _rigidbody2D.position + (currentInput * 5f);
        transform.up = (targetPos - transform.position);
    }

    private void RotateToMousePos()
    {
        Vector2 targetPos = cam.ScreenToWorldPoint(Input.mousePosition);
        transform.up = Vector3.SmoothDamp(transform.up, (targetPos - new Vector2
            (transform.position.x, transform.position.y)), ref shootVelocity, 0.1f);
    }

    private void Dash()
    {
        _rigidbody2D.MovePosition(Vector3.SmoothDamp(_rigidbody2D.position, dashTarget,
            ref dashVelocity, dashSmoothing));
        //If player has reached dash position stop
        if (Vector2.Distance(transform.position, dashTarget) <= 0.1f)
            canDash = false;
    }

    private void CheckForDash()
    {
        if (Input.GetKey(KeyCode.Space) && curDashCharge <= 0f && !canDash)
        {
            Vector2 dashDir = transform.up;
            RaycastHit2D hit = Physics2D.BoxCast(transform.position,
                playerSize, 0, dashDir, dashLength, dashLayer);

            float smallPoint = (hit.point - new Vector2(transform.position.x,
                transform.position.y)).magnitude - playerSize.magnitude;
            if (hit.collider != null && smallPoint > 0.1f)
            {
                //Dash at the position of Impact (Doesn't passes through objects)
                dashTarget = hit.point + ((playerSize.magnitude + 0.1f) * -dashDir);
            }

            else
                dashTarget = _rigidbody2D.position + (dashDir * dashLength);

            canDash = true;
            curDashCharge = dashCharge;
        }
        else if (curDashCharge > 0f)
            curDashCharge -= Time.deltaTime;
    }

    private void OnDrawGizmos()
    {
        if (canDash)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(dashTarget, playerSize.magnitude);
        }
    }
}