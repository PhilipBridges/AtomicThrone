    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Vector3 aimDirection;
    Rigidbody2D rigidbody2d;

    void Awake()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        transform.position = transform.position + aimDirection * Time.deltaTime;

        aimDirection = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position);
        aimDirection.z = 0;
        aimDirection.Normalize();
    }

    void Update()
    {
        if (transform.position.magnitude > 75.0f)
        {
            Destroy(gameObject);
        }
    }

    public void Launch(float force)
    {
        rigidbody2d.AddForce(aimDirection * force);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        EnemyController e = other.collider.GetComponent<EnemyController>();
        if (e != null)
        {
            e.Damage();
        }

        Destroy(gameObject);
    }
}