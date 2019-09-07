using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class Perks : MonoBehaviour
{
    public static List<string> allPerks = new List<string> { "LIFESTEAL", "EVASION", "FRAGILE", "TEST2", "TEST3", "TEST4", "TEST5", "TEST6", "TEST7" };

    public static bool evasion = true;
    public static bool lifesteal = false;

    public static void PerkCheck(string perkName)
    {
        switch (perkName)
        {
            case "LIFESTEAL":
                lifesteal = true;
                break;
            case "EVASION":
                evasion = true;
                break;
            case "FRAGILE":
                DudeController.playerSpeed += 1;
                DudeController.maxHealth -= 1;
                break;
            default:
                break;
        }
    }
}

