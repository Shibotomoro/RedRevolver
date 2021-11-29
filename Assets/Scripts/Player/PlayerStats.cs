using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    [SerializeField] private float maxHealth = 50.0f;

    public static float currentHealth;
    public static int fruitsCollected = 0;
    //public static int numBullets = 0;

    private GameManager GM;
    private GameObject spikeObject;
    public GameObject deathEffect;
    public PlayerController p;

    private bool isCollidingWithSpike;



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
        if (hitInfo.gameObject.tag == "Spike")
        {
            spikeObject = hitInfo.gameObject;
            isCollidingWithSpike = true;
            // Facing Left
        }
    }

    private void OnTriggerExit2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag == "Spike")
        {
            isCollidingWithSpike = false;
        }
    }


    private void Update()
    {
        CheckSpikeCollision();
    }

    private void Start()
    {
        currentHealth = maxHealth;
        //numBullets = p.GetComponent<PlayerController>().amountOfBullets;
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

    public void Die()
    {
        SoundManagerScript.PlaySound("playerDeath");
        Destroy(gameObject);
        Instantiate(deathEffect, transform.position, Quaternion.identity);
        GM.Respawn();
    }

    private void CheckSpikeCollision()
    {
        if (isCollidingWithSpike)
        {
            if (spikeObject.transform.rotation.eulerAngles.z == 90)
            {
                if (p.RB.velocity.x <= 0.0) { }
                else
                {
                    Die();
                }
            }
            // Facing Down
            else if (spikeObject.transform.rotation.eulerAngles.z == 180)
            {
                if (p.RB.velocity.y <= 0.0) { }
                else
                {
                    Die();
                }
            }
            // Facing Right
            else if (spikeObject.transform.rotation.eulerAngles.z == 270)
            {
                if (p.RB.velocity.x >= 0.0) { }
                else
                {
                    Die();
                }
            }
            // Facing Up
            else
            {
                if (p.RB.velocity.y >= 0.0) { }
                else
                {
                    Die();
                }
            }
        }
    }

}
