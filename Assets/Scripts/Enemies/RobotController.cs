using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RobotController : MonoBehaviour
{
    public float speed = 2.7f;
    public ParticleSystem smokeEffect;
    public GameObject projectilePrefab;
    public Vector2 playerLoc;
    private NavMeshAgent nav;
    private GameObject player;
    public Drops drops;
    private Renderer rend;
    private new AudioSource audio;
    new Rigidbody2D rigidbody2D;
    Animator animator;
    public GameObject hitLight;

    public AudioClip death1;
    public AudioClip hit;

    public bool dead = false;
    float timer;
    float resetTime = 1.0f;
    private float distanceToPlayer;

    private float health = 5;
    private bool damaged = false;
    //Color lerp stuff 
    Color colorStart = Color.red;
    Color colorEnd = Color.white;
    //---

    bool dropShotgun = false;
    bool dropMagnum = false;
    bool dropBouncer = false;
    bool dropLauncher = false;
    void Start()
    {
        drops = GetComponent<Drops>();
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
        hitLight.SetActive(false);
    }

    private void Awake()
    {
        health += LevelManager.difficulty;
        float roll = GetComponent<Drops>().DropRoll();
        if (roll < 40)
        {
            dropShotgun = true;
            return;
        }
        if (roll > 40 && roll < 65)
        {
            dropBouncer = true;
            return;
        }
        if (roll > 65 && roll < 90)
        {
            dropMagnum = true;
            return;
        }
        if (roll > 90 && roll < 100)
        {
            dropLauncher = true;
            return;
        }
    }

    void Update()
    {
        timer -= Time.deltaTime;

        if (dead)
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
            MoveToPlayer();
        }
        if (distanceToPlayer < 8.5f)
        {
            if (timer < 0)
            {
                timer = resetTime;
                MoveToPlayer();
                
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

        if (distanceToPlayer > 7f && distanceToPlayer < 11.5)
        {
            MoveToPlayer();
        }

        if (LevelManager.remainingEnemies < 4)
        {
            MoveToPlayer();
        }

    }

    float CurveWeightedRandom(AnimationCurve curve)
    {
        return curve.Evaluate(Random.value) * 100;
    }

    private void MoveToPlayer()
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

    public void Damage(float dmgValue)
    {
        if (!damaged)
        {
            damaged = true;
            audio.volume = 1.5f;
            audio.PlayOneShot(hit);
            audio.volume = .435f;
            StartCoroutine("SwitchColor");
        }
        health -= dmgValue;
    }

    IEnumerator SwitchColor()
    {
        hitLight.SetActive(true);
        yield return new WaitForSeconds(.2f);
        hitLight.SetActive(false);
        damaged = false;
    }

    private void Kill()
    {
        audio.Stop();
        audio.PlayOneShot(death1);
        nav.speed = 0;
        this.GetComponentInChildren<Hitbox>().gameObject.layer = 16;
        rigidbody2D.bodyType = RigidbodyType2D.Static;
        dead = true;
        animator.SetTrigger("Fixed");
        smokeEffect.Play();

        if (dropShotgun)
        {
            drops.DropShotgunAmmo(this.gameObject.transform.position);
        }
        if (dropMagnum)
        {
            drops.DropMagnumAmmo(this.gameObject.transform.position);
        }
        if (dropBouncer)
        {
            drops.DropBouncerAmmo(this.gameObject.transform.position);
        }
        if (dropLauncher)
        {
            drops.DropLauncherAmmo(this.gameObject.transform.position);
        }

        DudeController playerController = player.gameObject.GetComponent<DudeController>();

        playerController.ChangeMoney(1);
        playerController.ChangeXp(11);
        LevelManager.remainingEnemies--;
        if (LevelManager.remainingEnemies <= 0){
            LevelManager.WinCheck();
        }
    }

    void Attack()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);
        EnemyProjectile projectile = projectileObject.GetComponent<EnemyProjectile>();

        projectile.EnemyAttack(2500);

    }
}