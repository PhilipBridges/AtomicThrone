using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    Animator anim;
    public float playerSpeed = 5.0f;
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        MoveUp();
        MoveDown();
        MoveLeft();
        MoveRight();
    }
    void MoveUp()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.Translate(Vector3.up * playerSpeed * Time.deltaTime);
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
            transform.Translate(Vector3.down * playerSpeed * Time.deltaTime);
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
            transform.Translate(Vector3.left * playerSpeed * Time.deltaTime);
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
            transform.Translate(Vector3.right * playerSpeed * Time.deltaTime);
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
}
