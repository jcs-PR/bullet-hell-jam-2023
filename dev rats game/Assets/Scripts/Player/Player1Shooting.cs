using System.Collections;
using System.Collections.Generic;
using BulletFury;
using BulletFury.Data;
using UnityEngine;

public class Player1Shooting : MonoBehaviour
{
    private BulletManager _bm;
    // Start is called before the first frame update
    void Start()
    {
        _bm = GetComponent<BulletManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0))
        {
            _bm.Spawn(transform.position, _bm.Plane == BulletPlane.XY ? transform.up : transform.forward);
        }
    }
}
