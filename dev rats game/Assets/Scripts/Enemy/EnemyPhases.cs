using System.Collections;
using System.Collections.Generic;
using BulletFury;
using UnityEngine;

public class EnemyPhases : MonoBehaviour
{
    private int enemyPhases = 0;

    private BulletManager _bulletManager;
    
    // Start is called before the first frame update
    void Start()
    {
        _bulletManager = FindObjectOfType<BulletManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdatePhase()
    {
        if (enemyPhases == 1)
        {
            
        }
        else if (enemyPhases == 2)
        {
            
        }

        else if (enemyPhases == 3)
        {
            
        }
    }

    public int GetCurrentPhase()
    {
        return enemyPhases;
    }

    public void IncreasePhase()
    {
        enemyPhases++;
    }
}
