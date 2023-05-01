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

    public bool isPaused = false;

    private HUDScript _hudScript;
    
    // Start is called before the first frame update
    void Start()
    {
        // GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        // amountOfEnemies = enemies.Length;

        _hudScript = FindObjectOfType<HUDScript>();

    }

    // Update is called once per frame
    void Update()
    {
        // CheckAmountOfEnemies();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
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

    public void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;
            _hudScript.SetPausePanel(true);
            isPaused = true;

        }
        else if (isPaused)
        {
            Time.timeScale = 1;
            _hudScript.SetPausePanel(false);
            isPaused = false;
        }
    }

    public bool GetIsPaused()
    {
        return isPaused;
    }
}

// References.
// Amount of enemies:
// https://answers.unity.com/questions/496609/how-to-keep-count-of-enemies-left.html
