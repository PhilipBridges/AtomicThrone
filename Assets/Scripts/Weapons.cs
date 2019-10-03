using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapons : MonoBehaviour
{
    public static bool pickedUpShotgun = false;
    public static bool pickedUpMagnum = false;
    public static bool pickedUpLauncher = false;
    public static bool pickedUpBouncer = false;

    public static bool hasPistol = true;
    public static bool hasShotgun = false;
    public static bool hasMagnum = false;
    public static bool hasLauncher = false;
    public static bool hasBouncer = false;

    public static int shotgunAmmo = 0;
    public static int magnumAmmo = 0;
    public static int launcherAmmo = 0;
    public static int bouncerAmmo = 0;

    public static void PistolSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            DudeController.weaponTime = 0f;
            hasPistol = true;
            hasShotgun = false;
            hasMagnum = false;
            hasLauncher = false;
            hasBouncer = false;

            PlayerUI.instance.pistolImage.gameObject.SetActive(true);
            PlayerUI.instance.magnumImage.gameObject.SetActive(false);
            PlayerUI.instance.launcherImage.gameObject.SetActive(false);
            PlayerUI.instance.shotgunImage.gameObject.SetActive(false);
            PlayerUI.instance.bouncerImage.gameObject.SetActive(false);

            PlayerUI.instance.ammo.text = "x " + "\u221E";
        }
    }
    public static void ShotgunSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha2) && pickedUpShotgun)
        {
            DudeController.weaponTime = .7f;
            hasPistol = false;
            hasShotgun = true;
            hasMagnum = false;
            hasLauncher = false;
            hasBouncer = false;

            PlayerUI.instance.pistolImage.gameObject.SetActive(false);
            PlayerUI.instance.magnumImage.gameObject.SetActive(false);
            PlayerUI.instance.launcherImage.gameObject.SetActive(false);
            PlayerUI.instance.shotgunImage.gameObject.SetActive(true);
            PlayerUI.instance.bouncerImage.gameObject.SetActive(false);

            PlayerUI.instance.SetAmmo(shotgunAmmo);
        }
    }

    public static void MagnumSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3) && pickedUpMagnum)
        {
            DudeController.weaponTime = .8f;
            hasPistol = false;
            hasShotgun = false;
            hasMagnum = true;
            hasLauncher = false;
            hasBouncer = false;

            PlayerUI.instance.pistolImage.gameObject.SetActive(false);
            PlayerUI.instance.magnumImage.gameObject.SetActive(true);
            PlayerUI.instance.launcherImage.gameObject.SetActive(false);
            PlayerUI.instance.shotgunImage.gameObject.SetActive(false);
            PlayerUI.instance.bouncerImage.gameObject.SetActive(false);

            PlayerUI.instance.SetAmmo(magnumAmmo);
        }
    }

    public static void LauncherSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha4) && pickedUpLauncher == true)
        {
            DudeController.weaponTime = 1.1f;
            hasPistol = false;
            hasShotgun = false;
            hasMagnum = false;
            hasLauncher = true;
            hasBouncer = false;

            PlayerUI.instance.pistolImage.gameObject.SetActive(false);
            PlayerUI.instance.magnumImage.gameObject.SetActive(false);
            PlayerUI.instance.launcherImage.gameObject.SetActive(true);
            PlayerUI.instance.shotgunImage.gameObject.SetActive(false);
            PlayerUI.instance.bouncerImage.gameObject.SetActive(false);

            PlayerUI.instance.SetAmmo(launcherAmmo);
        }
    }

    public static void BouncerSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha5) && pickedUpBouncer)
        {
            DudeController.weaponTime = .6f;
            hasPistol = false;
            hasShotgun = false;
            hasMagnum = false;
            hasLauncher = false;
            hasBouncer = true;

            PlayerUI.instance.pistolImage.gameObject.SetActive(false);
            PlayerUI.instance.magnumImage.gameObject.SetActive(false);
            PlayerUI.instance.launcherImage.gameObject.SetActive(false);
            PlayerUI.instance.shotgunImage.gameObject.SetActive(false);
            PlayerUI.instance.bouncerImage.gameObject.SetActive(true);

            PlayerUI.instance.SetAmmo(bouncerAmmo);
        }
    }

    public static void Default()
    {
        DudeController.weaponTime = 0f;
        hasPistol = true;
        hasShotgun = false;
        hasMagnum = false;
        hasLauncher = false;
        hasBouncer = false;

        PlayerUI.instance.pistolImage.gameObject.SetActive(true);
        PlayerUI.instance.magnumImage.gameObject.SetActive(false);
        PlayerUI.instance.launcherImage.gameObject.SetActive(false);
        PlayerUI.instance.shotgunImage.gameObject.SetActive(false);
        PlayerUI.instance.bouncerImage.gameObject.SetActive(false);

        PlayerUI.instance.ammo.text = "x " + "\u221E";
    }

    public static void AllWeapons()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            pickedUpShotgun = true;
            pickedUpMagnum = true;
            pickedUpLauncher = true;
            pickedUpBouncer = true;

            shotgunAmmo = 999;
            magnumAmmo = 999;
            launcherAmmo = 999;
            bouncerAmmo = 999;
        }
    }
}
