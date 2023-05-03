using System.Collections;
using System.Collections.Generic;
using BulletFury;
using BulletFury.Data;
using UnityEngine;
using UnityEngine.Serialization;

public class EnemyPhases : MonoBehaviour
{
    private int enemyPhases = 0;

    private BulletManager _bulletManager;

    [SerializeField] private bool phasingEnabled = false;

    [FormerlySerializedAs("_bulletSettingsArray")] [SerializeField] private BulletSettings[] bulletSettingsArray;
    [FormerlySerializedAs("_spawnSettingsArray")] [SerializeField] private SpawnSettings[] spawnSettingsArray;
    
    // Start is called before the first frame update
    void Start()
    {
        _bulletManager = FindObjectOfType<BulletManager>();
    }

    public void UpdatePhase(int newPhaseID)
    {
        enemyPhases = newPhaseID;
        _bulletManager.SetBulletSettings(bulletSettingsArray[newPhaseID]);
        _bulletManager.SetSpawnSettings(spawnSettingsArray[newPhaseID]);
    }

    public int GetCurrentPhase()
    {
        return enemyPhases;
    }

    public void IncreasePhase()
    {
        enemyPhases++;
    }

    public bool IsPhasingEnabled()
    {
        return phasingEnabled;
    }
}
