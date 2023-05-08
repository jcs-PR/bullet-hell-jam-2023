using System.Collections;
using System.Collections.Generic;
using BulletFury;
using BulletFury.Data;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [Range(0f, 2f)]
    [SerializeField] private float smooth;
    [SerializeField] private bool useTargetingSystem;
    [HideInInspector] public bool canAttack;

    BulletManager _bm;
    Aim _aim;
    Vector3 targetPos;
    Vector2 smoothVelocity;
    float bulletSpeed;
    float deltaSpeed;

    private void Start()
    {
        _bm = GetComponentInChildren<BulletManager>();
        bulletSpeed = _bm.GetBulletSettings().Speed;
        _aim = GetComponent<Aim>();
        canAttack = false;
    }

    private void Update()
    {
        if (canAttack)
        {
            deltaSpeed = bulletSpeed * Time.deltaTime;
            if(useTargetingSystem)
                targetPos = _aim.GetTargetPos(deltaSpeed);
            RotateTowardsTarget(targetPos);
            _bm.GetBulletSettings().SetSpeed(deltaSpeed);
            _bm.Spawn(transform.position, _bm.Plane == BulletPlane.XY ?
                transform.up : transform.forward);
        }
    }

    public void RotateTowardsTarget(Vector3 target)
    {
        transform.up = Vector2.SmoothDamp(transform.up, target - transform.position,
            ref smoothVelocity, smooth);
    }

    public void SetTargetPos(Vector3 _target) => targetPos = _target;
}
