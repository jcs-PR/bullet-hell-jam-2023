using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTurretObj : MonoBehaviour
{
    [SerializeField] private GameObject turretObj;
    [SerializeField] private GameObject rotationObj;
    [SerializeField] private GameObject bulletObj;

    [SerializeField] private Transform playerTransform;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 playerDirection = playerTransform.position - rotationObj.transform.position;
        float angle = Mathf.Atan2(playerDirection.y, playerDirection.x);
        rotationObj.transform.rotation = Quaternion.Euler(0f, 0f, angle * Mathf.Rad2Deg);
    }
}
