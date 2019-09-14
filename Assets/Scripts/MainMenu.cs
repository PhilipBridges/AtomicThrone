using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text moneyText;
    public static List<string> perksToDisplay = new List<string>();
    private void Start()
    {
        perksToDisplay.Clear();
        foreach (var perk in Perks.allPerks)
        {
            perksToDisplay.Add(perk);
        }

       Save data = SaveSystem.LoadSave();
        if (data != null)
        {
            DudeController.currentMoney = data.savedMoney;

            if (data.arraySavePerks != null && data.arraySavePerks.Length > 0)
            {
                for (int i = 0; i < data.arraySavePerks.Length; i++)
                {
                    if (!Perks.unlockedPerks.Contains(data.arraySavePerks[i]))
                    {
                        Perks.unlockedPerks.Add(data.arraySavePerks[i]);
                    }
                }
            }

            moneyText.text = "MONEY: $" + data.savedMoney.ToString();
        } else
        {
            moneyText.text = "MONEY: $0";
        }
    }
    public void PlayGame()
    {
        StartCoroutine(LoadGame());
    }

    IEnumerator LoadGame()
    {
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync("PGScene");

        while (!asyncOperation.isDone)
        {
            if (asyncOperation.progress >= 0.9f)
            {
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
