using System.Collections;
using System.Collections.Generic;
using BulletFury;
using BulletFury.Data;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [HideInInspector] public float bulletSpeed;
    [Range(0f, 2f)]
    [SerializeField] private float smooth;

    BulletManager _bm;
    Aim _aim;
    Vector3 targetPos;
    Vector2 smoothVelocity;
    bool canAttack;
    float deltaSpeed;

    private void Start()
    {
        _bm = GetComponent<BulletManager>();
        bulletSpeed = _bm.GetBulletSettings().Speed;
        _aim = GetComponent<Aim>();
        canAttack = true;
    }

    private void Update()
    {
        if (canAttack)
        {
            deltaSpeed = bulletSpeed * Time.deltaTime;
            RotateTowardsTarget();
            _bm.GetBulletSettings().SetSpeed(deltaSpeed);
            _bm.Spawn(transform.position, _bm.Plane == BulletPlane.XY ?
                transform.up : transform.forward);
        }
    }

    private void RotateTowardsTarget()
    {
        targetPos = _aim.GetTargetPos(deltaSpeed);
        Debug.DrawLine(transform.position, targetPos);
        transform.up = Vector2.SmoothDamp(transform.up, targetPos - transform.position,
            ref smoothVelocity, smooth);
    }
}
