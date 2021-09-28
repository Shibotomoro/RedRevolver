using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float upCap;
    [SerializeField] private float downCap;
    [SerializeField] private float flightSpeed;
    private Rigidbody2D rb;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        if (transform.position.y >= upCap)
        {
            rb.velocity = new Vector2(0.0f, -flightSpeed);
        }
        else if (transform.position.y <= downCap)
        {
            rb.velocity = new Vector2(0.0f, flightSpeed);
        }
    }

    public int health = 100;

    public GameObject deathEffect;

    public void TakeDamage(int damage)
    {
        health -= damage;

        if (health <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag == "Projectile")
        {
            Die();
        }
    }

    private void Die()
    {
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
