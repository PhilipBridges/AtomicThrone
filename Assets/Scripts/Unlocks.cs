using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Unlocks : MonoBehaviour
{
    public GameObject unlockButton;
    public Text unlockMoneyText;
    private Music perkSound;
    List<string> perksToCheck;

    float positionX = -260;
    float positionY = 150;

    int column = 0;
    
    void Start()
    {
        
        Vector2 position = new Vector3(positionX, positionY, 0);
        for (int i = 0; i < Perks.allPerks.Count; i++)
        {
            if (Perks.unlockedPerks != null)
            {
                if (Perks.unlockedPerks.Contains(Perks.allPerks[i]))
                {
                    Debug.Log(Perks.allPerks.Count);
                    MainMenu.perksToDisplay.Remove(Perks.allPerks[i].ToString());
                    Debug.Log(Perks.allPerks.Count);
                }
            }

            if (Perks.allPerks.Count == Perks.unlockedPerks.Count)
            {
                Debug.Log("CLEAR");
                MainMenu.perksToDisplay.Clear();
            }
            
        }
        MainMenu.perksToDisplay.Remove("LIFESTEAL");

        for (int i = 0; i < MainMenu.perksToDisplay.Count; i++)
        {
            GameObject perkButton = Instantiate(unlockButton, Vector2.zero + position, Quaternion.identity);
            perkButton.transform.SetParent(GameObject.FindGameObjectWithTag("UnlockMenu").transform, false);
            Text perkButtonText = perkButton.GetComponentInChildren<Text>();
            perkButton.name = MainMenu.perksToDisplay[i];
            perkButtonText.text = MainMenu.perksToDisplay[i];
            perkButton.SetActive(true);

            if (column < 5)
            {
                position.y -= 50;
                column++;
            } else
            {
                column = 0;
                position.x += 120;
                position.y = 150;
            }
        }
    }

    public void BuyPerk()
    {
        if (DudeController.currentMoney > 15)
        {
            GameObject.FindGameObjectWithTag("MenuSounds").GetComponent<MenuSounds>().PlayChing();
            string choice = EventSystem.current.currentSelectedGameObject.name;
            Perks.unlockedPerks.Add(choice);
            unlockMoneyText = GameObject.FindWithTag("MoneyText").GetComponent<Text>();
            DudeController.currentMoney -= 15;
            unlockMoneyText.text = "MONEY: $" + DudeController.currentMoney.ToString();
            MainMenu.perksToDisplay.Remove(choice);

            SaveSystem.SavePlayer();
            EventSystem.current.currentSelectedGameObject.SetActive(false);
        }
    }
}
