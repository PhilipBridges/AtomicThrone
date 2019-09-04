using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
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

        asText.text = "Attack Speed: " + DudeController.cooldown.ToString();
        healthText.text = "Max Health: " + DudeController.maxHealth.ToString();
        msText.text = "Move Speed: " + DudeController.playerSpeed.ToString();
        statPointText.text = "Perk Points: " + DudeController.perkPoints.ToString();
        perkPointText.text = "Perk Points: " + DudeController.perkPoints.ToString();

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

    public void ChoosePerk(Button.ButtonClickedEvent but)
    {
        Text choice = perk1button.GetComponentInParent<Text>();
        Debug.Log(choice.text);
    }

    public void IncreaseAttackSpeed()
    {
        if (DudeController.statPoints > 0)
        {
            var currentAttackSpeed = DudeController.cooldown;
            DudeController.cooldown = currentAttackSpeed - .02f;
            asText.text = "Attack Speed: " + DudeController.cooldown.ToString();
            DudeController.statPoints--;
        }
    }

    public void IncreaseMaxHealth()
    {
        if (DudeController.statPoints > 0)
        {
            DudeController.maxHealth += .5f;
            DudeController.currentHealth = DudeController.maxHealth;
            healthText.text = "Max Health: " + DudeController.maxHealth.ToString();
            DudeController.statPoints--;
        }
    }

    public void IncreaseMoveSpeed()
    {
        if (DudeController.statPoints > 0)
        {
            DudeController.playerSpeed += .1f;
            msText.text = "Move Speed: " + DudeController.playerSpeed.ToString();
            DudeController.statPoints--;
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
