using UnityEngine;
using BulletFury;
using BulletFury.Data;

public class EnemyHit : MonoBehaviour
{
    [SerializeField] private EnemyHealth eHealth;
    
    [SerializeField] private int healthToReduce = 5;

    public void OnBulletHit(BulletContainer bCon, BulletCollider bCol)
    {
        eHealth.ReduceEnemyHealth(healthToReduce);
        eHealth.CheckEnemyHealth(); 
    }
}
