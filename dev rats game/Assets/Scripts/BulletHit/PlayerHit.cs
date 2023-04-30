using System.Collections;
using System.Collections.Generic;
using BulletFury;
using BulletFury.Data;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHit : MonoBehaviour
{
    private GameManager _gameManager;
    
    private Player1Health _player1Health;
    
    // Start is called before the first frame update
    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _player1Health = GetComponent<Player1Health>();
    }
    
    public void OnBulletHit(BulletContainer bCon, BulletCollider bCol)
    {
        _player1Health.ReduceHealth(1);
    }


}
