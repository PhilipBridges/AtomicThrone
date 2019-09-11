using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Perks : MonoBehaviour
{
    public static List<string> allPerks = new List<string> { "LIFESTEAL", "EVASION", "FRAGILE", "TEST2", "TEST3", "TEST4", "TEST5", "TEST6", "TEST7" };
    public static List<string> unlockedPerks = new List<string> { "LIFESTEAL" };

    public static bool evasion = false;
    public static bool lifesteal = false;

    public static void PerkCheck(string perkName)
    {
        switch (perkName)
        {
            case "LIFESTEAL":
                lifesteal = true;
                LevelEnd.perkPool.Remove("LIFESTEAL");
                break;
            case "EVASION":
                evasion = true;
                LevelEnd.perkPool.Remove("EVASION");
                break;
            case "FRAGILE":
                LevelEnd.perkPool.Remove("FRAGILE");
                DudeController.playerSpeed += 1;
                DudeController.maxHealth -= 1;
                UIHealthbar.instance.SetValue(DudeController.currentHealth / (float)DudeController.maxHealth);
                break;
            default:
                break;
        }
    }
}

