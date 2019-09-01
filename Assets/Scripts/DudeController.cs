using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DudeController : MonoBehaviour
{
    Animator anim;
    new Rigidbody2D rigidbody;
    public bool open = false;

    public GameObject menu;
    public GameObject projectilePrefab;

    private AudioSource audioSource;
    public AudioClip throwSound;
    public AudioClip shoot;
    public AudioClip hitSound;

    Vector2 movementDirection;
    Vector2 lookDirection = new Vector2(1, 0);
    
    public int maxHealth = 5;
    public float playerSpeed = 6f;
    public float timeInvincible = 1.0f;
    public int health { get { return currentHealth; } }
    int currentHealth;
    float invincibleTimer;
    bool isInvincible;
    float deathDelay = 4.0f;
    private bool dead = false;
    bool damaged = false;
    private float cooldown = 0.1f;
    private bool canShoot = true;
    public static int currentMoney;
    SpriteRenderer rend;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
        menu.gameObject.SetActive(false);
        currentHealth = maxHealth;
    }

    public void PlaySound(AudioClip clip)
    {
        if (audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            anim.SetTrigger("Death");
            dead = true;
            deathDelay -= Time.deltaTime;
            if (deathDelay <= 0)
            {
                Scene thisScene = SceneManager.GetActiveScene();
                SceneManager.LoadScene(thisScene.name);
            }
        }

        MoveUp();
        MoveDown();
        MoveLeft();
        MoveRight();
        OpenMenu();
        CloseMenu();

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector2 move = new Vector2(horizontal, vertical);

        float mouseX = Input.mousePosition.x;
        float mouseY = Input.mousePosition.y;


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

        if (horizontal < 0.0f)
        {
            anim.SetTrigger("RunLeft");
        }

        if (vertical < 0.0f)
        {
            anim.SetTrigger("RunRight");
        }

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKey(KeyCode.Mouse0) && canShoot == true && !dead && !open)
        {
            StartCoroutine(Launch());
        }
    }


    public void ChangeHealth(int amount)
    {
        if (dead)
        {
            return;
        }
        if(amount > 0)
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
                damaged = true;
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
        currentMoney += amount;
    }
    IEnumerator Launch()
    {
        canShoot = false;
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody.position + Vector2.left * 0.2f + lookDirection * 0.4f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();

        projectile.Launch(700);
        PlaySound(shoot);
        PlaySound(throwSound);
        yield return new WaitForSeconds(cooldown);
        canShoot = true;
    }

    IEnumerator SwitchColorRed()
    {
        rend.material.color = new Color(1f, 0.30196078f, 0.30196078f);
        yield return new WaitForSeconds(.3f);
        rend.material.color = Color.white;
        damaged = false;
    }

    IEnumerator SwitchColorGreen()
    {
        rend.material.color = new Color(.05f, 1f, 0f);
        yield return new WaitForSeconds(.3f);
        rend.material.color = Color.white;
        damaged = false;
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

    void OpenMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape)){
            StartCoroutine(MenuCooldown(true));
            menu.SetActive(true);
        }
    }

    public void CloseMenu()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && open)
        {
            StartCoroutine(MenuCooldown(false));
            menu.SetActive(false);
        }
    }

    public void BackClick()
    {
        StartCoroutine(MenuCooldown(false));
        menu.SetActive(false);
    }

    IEnumerator MenuCooldown(bool val)
    {
        yield return new WaitForSeconds(.1f);
        open = val;
    }

    public void QuitToMenu()
    {
        Manager.BackToMenu();
    }
}

