using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelEnd : MonoBehaviour
{
    public Canvas levelEndCanvas;

    public Text healthText;
    public Text asText;
    public Text msText;
    public Text statPointText;
    public Text perkPointText;

    public Text perk1Text;
    public Text perk2Text;
    public Text perk3Text;

    public Button perk1button;
    public Button perk2button;
    public Button perk3button;

    public static List<string> randPerks = new List<string>();
    public static List<string> perkPool = new List<string>();
    public static bool newGame = true;

    private int numPerks = 0;
    public static LevelEnd instance { get; private set; }
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        levelEndCanvas = GetComponent<Canvas>();

        if (levelEndCanvas.enabled)
        {
            levelEndCanvas.enabled = false;
        }

        asText.text = "Atk Speed: " + DudeController.cooldown.ToString();
        healthText.text = "Max Health: " + DudeController.maxHealth.ToString();
        msText.text = "Move Speed: " + DudeController.playerSpeed.ToString();
        statPointText.text = "Stat Points: " + DudeController.statPoints.ToString();
        perkPointText.text = "Perk Points: " + DudeController.perkPoints.ToString();

        randPerks.Clear();
        perkPool.Clear();
        if (LevelManager.stage == 1)
        {
            foreach (var perk in Perks.unlockedPerks)
            {
                perkPool.Add(perk);
            }
        }
        GenPerks();
    }

    public void GenPerks()
    {
        if (perkPool != null && perkPool.Count > 0)
        {
            while (perkPool.Count != randPerks.Count && randPerks.Count < 3)
            {
                {
                    int rand = UnityEngine.Random.Range(0, perkPool.Count);
                    if (!randPerks.Contains(perkPool[rand]))
                    {
                        randPerks.Add(perkPool[rand]);
                        numPerks++;
                    }
                }
            }
            if (randPerks.Count >= 3)
            {
                perk1Text.text = randPerks[0];
                perk1button.name = randPerks[0];

                perk2Text.text = randPerks[1];
                perk2button.name = randPerks[1];

                perk3Text.text = randPerks[2];
                perk3button.name = randPerks[2];
            }
            if (randPerks.Count < 3)
            {
                if (randPerks.Count == 1)
                {
                    perk1Text.text = randPerks[0];
                    perk1button.name = randPerks[0];

                    perk2Text.text = "None";
                    perk2button.name = "None";

                    perk3Text.text = "None";
                    perk3button.name = "None";

                }

                if (randPerks.Count == 2)
                {
                    perk1Text.text = randPerks[0];
                    perk1button.name = randPerks[0];

                    perk2Text.text = randPerks[1];
                    perk2button.name = randPerks[1];

                    perk3Text.text = "None";
                    perk3button.name = "None";
                }
            }
        }
        else
        {
            perk1Text.text = "None";
            perk1button.name = "None";
            perk2Text.text = "None";
            perk2button.name = "None";
            perk3Text.text = "None";
            perk3button.name = "None";

        }
        numPerks = 0;
    }

    public void OpenCanvas()
    {
        if (levelEndCanvas != null)
        {
            levelEndCanvas.enabled = true;
        }
    }

    public void CloseCanvas()
    {
        if (levelEndCanvas != null)
        {
            levelEndCanvas.enabled = false;
        }
    }

    public void ChoosePerk()
    {
        if (DudeController.perkPoints > 0)
        {
            DudeController.perkPoints--;
            string choice = EventSystem.current.currentSelectedGameObject.name;
            perkPointText.text = "Perk Points: " + DudeController.perkPoints.ToString();
            Perks.PerkCheck(choice);
            perkPool.Remove(choice);
        }
    }

    public void IncreaseAttackSpeed()
    {
        if (DudeController.statPoints > 0)
        {
            DudeController.statPoints--;
            var currentAttackSpeed = DudeController.cooldown;
            DudeController.cooldown = currentAttackSpeed - .02f;
            asText.text = "Atk Speed: " + DudeController.cooldown.ToString("0.00");
            statPointText.text = "Stat Points: " + DudeController.statPoints.ToString();
        }
    }

    public void IncreaseMaxHealth()
    {
        if (DudeController.statPoints > 0)
        {
            DudeController.statPoints--;
            DudeController.maxHealth += .5f;
            DudeController.currentHealth = DudeController.maxHealth;
            healthText.text = "Max Health: " + DudeController.maxHealth.ToString();
            statPointText.text = "Stat Points: " + DudeController.statPoints.ToString();
        }
    }

    public void IncreaseMoveSpeed()
    {
        if (DudeController.statPoints > 0)
        {
            DudeController.statPoints--;
            DudeController.playerSpeed += .1f;
            msText.text = "Move Speed: " + DudeController.playerSpeed.ToString("0.00");
            statPointText.text = "Stat Points: " + DudeController.statPoints.ToString();
        }
    }

    public void IncreaseStatPoints()
    {
        statPointText.text = "Stat Points: " + DudeController.statPoints.ToString();
    }

    public void IncreasePerkPoints()
    {
        perkPointText.text = "Perk Points: " + DudeController.perkPoints.ToString();
    }
}
