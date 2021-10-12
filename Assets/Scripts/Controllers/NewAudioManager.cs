using System;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class NewAudioManager : MonoBehaviour
{
    public static NewAudioManager instance; 
    
    public AudioSource Music;
    public AudioSource Player;
    public Sound[] SFXSounds;

    public bool musicOn = true;
    public bool sfxOn = true;
    
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (instance == null) instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        
    }

    private void Start()
    {
        throw new NotImplementedException();
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "1_GameLevel")
        {
            Player = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
        }

        else Player = null;
        
        if (!musicOn) Music.volume = 0f;
        else if (musicOn) Music.volume = 0.3f;
        if (!sfxOn) Player.volume = 0f;
        else if (sfxOn) Player.volume = 0.5f;
    }
    
    public void toggleMusic(string name)
    {
        if (musicOn)
        {
            PlayerPrefs.SetFloat("musicIsOn", 0);
            Music.volume = 0f;
            musicOn = false;
        }
        
        else if (!musicOn)
        {
            PlayerPrefs.SetFloat("musicIsOn", 1);
            Music.volume = 0.1f;
            musicOn = true;
        }

    }

    public void PlaySFX(string name)
    {
        Sound sfx = Array.Find(SFXSounds, sound => sound.name == name);
        if (sfx == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
            return;
        }

        Player.clip = sfx.clip;
        Player.Play();
    }

    public void toggleSFX(string name)
    {
        if (sfxOn)
        {
            PlayerPrefs.SetFloat("musicIsOn", 0);
            Music.volume = 0f;
            musicOn = false;
        }
        
        else if (!sfxOn)
        {
            PlayerPrefs.SetFloat("musicIsOn", 1);
            Music.volume = 0.1f;
            musicOn = true;
        }

    }


}
