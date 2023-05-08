using System.Collections;
using System.Collections.Generic;
using BulletFury;
using BulletFury.Data;
using UnityEngine;


public class TurretAI : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private BulletSettings random;
    [SerializeField] private SpawnSettings randomSpawn;
    [Space]
    [SerializeField] private BulletSettings direct;
    [SerializeField] private SpawnSettings directSpawn;

    [SerializeField] private LayerMask playerMask;
    [SerializeField] private LayerMask obstacleMask;
    [SerializeField] private float attackRange;
    [SerializeField] private float ultimateDuration;
    [SerializeField] private float ultimateCharge;

    Shoot shoot;
    float curCharge;
    float checkDelay = 0.1f;
    bool usingUltimate;
    BulletManager _bm;

    private void Awake()
    {
        shoot = GetComponent<Shoot>();
        GetComponent<Aim>().maxRange = attackRange;
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void Start()
    {
        curCharge = ultimateCharge;
        _bm = GetComponentInChildren<BulletManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!usingUltimate)
            StartCoroutine(CheckIfCanAttack());
        else
            RotateToPlayer();
    }

    private IEnumerator CheckIfCanAttack()
    {
        yield return new WaitForSeconds(checkDelay);
        if (Physics2D.OverlapCircle(transform.position, attackRange, playerMask))
        {
            if (curCharge <= 0f)
            {
                curCharge = ultimateCharge;
                StartCoroutine(UseUltimate());
                yield return null;
            }

            else
                curCharge -= Time.deltaTime + checkDelay;

            RaycastHit2D hit;
            Vector2 vector = player.position - transform.position;
            hit = Physics2D.Raycast(transform.position, vector.normalized, vector.magnitude, obstacleMask);
            if (hit != true)
                shoot.canAttack = true;

            else
                shoot.canAttack = false;
        }
        else
            shoot.canAttack = false;
    }

    private IEnumerator UseUltimate()
    {
        //Use
        _bm.SetBulletSettings(random);
        _bm.SetSpawnSettings(randomSpawn);
        usingUltimate = true;
        shoot.canAttack = false;
        Debug.Log("Using Ultimate");
        yield return new WaitForSeconds(ultimateDuration);
        //return
        _bm.SetBulletSettings(direct);
        _bm.SetSpawnSettings(directSpawn);
        usingUltimate = false;
    }

    private void RotateToPlayer() => transform.up = (player.position + (player.up * 5) - transform.position);

    private void OnDrawGizmos()
    {
        if (Application.isPlaying && shoot.canAttack)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, player.position);
        }

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }
}