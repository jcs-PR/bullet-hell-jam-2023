using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private bool permadeathMode = false;
    [SerializeField] private string firstLevelName = "lvl01";
    public IEnumerator ResetGame()
    {
        Time.timeScale = 0;
        yield return new WaitForSeconds(3);
        NewSession();

    }

    void NewSession()
    {
        Time.timeScale = 1;
        
        if (permadeathMode)
        {
            SceneManager.LoadScene(firstLevelName);
        }
        else
        {
            int currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentLevelIndex);
        }
    }
}
