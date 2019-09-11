using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public static bool hasPistol = true;
    public static bool hasShotgun = false;
    public static bool hasMagnum = false;
    public static bool hasLauncher = false;
    // Start is called before the first frame update

    public static void PistolSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DudeController.weaponTime = 0f;
            hasPistol = true;
            hasShotgun = false;
            hasMagnum = false;
            hasLauncher = false;
        }
    }
    public static void ShotgunSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            DudeController.weaponTime = .4f;
            hasPistol = false;
            hasShotgun = true;
            hasMagnum = false;
            hasLauncher = false;
        }
    }

    public static void MagnumSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            DudeController.weaponTime = .7f;
            hasPistol = false;
            hasShotgun = false;
            hasMagnum = true;
            hasLauncher = false;
        }
    }

    public static void LauncherSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            DudeController.weaponTime = 1.1f;
            hasPistol = false;
            hasShotgun = false;
            hasMagnum = false;
            hasLauncher = true;
        }
    }
}
