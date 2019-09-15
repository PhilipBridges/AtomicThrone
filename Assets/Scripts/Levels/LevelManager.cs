using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    AsyncOperation asyncOperation;
    public static int stage = 1;
    public static int difficulty;
    public static int remainingEnemies = 0;
    public static bool done = false;
    void Start()
    {

    }

    void Update()
    {
        if (done)
        {
            done = false;
            if (stage < 5)
            {
                asyncOperation = SceneManager.LoadSceneAsync("Level1");
            }
            else
            {
                asyncOperation = SceneManager.LoadSceneAsync("Level2");
            }

            asyncOperation.allowSceneActivation = false;
            StartCoroutine(NextLevel());
        }
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
            if (asyncOperation.progress >= 0.9f)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    asyncOperation.allowSceneActivation = true;
                }
            }

            yield return null;
        }
    }
}