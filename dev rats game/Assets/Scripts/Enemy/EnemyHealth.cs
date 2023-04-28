using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EnemyHealth : MonoBehaviour
{
    [SerializeField] private int resetTime = 3;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Player1Health player1Health = other.gameObject.GetComponent<Player1Health>();
            player1Health.SetPlayerHealth(0);
            StartCoroutine(ResetGame());
        }
    }

    IEnumerator ResetGame()
    {
        yield return new WaitForSeconds(resetTime);
        int currentScene = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentScene);
    }
}
