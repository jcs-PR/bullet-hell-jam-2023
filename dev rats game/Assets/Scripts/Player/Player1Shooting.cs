using System.Collections;
using System.Collections.Generic;
using BulletFury;
using BulletFury.Data;
using UnityEngine;

public class Player1Shooting : MonoBehaviour
{
    private BulletManager _bm;

    private int _bulletAmount;
    [SerializeField] private int maxBulletAmount = 100; 
    
    // Start is called before the first frame update
    void Start()
    {
        _bm = GetComponent<BulletManager>();

        _bulletAmount = maxBulletAmount;
    }

    // Update is called once per frame
    void Update()
    {
        ShootNewBullet();
    }

    private void ShootNewBullet()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _bm.Spawn(transform.position, _bm.Plane == BulletPlane.XY ? transform.up : transform.forward);
            _bulletAmount -= 1;
        }
    }
}
