using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public Text money;
    public Text level;
    public Text xp;
    public Text ammo;
    public Slider xpBar;

    public Image shotgunImage;
    public Image magnumImage;
    public Image bouncerImage;
    public Image launcherImage;
    public Image pistolImage;

    public static PlayerUI instance { get; private set; }
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        money.text = "$" + DudeController.currentMoney;
        level.text = "Lvl" + DudeController.level;
        xp.text = DudeController.currentXp.ToString() + " / " + (int)DudeController.requiredXp + "  XP";
        xpBar.value = DudeController.currentXp;
        xpBar.maxValue = DudeController.requiredXp;
    }

    // Update is called once per frame
    void Update()
    {
        money.text = "$" + DudeController.currentMoney;
        level.text = "Lvl " + DudeController.level;
    }

    public void SetXPValue(float value)
    {
        xpBar.value = value;
        xpBar.value = DudeController.currentXp;
        xpBar.maxValue = DudeController.requiredXp;
        xp.text = DudeController.currentXp.ToString() + " / " + (int)DudeController.requiredXp + "  XP";
    }
    public void SetAmmo(float value)
    {
        ammo.text = "x " + value.ToString();
        xp.text = DudeController.currentXp.ToString() + " / " + (int)DudeController.requiredXp + "  XP";
    }
}
