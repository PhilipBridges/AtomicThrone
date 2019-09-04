using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    public float speed = 3;
    public ParticleSystem smokeEffect;
    public GameObject projectilePrefab;
    public Vector2 playerLoc;
    private NavMeshAgent nav;
    private GameObject player;

    private Renderer rend;
    private new AudioSource audio;
    new Rigidbody2D rigidbody2D;
    Animator animator;
    bool broken = true;
    float timer;
    float resetTime = 1.0f;
    private float distanceToPlayer;

    public int health = 5;
    private bool damaged = false;
    //Color lerp stuff 
    Color colorStart = Color.red;
    Color colorEnd = Color.white;
    float duration = 0.2f;
    private readonly float t = 0;
    //---
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        audio = GetComponent<AudioSource>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        nav.updateRotation = false;
        nav.updateUpAxis = false;
        player = GameObject.FindGameObjectWithTag("Player");
        playerLoc = player.transform.position;
        timer = 1.0f;
        smokeEffect.Stop();
        rigidbody2D.transform.rotation = new Quaternion(0,0,0,0);
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (!broken)
        {
            return;
        }

        if (this.health <= 0)
        {
            Kill();
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        playerLoc = player.transform.position;
        distanceToPlayer = Vector2.Distance(transform.position, playerLoc);

        if (health < 5)
        {
            Move();
        }
        if (distanceToPlayer < 6f)
        {
            if (timer < 0)
            {
                timer = resetTime;
                Move();
                
                if (horizontal != 0.0f)
                {
                    animator.SetFloat("Move X", horizontal);
                }

                if (vertical != 0.0f)
                {
                    animator.SetFloat("Move Y", vertical);
                }
                Attack();
            }
        }

        if (distanceToPlayer > 7f && distanceToPlayer < 9)
        {
            Move();
        }

    }

    private void Move()
    {
        nav.SetDestination(playerLoc);
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        DudeController player = other.gameObject.GetComponent<DudeController>();

        if (player != null)
        {
            player.ChangeHealth(-1);
        }
    }

    public void Damage()
    {
        if (!damaged)
        {
            damaged = true;
            StartCoroutine("SwitchColor");
        }
        
        health -= 1;
    }

    IEnumerator SwitchColor()
    {
        rend.material.color = new Color(1f, 0.30196078f, 0.30196078f);
        yield return new WaitForSeconds(.2f);
        rend.material.color = Color.white;
        damaged = false;
    }

    private void Kill()
    {
        rigidbody2D.bodyType = RigidbodyType2D.Static;
        broken = false;
        animator.SetTrigger("Fixed");
        audio.Stop();
        smokeEffect.Play();
        DudeController playerController = player.gameObject.GetComponent<DudeController>();
        if (this.health == 0)
        {
            playerController.ChangeMoney(1);
            playerController.ChangeXp(10);
            LevelManager.remainingEnemies--;
            if (LevelManager.remainingEnemies <= 0){
                StartCoroutine(LevelManager.WinCheck());
            }
        }
    }

    void Attack()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);
        EnemyProjectile projectile = projectileObject.GetComponent<EnemyProjectile>();

        projectile.EnemyAttack(500);

    }
}