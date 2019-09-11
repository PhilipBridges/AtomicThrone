using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
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
            NextLevel();
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

    void NextLevel()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            done = false;
            SceneManager.LoadScene(sceneBuildIndex: 1);
        }
    }
}