using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public static int stage;
    public static int difficulty;
    public static int remainingEnemies = 0;
    void Start()
    {
    }

    void Update()
    {
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

    public static IEnumerator WinCheck()
    {
        remainingEnemies = 0;
        stage++;
        LevelEnd.instance.OpenCanvas();

        yield return null;
        
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneBuildIndex: 1);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    asyncOperation.allowSceneActivation = true;
                }
                yield return null;
            }
        }
    }
}