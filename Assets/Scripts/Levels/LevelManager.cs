using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    AsyncOperation asyncOperation;
    public static GameObject loadingScreen;
    public static int stage = 1;
    public static int difficulty = 0;
    public static int remainingEnemies = 0;
    public static bool done = false;


    void Start()
    {
        QualitySettings.vSyncCount = 0;
    }

    void Update()
    {
        if (done)
        {
            done = false;
            if (stage <= 5)
            {
                asyncOperation = SceneManager.LoadSceneAsync("Level1");
            }
            else if (stage >= 6 && stage <= 10)
            {
                asyncOperation = SceneManager.LoadSceneAsync("Level2");
            }
            else if (stage >= 11 && stage <= 16)
            {
                asyncOperation = SceneManager.LoadSceneAsync("Level3");
            }
            else if (stage > 16)
            {
                stage = 1;
                difficulty++;
                asyncOperation = SceneManager.LoadSceneAsync("Level1");
            }

            asyncOperation.allowSceneActivation = false;
            StartCoroutine(NextLevel());
        }
    }

    public static IEnumerator StartLoad()
    {
        yield return null;

        loadingScreen = GameObject.FindGameObjectWithTag("Loading");
        loadingScreen.GetComponent<Canvas>().enabled = true;

        yield return new WaitForSeconds(.3f);
        loadingScreen.GetComponent<Canvas>().enabled = false;

    }

    private void Awake()
    {
        
        int numManagers = FindObjectsOfType<LevelManager>().Length;
        if (numManagers > 1)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(transform.gameObject);
        }
    }

    public static void WinCheck()
    {
        done = true;
        remainingEnemies = 0;
        stage++;
        SaveSystem.SavePlayer();
        LevelEnd.instance.OpenCanvas();
    }

    IEnumerator NextLevel()
    {
        yield return null;

        while (!asyncOperation.isDone)
        {
            DudeController.canShoot = false;
            if (asyncOperation.progress >= 0.9f)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    loadingScreen = GameObject.FindGameObjectWithTag("Loading");
                    loadingScreen.GetComponent<Canvas>().enabled = true;

                    DudeController.canShoot = true;
                    asyncOperation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}