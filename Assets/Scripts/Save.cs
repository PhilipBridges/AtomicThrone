using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    public int savedMoney;
    public static List<string> listSavePerks = new List<string>();
    public string[] arraySavePerks;
    public Save ()
    {
        savedMoney = DudeController.currentMoney;

        for (int i = 0; i < Perks.unlockedPerks.Count; i++)
        {
            listSavePerks.Add(Perks.unlockedPerks[i]);
        }

        arraySavePerks = listSavePerks.ToArray();

    }
}
