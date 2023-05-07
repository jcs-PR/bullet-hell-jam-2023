using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumperAI : MonoBehaviour
{
    [SerializeField] private  AnimationCurve curve;
    [SerializeField] private Transform player;
    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float lastKey = 0.8f;
    [SerializeField] private float attackRange;
    [SerializeField] private float fireRate = 2f;

    //Behaviour
    enum Behaviour {Roam, Attack}
    Behaviour currentBehaviour;

    //private variables
    Rigidbody2D rb;
    Quaternion rot;
    Vector2 velocity;
    Vector2 moveDir;
    Shoot shoot;
    float curveTime;
    float moveForce;
    float rotTime;
    float curRate;
    bool canMove = false;
    bool getRandom = true;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        rb = GetComponent<Rigidbody2D>();
        shoot = GetComponent<Shoot>();
        rot = transform.rotation;
        curRate = fireRate;
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(RefreshAI());
        if (shoot.canAttack)
            shoot.canAttack = false;

        if (currentBehaviour == Behaviour.Roam)
        {
            StartCoroutine(GetRandomDir());
            curRate -= Time.deltaTime;
        }

        else if (currentBehaviour == Behaviour.Attack && curRate <= 0f && rb.velocity.magnitude <= 0.1f)
        {
            Attack();
            curRate = fireRate;
        }
        else
            curRate -= Time.deltaTime;
    }

    void FixedUpdate()
    {
        if(canMove)
            Move();
    }

    private void Move()
    {
        if (curveTime <= lastKey)
        {
            moveForce = curve.Evaluate(curveTime);
            curveTime += Time.fixedDeltaTime;
            rb.AddForce(moveDir * moveForce * Time.deltaTime, ForceMode2D.Impulse);
        }
        else
            canMove = false;
    }

    private IEnumerator GetRandomDir()
    {

        if (!canMove && rb.velocity.magnitude <= 0.1f)
        {
            if (getRandom)
            {
                rot = Random.rotation;
                curveTime = rot.y = rot.x = 0f;
                rb.velocity = Vector2.zero;
                rb.angularVelocity = 0f;
                getRandom = false;
            }

            transform.rotation = Quaternion.Slerp(transform.rotation, rot, Mathf.Clamp01(rotTime));
            rotTime += Time.deltaTime;
        }
        if (rotTime >= 1f)
        {
            yield return new WaitForSeconds(1f);
            rotTime = 0f;
            getRandom = canMove = true;
            moveDir = transform.up;
        }
    }

    private IEnumerator RefreshAI()
    {
        yield return new WaitForSeconds(0.1f);
        if (Physics2D.OverlapCircle(transform.position, attackRange, playerMask))
        {
            RaycastHit2D hit;
            Vector2 vector = player.position - transform.position;
            hit = Physics2D.Raycast(transform.position, vector.normalized,
                vector.magnitude, obstacleMask);
            if (hit != true)
            {
                currentBehaviour = Behaviour.Attack;
                transform.up = player.position - transform.position;
            }
        }
        else
            currentBehaviour = Behaviour.Roam;

    }

    private void Attack()
    {
        curveTime = 0f;
        shoot.canAttack = true;
        shoot.SetTargetPos(player.position);
        AddRecoil();
    }

    private void AddRecoil()
    {
        moveDir = -transform.up;
        canMove = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}