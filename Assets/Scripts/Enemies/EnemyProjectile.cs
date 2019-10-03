using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    Vector3 playerTarget;
    Rigidbody2D rigidbody2d;
    GameObject player;

    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        rigidbody2d = GetComponent<Rigidbody2D>();
        transform.position = transform.position + playerTarget * Time.deltaTime;

        playerTarget = player.gameObject.transform.position - transform.position;
        playerTarget.z = 0;
        playerTarget.Normalize();
    }

    void Update()
    {
        if (transform.position.magnitude > 100.0f)
        {
            Destroy(gameObject);
        }
    }

    public void EnemyAttack(float force)
    {
        Transform playerTransform = player.transform;
        Vector2 position = playerTransform.position;
        position.Normalize();
        rigidbody2d.AddForce(playerTarget * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        DudeController player = other.gameObject.GetComponent<DudeController>();
        int bonusDamage = LevelManager.difficulty;
        if (player != null)
        {
            player.ChangeHealth(-1 - bonusDamage);
        }

        Destroy(gameObject);
    }
}