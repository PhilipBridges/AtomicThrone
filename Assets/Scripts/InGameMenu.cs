using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    public AudioMixer mixer;
    public static Canvas menuCanvas;
    public GameObject menu;
    bool closeable = false;

    void Start()
    {
        menuCanvas = GetComponent<Canvas>();
        if (menuCanvas != null)
        {
            menuCanvas.enabled = false;
        }
        GameObject.FindGameObjectWithTag("Music").GetComponent<Music>().PlayMusic();
        closeable = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !closeable)
        {
            StartCoroutine(OpenMenu());
        }

        if (Input.GetKeyDown(KeyCode.Escape) && closeable)
        {
            StartCoroutine(CloseMenu());
        }
    }

    IEnumerator OpenMenu()
    {
        //menu = menuCanvas.gameObject;
        //menu.GetComponentInChildren<Canvas>().enabled = true;
        menuCanvas.enabled = true;
        DudeController.canShoot = false;
        yield return new WaitForSeconds(.3f);
        closeable = true;
    }

    IEnumerator CloseMenu()
    {
        //menu = menuCanvas.gameObject;
        //menu.GetComponentInChildren<Canvas>().enabled = false;
        menuCanvas.enabled = false;
        DudeController.canShoot = true;
        yield return new WaitForSeconds(.3f);
        closeable = false;
    }


    public void BackClick()
    {
        menuCanvas.enabled = false;
    }

    public void BackToMenu()
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