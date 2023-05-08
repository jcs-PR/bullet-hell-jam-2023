using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HUDScript : MonoBehaviour
{
    [SerializeField] private string gameURL;

    [SerializeField] private string firstLevelName = "lvl01";

    [SerializeField] private TMP_Text ammoTxt;
    [SerializeField] private TMP_Text ammoAmountTxt;
    [SerializeField] private TMP_Text healthAmountTxt;

    [FormerlySerializedAs("_player1Shooting")] [SerializeField] Player1Combat player1Shooting;
    [FormerlySerializedAs("_player1Health")] [SerializeField] Player1Health player1Health;

    [SerializeField] private Slider healthSlider;

    [SerializeField] private GameObject pausePanel;

    private bool _localeButtonActive = false;
    
    

    // Start is called before the first frame update
    void Start()
    {
        pausePanel.SetActive(false);
        UpdateHUDValues();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHUDValues();
    }

    public void ChangeLanguage(int localeID)
    {
        if (_localeButtonActive)
            return;
        StartCoroutine(SetLanguage(localeID));
    }

    IEnumerator SetLanguage(int _localeID)
    {
        _localeButtonActive = true;
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[_localeID];
        _localeButtonActive = false;

    }

    void UpdateHUDValues()
    {
        if (player1Shooting != null)
        {
            UpdateAmmoTxt();
        }

        if (player1Health != null)
        {
            UpdateHealthSlider();
            UpdateHealthAmountTxt();

        }
    }

    void UpdateHealthSlider()
    {
        healthSlider.value = player1Health.GetPlayerCurrentHealth();
    }

    void UpdateHealthAmountTxt()
    {
        healthAmountTxt.text = player1Health.GetPlayerCurrentHealth().ToString();
    }

    private void UpdateAmmoTxt()
    {
        if (player1Shooting.GetInfinityEnabled())
        {
            char infinitySymbol = '\u221e';
            ammoAmountTxt.text = infinitySymbol.ToString();
        }
        
        else if (!player1Shooting.GetInfinityEnabled())
        {
            ammoAmountTxt.text = player1Shooting.GetBulletAmount().ToString();
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

    public void SetPausePanel(bool condition)
    {
        pausePanel.SetActive(condition);
    }
}

// References.
// Infinity Symbol Unicode:
// https://stackoverflow.com/a/10806963
// Unicode for Unity C#:
// http://answers.unity.com/answers/170110/view.html
// Language change:
// https://youtu.be/qcXuvd7qSxg
