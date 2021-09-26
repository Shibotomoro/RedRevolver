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
        //Check if player collides with flying enemy

        if (col.gameObject.tag.Equals("Enemy"))
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
