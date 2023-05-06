using UnityEngine;
using UnityEngine.SceneManagement;

public class LvlEndPoint : MonoBehaviour
{
    private GameObject[] _enemies;

    [SerializeField] private string enemyTag = "Enemy";

    private int _numOfEnemies;

    [SerializeField] private string lvl01Name = "lvl01";
    [SerializeField] private string gameOverName = "Game Over";
    
    [SerializeField] private bool isEndLevel = false;
    
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
               if (!isEndLevel)
               {
                   int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
                   SceneManager.LoadScene(currentSceneIndex + 1);
               }
               else
               {
                   SceneManager.LoadScene(gameOverName);
               }
           }
        }
    }
}

// References.
// https://answers.unity.com/questions/496609/how-to-keep-count-of-enemies-left.html

