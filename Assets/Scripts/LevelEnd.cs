using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public static List<String> randPerks = new List<String>();
    public static List<String> perkPool = Perks.allPerks;
    public static bool newGame = true;
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

        GenPerks();
    }

    public void GenPerks()
    {
        while (randPerks.Count < 4)
        {
            if (perkPool.Count > 0)
            {
                int rand = UnityEngine.Random.Range(0, perkPool.Count - 1);
                if (!randPerks.Contains(perkPool[rand]))
                {
                    randPerks.Add(perkPool[rand]);
                }
            }
        }
        perk1Text.text = randPerks[0];
        perk1button.name = randPerks[0];

        perk2Text.text = randPerks[1];
        perk2button.name = randPerks[1];

        perk3Text.text = randPerks[2];
        perk3button.name = randPerks[2];
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
            asText.text = "Atk Speed: " + DudeController.cooldown.ToString();
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
            msText.text = "Move Speed: " + DudeController.playerSpeed.ToString();
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
