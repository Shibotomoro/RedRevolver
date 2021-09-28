using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50.0f;

    private float currentHealth;

    private GameManager GM;


    private void OnCollisionEnter2D(Collision2D col)
    {

        if (col.gameObject.tag.Equals("Enemy"))
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

    private void Update()
    {

    }

    private void Start()
    {
        currentHealth = maxHealth;
        GM = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    private void Damage(AttackDetails attackDetails)
    {
        DecreaseHealth(attackDetails.damageAmount);
    }

    public void DecreaseHealth(float amount)
    {
        currentHealth -= amount;
        if (currentHealth <= 0.0f)
        {
            Die();
        }
    }

    private void Die()
    {
        GM.Respawn();
        Destroy(gameObject);
    }
}
