using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] musicClips;

    [SerializeField] private string lvl01Name = "lvl01";

    private AudioSource _audioSource;
    
    // Start is called before the first frame update
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        ChooseMusicTrack();
    }

    private void Update()
    {
        MuteAudio();
    }

    private void ChooseMusicTrack()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == SceneManager.GetSceneByName(lvl01Name).buildIndex)
        {
            _audioSource.clip = musicClips[0];
        }


        _audioSource.Play();
    }

    void MuteAudio()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            if (_audioSource.mute == false)
            {
                _audioSource.mute = true;
            }
            
            else if (_audioSource.mute = true)
            {
                _audioSource.mute = false;
            }
        }
    }
}
