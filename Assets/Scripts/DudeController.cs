using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DudeController : MonoBehaviour
{
    Animator anim;
    new Rigidbody2D rigidbody;
    //public bool open = false;

    //public GameObject menu;
    public GameObject projectilePrefab;
    public GameObject bouncerPrefab;
    public GameObject grenade;
    // SOUND STUFF
    private AudioSource audioSource;
    public AudioClip shotgunSound;
    public AudioClip launcherSound;
    public AudioClip magnumSound;
    public AudioClip bouncerSound;
    public AudioClip shoot;
    public AudioClip hitSound;
    public AudioClip pickupSound;
    //-------------
    Vector2 movementDirection;
    Vector2 lookDirection = new Vector2(1, 0);
    public static Vector2 location;

    //Player things ---------------------------
    public static float maxHealth = 5;
    public static float playerSpeed = 5f;
    public static float timeInvincible = 1.0f;
    public static bool canShoot = true;
    public static float cooldown = 0.4f;
    public static float weaponTime = 0f;
    public static int currentMoney;
    public static float currentXp;
    public static int level = 0;
    public static float requiredXp = 100;
    public static float totalXp;
    public static float currentHealth;
    public static int statPoints;
    public static int perkPoints;

    //----------------------------------------

    float invincibleTimer;
    bool isInvincible;
    float deathDelay = 4.0f;
    private bool dead = false;
    SpriteRenderer rend;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        PlayerUI.instance.SetXPValue(currentXp);
        canShoot = true;
        if (currentXp == 0)
        {
            currentHealth = maxHealth;
        }
        UIHealthbar.instance.SetValue(currentHealth / (float)maxHealth);
        Weapons.Default();
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    void Update()
    {
        
        MoveUp();
        MoveDown();
        MoveLeft();
        MoveRight();
        Weapons.PistolSwitch();
        Weapons.ShotgunSwitch();
        Weapons.MagnumSwitch();
        Weapons.BouncerSwitch();
        Weapons.LauncherSwitch();
        Stage();
        GodMode();
        Win();
        Money();
        Weapons.AllWeapons();

        if (currentHealth <= 0)
        {
            DeadPlayer();
        }

        location = this.gameObject.transform.position;

        lookDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        lookDirection.Normalize();

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NPC character = hit.collider.GetComponent<NPC>();
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
        }

        anim.SetFloat("Look X", lookDirection.x);
        anim.SetFloat("Look Y", lookDirection.y);

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKey(KeyCode.Mouse0))
        {
            if (!dead && canShoot)
            {
                StartCoroutine(Launch());
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other != null)
        {
            switch (other.gameObject.tag)
            {
                case "Shotgun":
                    Weapons.shotgunAmmo += 4;
                    Weapons.pickedUpShotgun = true;
                    if (Weapons.hasShotgun)
                    {
                        PlayerUI.instance.SetAmmo(Weapons.shotgunAmmo);
                    }
                    audioSource.volume = 1.5f;
                    PlaySound(pickupSound);
                    Destroy(other.gameObject);
                    break;
                case "Bouncer":
                    Weapons.bouncerAmmo += 6;
                    Weapons.pickedUpBouncer = true;
                    if (Weapons.hasBouncer)
                    {
                        PlayerUI.instance.SetAmmo(Weapons.bouncerAmmo);
                    }
                    audioSource.volume = 1.5f;
                    PlaySound(pickupSound);
                    Destroy(other.gameObject);
                    break;
                case "Magnum":
                    Weapons.magnumAmmo += 4;
                    Weapons.pickedUpMagnum = true;
                    if (Weapons.hasMagnum)
                    {
                        PlayerUI.instance.SetAmmo(Weapons.magnumAmmo);
                    }
                    audioSource.volume = 1.5f;
                    PlaySound(pickupSound);
                    Destroy(other.gameObject);
                    break;
                case "Launcher":
                    Weapons.launcherAmmo += 3;
                    Weapons.pickedUpLauncher = true;
                    if (Weapons.hasLauncher)
                    {
                        PlayerUI.instance.SetAmmo(Weapons.launcherAmmo);
                    }
                    audioSource.volume = 1.5f;
                    PlaySound(pickupSound);
                    Destroy(other.gameObject);
                    break;
                default:
                    break;
            }
            audioSource.volume = .165f;
        }
    }

    public void ChangeHealth(int amount)
    {
        amount += LevelManager.difficulty;
        if (Perks.evasion)
        {
            int rand = UnityEngine.Random.Range(1, 100);
            if (rand > 75)
            {
                return;
            }
        }
        if (amount > 0)
        {
            StartCoroutine("SwitchColorGreen");
        }
        if (amount < 0)
        {
            if (!isInvincible)
            {
                StartCoroutine("SwitchColorRed");
                anim.SetTrigger("Hit");
                PlaySound(hitSound);
            }
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);


        UIHealthbar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    public void ChangeMoney(int amount)
    {
        if (LevelManager.difficulty != 0)
        {
            amount = Convert.ToInt32(amount + LevelManager.difficulty * 1.1f);
        }
        currentMoney += amount;
    }

    public void ChangeXp(int amount)
    {
        if (LevelManager.difficulty != 0)
        {
            amount = Convert.ToInt32(amount + LevelManager.difficulty * 1.1f);
        }
        currentXp = currentXp + amount;
        totalXp = totalXp + amount;
        PlayerUI.instance.SetXPValue(currentXp);
        if (currentXp >= requiredXp)
        {
            ++level;
            currentXp = 0;
            requiredXp = requiredXp * 1.1f;
            PlayerUI.instance.SetXPValue(0);
            statPoints++;
            LevelEnd.instance.IncreaseStatPoints();
            if (level % 2 == 0)
            {
                perkPoints++;
                LevelEnd.instance.IncreasePerkPoints();
            }
        }
    }
    IEnumerator Launch()
    {
        canShoot = false;
        if (Weapons.hasPistol)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody.position + Vector2.left * 0.2f + lookDirection * 0.4f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();

            projectile.Launch(700);
            PlaySound(shoot);
        }

        if (Weapons.hasMagnum && Weapons.magnumAmmo > 0)
        {
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody.position + Vector2.left * 0.2f + lookDirection * 0.4f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();

            projectile.Launch(800);
            PlaySound(magnumSound);

            Weapons.magnumAmmo--;
            PlayerUI.instance.SetAmmo(Weapons.magnumAmmo);
        }

        if (Weapons.hasShotgun && Weapons.shotgunAmmo > 0)
        {
            GameObject shotgunShell1 = Instantiate(projectilePrefab, rigidbody.position + Vector2.left * 0.2f + lookDirection * 0.3f, Quaternion.identity);
            GameObject shotgunShell2 = Instantiate(projectilePrefab, rigidbody.position + Vector2.left * 0.4f + lookDirection * 0.4f, Quaternion.identity);
            GameObject shotgunShell3 = Instantiate(projectilePrefab, rigidbody.position + Vector2.left * 0.6f + lookDirection * 0.6f, Quaternion.identity);

            Projectile projectile1 = shotgunShell1.GetComponent<Projectile>();
            Projectile projectile2 = shotgunShell2.GetComponent<Projectile>();
            Projectile projectile3 = shotgunShell3.GetComponent<Projectile>();

            projectile1.Launch(700);
            projectile2.Launch(700);
            projectile3.Launch(700);
            PlaySound(shotgunSound);

            Weapons.shotgunAmmo--;
            PlayerUI.instance.SetAmmo(Weapons.shotgunAmmo);
        }

        if (Weapons.hasBouncer && Weapons.bouncerAmmo > 0)
        {
            GameObject projectileObject = Instantiate(bouncerPrefab, rigidbody.position + Vector2.left * 0.2f + lookDirection * 0.4f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();

            projectile.Launch(900);
            PlaySound(bouncerSound);
            Weapons.bouncerAmmo--;
            PlayerUI.instance.SetAmmo(Weapons.bouncerAmmo);
        }

        if (Weapons.hasLauncher && Weapons.launcherAmmo > 0)
        {

            GameObject projectileObject = Instantiate(grenade, rigidbody.position + Vector2.left * 0.2f + lookDirection * 0.4f, Quaternion.identity);

            Projectile projectile = projectileObject.GetComponent<Projectile>();

            projectile.Launch(900);
            PlaySound(launcherSound);
            Weapons.launcherAmmo--;
            PlayerUI.instance.SetAmmo(Weapons.launcherAmmo);
        }

        yield return new WaitForSeconds(cooldown + weaponTime);
        canShoot = true;
    }

    private void DeadPlayer()
    {
        anim.SetTrigger("Death");
        dead = true;
        deathDelay -= Time.deltaTime;
        if (deathDelay <= 0)
        {
            currentHealth = 5;
            maxHealth = 5;
            Weapons.PistolSwitch();
            dead = false;
            currentXp = 0;
            totalXp = 0;
            level = 0;
            PlayerUI.instance.SetXPValue(0);

            Weapons.bouncerAmmo = 0;
            Weapons.shotgunAmmo = 0;
            Weapons.launcherAmmo = 0;
            Weapons.magnumAmmo = 0;
            Weapons.hasPistol = true;
            Weapons.pickedUpShotgun = false;
            Weapons.pickedUpMagnum = false;
            Weapons.pickedUpLauncher = false;
            Weapons.pickedUpBouncer = false;

            SceneManager.LoadScene(sceneBuildIndex: 0);
        }
    }

    IEnumerator SwitchColorRed()
    {
        rend.material.color = new Color(1f, 0.30196078f, 0.30196078f);
        yield return new WaitForSeconds(.3f);
        rend.material.color = Color.white;
    }

    IEnumerator SwitchColorGreen()
    {
        rend.material.color = new Color(.05f, 1f, 0f);
        yield return new WaitForSeconds(.3f);
        rend.material.color = Color.white;
    }

    void CheckAndMove()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        Vector2 position = rigidbody.position;

        position = position + move * playerSpeed * Time.deltaTime;
        rigidbody.MovePosition(position);
    }

    void MoveUp()
    {
        if (Input.GetKey(KeyCode.W))
        {
            CheckAndMove();
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            anim.SetTrigger("RunUp");
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            anim.SetTrigger("IdleUp");
        }
    }
    void MoveDown()
    {
        if (Input.GetKey(KeyCode.S))
        {
            CheckAndMove();
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            anim.SetTrigger("RunDown");
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            anim.SetTrigger("IdleDown");
        }
    }
    void MoveLeft()
    {
        if (Input.GetKey(KeyCode.A))
        {
            CheckAndMove();
        }
        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetTrigger("RunLeft");
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetTrigger("IdleLeft");
        }
    }
    void MoveRight()
    {
        if (Input.GetKey(KeyCode.D))
        {
            CheckAndMove();
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetTrigger("RunRight");
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetTrigger("IdleRight");
        }
    }

    void Stage()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            LevelManager.stage++;
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            ChangeXp(100);
            currentHealth = maxHealth;
        }
    }

    void Win()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            LevelManager.WinCheck();
        }
    }
    void Money()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            currentMoney += 100;
        }
    }

    void GodMode()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            isInvincible = true;
            invincibleTimer = 100f;
        }
    }
}

