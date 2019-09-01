using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour
{
    public AudioMixer mixer;

    public static void BackToMenu()
    {
        SceneManager.LoadScene(sceneBuildIndex: 0);
    }
    public void SetLevel(float sliderValue)
    {
        mixer.SetFloat("Master", Mathf.Log10(sliderValue) * 20);
    }

    public void SetMusic(float sliderValue)
    {
        mixer.SetFloat("Music", Mathf.Log10(sliderValue) * 20);
    }

    public void SetSound(float sliderValue)
    {
        mixer.SetFloat("Effects", Mathf.Log10(sliderValue) * 20);
    }
}