using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BulletFury;
using BulletFury.Data;
using UnityEngine.Serialization;

public class EnemyHit : MonoBehaviour
{
    [FormerlySerializedAs("_eC")] [SerializeField] private EnemyHealth eHealth;
    
    [SerializeField] private int healthToReduce = 5;
    public void OnBulletHit(BulletContainer bCon, BulletCollider bCol)
    {
        eHealth.ReduceEnemyHealth(healthToReduce);
        eHealth.CheckEnemyHealth(); 
    }
}
