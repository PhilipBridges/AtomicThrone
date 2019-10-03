using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GhostController : MonoBehaviour
{
    Vector2 playerLoc;
    private NavMeshAgent nav;
    private GameObject player;

    SpriteRenderer rend;
    private new AudioSource audio;
    new Rigidbody2D rigidbody2D;
    Animator animator;
    public Drops drops;
    float timer;
    float resetTime = .5f;
    private float distanceToPlayer;
    Vector2 lookDirection = new Vector2(1, 0);

    public float health = 5;
    private bool damaged = false;
    public bool dead = false;

    // Color lerp stuff 
    Color colorRed;
    Color colorBlue;
    Color colorWhite;
    Color currentColor;
    //---

    bool dropShotgun = false;
    bool dropMagnum = false;
    bool dropBouncer = false;
    bool dropLauncher = false;
    void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        nav = GetComponent<NavMeshAgent>();
        audio = GetComponent<AudioSource>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        drops = GetComponent<Drops>();
        nav.updateRotation = false;
        nav.updateUpAxis = false;
        player = GameObject.FindGameObjectWithTag("Player");
        playerLoc = player.transform.position;
        timer = 1.0f;
        rigidbody2D.transform.rotation = new Quaternion(0, 0, 0, 0);

        colorBlue = Color.blue;
        colorRed = Color.red;
        colorWhite = Color.white;
        currentColor = colorRed;
        colorBlue.a = .2f;
        colorRed.a = .2f;
        colorWhite.a = .2f;
    }

    private void Awake()
    {
        health += LevelManager.difficulty;
        float roll = GetComponent<Drops>().DropRoll();

        if (roll < 50)
        {
            dropShotgun = true;
            return;
        }
        if (roll > 50 && roll < 75)
        {
            dropBouncer = true;
            return;
        }
        if (roll > 75 && roll < 90)
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
        if (dead)
        {
            return;
        }

        if (this.health <= 0)
        {
            Kill();
        }

        if (currentColor == colorRed)
        {
            currentColor = colorBlue;
        } else
        {
            currentColor = colorRed;
        }

        //rend.material.color = Color.Lerp(rend.material.color, currentColor, 1);

        timer -= Time.deltaTime;

        lookDirection = player.gameObject.transform.position - transform.position;
        lookDirection.Normalize();
        lookDirection /= 2;

        playerLoc = player.transform.position;
        distanceToPlayer = Vector2.Distance(transform.position, playerLoc);

        if (health < 5)
        {
            MoveToPlayer();
        }

        if (distanceToPlayer < 2f)
        {
            if (timer < 0)
            {

                timer = resetTime;

                animator.SetFloat("Move X", lookDirection.x);
                animator.SetFloat("Move Y", lookDirection.y);
                nav.speed = 3;
                Attack();
            }
        }
        if (distanceToPlayer > 4 && distanceToPlayer < 11.5)
        {
            animator.SetFloat("Move X", lookDirection.x);

            animator.SetFloat("Move Y", lookDirection.y);

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
        if (!dead)
        nav.SetDestination(playerLoc);
    }

    public void Damage(float dmgValue)
    {
        if (!damaged)
        {
            damaged = true;
        }
        health -= dmgValue;
    }

    private void Kill()
    {
        if (lookDirection.x > .4)
        {
            animator.SetTrigger("Dead");
        } else
        {
            animator.SetTrigger("DeadLeft");
        }
        dead = true;
        
        nav.speed = 0;
        nav.enabled = false;
        this.GetComponentInChildren<Hitbox>().gameObject.layer = 16;
        rigidbody2D.bodyType = RigidbodyType2D.Static;
        DudeController playerController = player.gameObject.GetComponent<DudeController>();

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

        playerController.ChangeMoney(1);
        playerController.ChangeXp(11);
        LevelManager.remainingEnemies--;
        if (LevelManager.remainingEnemies <= 0)
        {
            LevelManager.WinCheck();
        }
    }

    void Attack()
    {
        if (distanceToPlayer < 2f)
        {
            player.gameObject.GetComponent<DudeController>().ChangeHealth(-1);
        }
        StartCoroutine(Freeze());
    }

    IEnumerator Freeze()
    {
        yield return new WaitForSeconds(1);
        nav.speed = 5.2f;
    }

    private void DeleteGhost()
    {
        Destroy(gameObject);
    }
}
