using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    void PlayGame()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneBuildIndex: 1);

        while (!asyncOperation.isDone)
        {
            yield return null;
        }
        asyncOperation.allowSceneActivation = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
