using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeath : MonoBehaviour
{
    private Rigidbody2D RB;

    protected void Start()
    {
        RB = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

    }

    public int health = 50;
    public GameObject deathEffect;
    public GameObject AmmoRefillPrefab;

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
        Instantiate(AmmoRefillPrefab, transform.position, Quaternion.identity);
        Destroy(transform.parent.gameObject);
    }
}
