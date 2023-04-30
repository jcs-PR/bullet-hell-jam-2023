using System.Collections;
using System.Collections.Generic;
using BulletFury;
using BulletFury.Data;
using UnityEngine;

public class PlayerHit : MonoBehaviour
{
    private Player1Health _player1Health;
    
    // Start is called before the first frame update
    void Start()
    {
        _player1Health = GetComponent<Player1Health>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnBulletHit(BulletContainer bCon, BulletCollider bCol)
    {
        _player1Health.ReduceHealth(1);
    }
}
