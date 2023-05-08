using System.Collections;
using System.Collections.Generic;
using BulletFury;
using BulletFury.Data;
using UnityEngine;

public class Player1Combat : MonoBehaviour
{
    private BulletManager _bm;

    private int _bulletAmount;
    [SerializeField] private int maxBulletAmount = 500;

    [SerializeField] private bool infinityEnabled = false;

    private GameManager _gameManager;

    [SerializeField] private ForceField shield;
    public bool aiming;

    float bulletSpeed;
    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();

        _bm = GetComponent<BulletManager>();

        _bulletAmount = maxBulletAmount;
        bulletSpeed = _bm.GetBulletSettings().Speed;

        aiming = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (infinityEnabled || !infinityEnabled && _bulletAmount > 0)
        {
            if (!_gameManager.GetIsPaused())
            {
                ShootNewBullet();
            }
        }
        //if (!isShooting)
            //RaiseShield();
    }

    private void ShootNewBullet()
    {
        if (Input.GetKey(KeyCode.Mouse0))
        {
            isShooting = aiming = true;
            _bm.Spawn(transform.position, _bm.Plane == BulletPlane.XY ? transform.up : transform.forward);
            _bulletAmount -= 1;
        }
        else
            aiming = isShooting = false;
    }

    private void RaiseShield()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            aiming = true;
            shield.MakeAvailable();
        }
        else
        {
            aiming = false;
            shield.MakeUnavailable();
        }
    }

    public int GetBulletAmount()
    {
        return _bulletAmount;
    }

    public bool GetInfinityEnabled()
    {
        return infinityEnabled;
    }
}
