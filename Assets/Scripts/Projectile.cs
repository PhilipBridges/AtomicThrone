    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector3 aimDirection;
    Rigidbody2D rigidbody2d;
    public GameObject explosion;
    private bool boom = false;
    public float dmgValue = 0;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        transform.position = transform.position + aimDirection * Time.deltaTime;

        aimDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        aimDirection.z = 0;
        aimDirection.Normalize();

        if (Weapons.hasLauncher)
        {
            dmgValue = 5;
        }

        if (Weapons.hasMagnum)
        {
            dmgValue = 6.5f;
        }

        if (Weapons.hasPistol)
        {
            dmgValue = 1;
        }

        if (Weapons.hasShotgun)
        {
            dmgValue = 1.8f;
        }

        if (Weapons.hasBouncer)
        {
            dmgValue = 2f;
        }
    }

    void Update()
    {
        if (transform.position.magnitude > 75.0f)
        {
            Destroy(gameObject);
        }

        if (Weapons.hasShotgun)
        {
            StartCoroutine(KillTime(gameObject, .3f));
        }
        if (Weapons.hasBouncer)
        {
            StartCoroutine(KillTime(gameObject, 3.5f));
        }
        if (Weapons.hasLauncher)
        {
            StartCoroutine(Detonate(gameObject, 1.5f));
        }
    }

    IEnumerator Detonate(GameObject gameobject, float time)
    {
        yield return new WaitForSeconds(time);
        if (!boom)
        {
            Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    IEnumerator KillTime(GameObject gameobject, float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
    public void Launch(float force)
    {
        if (Weapons.hasShotgun)
        {
            rigidbody2d.AddForce((aimDirection * .2f) * force);
        }
        rigidbody2d.AddForce(aimDirection * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (Weapons.hasBouncer)
        {
            gameObject.GetComponent<AudioSource>().Play();
        }

        if (!Weapons.hasBouncer && !Weapons.hasLauncher)
        {
            
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Weapons.hasLauncher && collision.tag != "Bouncer" && collision.tag != "Magnum" && collision.tag != "Launcher" && collision.tag != "Launcher")
        {
            Instantiate(explosion, gameObject.transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

}

