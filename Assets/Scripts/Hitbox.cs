using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    RobotController robot;
    BossController boss;
    void Start()
    {
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        float dmgValue = 0;

        if (Weapons.hasLauncher)
        {
            dmgValue = 5;
        }
        if (Weapons.hasMagnum)
        {
            dmgValue = 5;
        }

        if (Weapons.hasPistol)
        {
            dmgValue = 1;
        }

        if (Weapons.hasShotgun)
        {
            dmgValue = 1.5f;
        }

        
        if (other.gameObject.CompareTag("PlayerProjectile"))
        {
            robot = GetComponentInParent<RobotController>();
            boss = GetComponentInParent<BossController>();

            // Robot Damage
            if (robot != null && !robot.dead)
            {
                if (Perks.lifesteal)
                {
                    DudeController.currentHealth += .1f * dmgValue;
                    DudeController.currentHealth = Mathf.Clamp(DudeController.currentHealth, 0, DudeController.maxHealth);
                    UIHealthbar.instance.SetValue(DudeController.currentHealth / (float)DudeController.maxHealth);
                }

                if (robot.dead)
                {
                    return;
                } 

                robot.Damage(dmgValue);
            }

            

            // Boss Damage
            if (boss != null)
            {
                if (Perks.lifesteal)
                {
                    DudeController.currentHealth += .1f * dmgValue;
                    DudeController.currentHealth = Mathf.Clamp(DudeController.currentHealth, 0, DudeController.maxHealth);
                    UIHealthbar.instance.SetValue(DudeController.currentHealth / (float)DudeController.maxHealth);
                }

                if (boss.dead)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    return;
                } 

                boss.Damage(dmgValue);

            }
        }

        if (other.gameObject.CompareTag("PlayerProjectile"))
        Destroy(other.gameObject);
    }
}
