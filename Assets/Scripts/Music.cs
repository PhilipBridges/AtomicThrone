using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour
{
    public AudioSource _audioSource;
    public AudioClip music1;
    public AudioClip music2;
    private void Awake()
    {
        int numMusicPlayers = FindObjectsOfType<Music>().Length;
        if (numMusicPlayers != 1)
        {
            Destroy(this.gameObject);
            _audioSource = GetComponent<AudioSource>();
        }
        else
        {
            DontDestroyOnLoad(gameObject);
            _audioSource = GetComponent<AudioSource>();
        }
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu" && _audioSource != null)
        {
            _audioSource.Stop();
            _audioSource.clip = music1;
            _audioSource.volume = .1f;
            _audioSource.Play();
        }

        if (scene.name == "Level1")
        {
            if (_audioSource.isPlaying && _audioSource.clip == music1)
            {
                return;
            }

            _audioSource.clip = music1;
            _audioSource.Stop();
            _audioSource.clip = music1;
            _audioSource.volume = .164f;
            _audioSource.Play();
        }
        if (scene.name == "Level2")
        {
            if (_audioSource.isPlaying && _audioSource.clip == music2)
            {
                return;
            }

            _audioSource.clip = music2;
            _audioSource.Stop();
            _audioSource.clip = music2;
            _audioSource.volume = .05f;
            _audioSource.Play();
        }

    }

    public void PlayChing()
    {
        _audioSource.Play();
    }
}