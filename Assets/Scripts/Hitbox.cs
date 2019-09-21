using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hitbox : MonoBehaviour
{
    RobotController robot;
    BossController boss;
    BatController bat;
    GhostController ghost;
    void Start()
    {
    }

    void Update()
    {
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("PlayerProjectile") || other.gameObject.CompareTag("BouncerBullet") || other.gameObject.CompareTag("Explosion"))
        {
            float dmgValue;

            if (other.gameObject.CompareTag("Explosion"))
            {
                dmgValue = 6;
            } else
            {
                dmgValue = other.gameObject.GetComponent<Projectile>().dmgValue;
            }

            robot = GetComponentInParent<RobotController>();
            boss = GetComponentInParent<BossController>();
            bat = GetComponentInParent<BatController>();
            ghost = GetComponentInParent<GhostController>();

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

            if (ghost != null && !ghost.dead)
            {
                if (Perks.lifesteal)
                {
                    DudeController.currentHealth += .1f * dmgValue;
                    DudeController.currentHealth = Mathf.Clamp(DudeController.currentHealth, 0, DudeController.maxHealth);
                    UIHealthbar.instance.SetValue(DudeController.currentHealth / (float)DudeController.maxHealth);
                }

                if (ghost.dead)
                {
                    return;
                } 

                ghost.Damage(dmgValue);
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
            
            // Bat Damage
            if (bat != null)
            {
                if (Perks.lifesteal)
                {
                    DudeController.currentHealth += .1f * dmgValue;
                    DudeController.currentHealth = Mathf.Clamp(DudeController.currentHealth, 0, DudeController.maxHealth);
                    UIHealthbar.instance.SetValue(DudeController.currentHealth / (float)DudeController.maxHealth);
                }

                if (bat.dead)
                {
                    GetComponent<BoxCollider2D>().enabled = false;
                    return;
                } 

                bat.Damage(dmgValue);

            }
        }

        if (other.gameObject.CompareTag("PlayerProjectile"))
        Destroy(other.gameObject);
    }
}
