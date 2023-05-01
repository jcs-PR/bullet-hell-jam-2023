using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    [SerializeField] private string gameURL;

    [SerializeField] private string firstLevelName = "lvl01";

    [SerializeField] private TMP_Text ammoTxt;
    [SerializeField] private TMP_Text healthAmountTxt;

    private Player1Shooting _player1Shooting;
    private Player1Health _player1Health;

    [SerializeField] private Slider healthSlider;

    private void Awake()
    {
        
    }

    // Start is called before the first frame update
    void Start()
    {
        _player1Shooting = FindObjectOfType<Player1Shooting>();
        _player1Health = FindObjectOfType<Player1Health>();
        UpdateHUDValues();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHUDValues();
    }

    void UpdateHUDValues()
    {
        if (_player1Shooting != null)
        {
            UpdateAmmoTxt();

        }

        if (_player1Health != null)
        {
            UpdateHealthSlider();
            UpdateHealthAmountTxt();

        }
    }

    void UpdateHealthSlider()
    {
        healthSlider.value = _player1Health.GetPlayerCurrentHealth();
    }

    void UpdateHealthAmountTxt()
    {
        healthAmountTxt.text = _player1Health.GetPlayerCurrentHealth().ToString();
    }

    private void UpdateAmmoTxt()
    {
        if (_player1Shooting.GetInfinityEnabled())
        {
            char infinitySymbol = '\u221e';
            string newInfinityAmmoTxt = "Ammo \n" + infinitySymbol.ToString();
            ammoTxt.text = newInfinityAmmoTxt;
        }
        
        else if (!_player1Shooting.GetInfinityEnabled())
        {
            string newStandardArrowTxt = "Ammo \n" + _player1Shooting.GetBulletAmount().ToString();
            ammoTxt.text = newStandardArrowTxt;
        }

    }

    public void OpenFeedbackForm()
    {
        Application.OpenURL(gameURL);
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(firstLevelName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}

// References.
// Infinity Symbol Unicode:
// https://stackoverflow.com/a/10806963
// Unicode for Unity C#:
// http://answers.unity.com/answers/170110/view.html
