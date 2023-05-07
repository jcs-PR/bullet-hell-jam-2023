using System.Collections;
using System.Collections.Generic;
using BulletFury;
using BulletFury.Data;
using UnityEngine;

public class Player1Combat : MonoBehaviour
{
    [SerializeField] private ForceField shield;
    [HideInInspector] public bool aiming;

    BulletManager _bm;
    float bulletSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _bm = GetComponent<BulletManager>();
        bulletSpeed = _bm.GetBulletSettings().Speed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            aiming = true;
            _bm.GetBulletSettings().SetSpeed(bulletSpeed * Time.deltaTime);
            _bm.Spawn(transform.position, _bm.Plane == BulletPlane.XY ?
                transform.up : transform.forward);
        }
        else if (Input.GetMouseButton(1))
        {
            aiming = true;
            shield.MakeAvailable();
        }
        else
        {
            shield.MakeUnavailable();
            aiming = false;
        }

    }
}
