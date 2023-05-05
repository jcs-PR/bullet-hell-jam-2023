using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlEndPoint : MonoBehaviour
{
    private GameObject[] _enemies;

    [SerializeField] private string enemyTag = "Enemy";

    private int _numOfEnemies;

    [SerializeField] private string lvl01Name = "lvl01";
    
    // Start is called before the first frame update
    void Start()
    {
        GetEnemyCount();
    }

    void GetEnemyCount()
    {
        _enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        _numOfEnemies = _enemies.Length;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
           GetEnemyCount();
           if (_numOfEnemies == 0)
           {
               SceneManager.LoadScene(lvl01Name);
           }
        }
    }
}

// References.
// https://answers.unity.com/questions/496609/how-to-keep-count-of-enemies-left.html

