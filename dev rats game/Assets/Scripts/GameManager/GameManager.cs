using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Debug = System.Diagnostics.Debug;

public class GameManager : MonoBehaviour
{

    [SerializeField] private int resetTime = 3;

    [SerializeField] private string lvl01Name = "lvl01";
    [SerializeField] private string lvl02Name = "lvl02";
    [SerializeField] private string gameOverLvlName = "gameover";

    private int amountOfEnemies;
    
    // Start is called before the first frame update
    void Start()
    {
        // GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // amountOfEnemies = enemies.Length;

    }

    // Update is called once per frame
    void Update()
    {
        // CheckAmountOfEnemies();
    }

    public void DecreaseAmountOfEnemies()
    {
        amountOfEnemies--;
    }

    public void CheckAmountOfEnemies()
    {
       
        if (amountOfEnemies == 0)
        {
            NextLevel();
        }
    }

    void NextLevel()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex + 1);
    }

    public IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(resetTime);
        int lvl01Index = SceneManager.GetSceneByName(lvl01Name).buildIndex;
        SceneManager.LoadScene(lvl01Name);
    }
}

// References.
// Amount of enemies:
// https://answers.unity.com/questions/496609/how-to-keep-count-of-enemies-left.html
