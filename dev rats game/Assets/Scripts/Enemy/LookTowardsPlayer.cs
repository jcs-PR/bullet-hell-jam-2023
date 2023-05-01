using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class LookTowardsPlayer : MonoBehaviour
{
    [SerializeField] private float maximumTurnSpeed;
    
    [SerializeField] private Transform player;

    private Vector3 _offset = new Vector3(0, 180, 0);
    
    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            transform.up = Vector3.RotateTowards(transform.up, player.position - transform.position,
                maximumTurnSpeed, 0);
        }

    }
}
