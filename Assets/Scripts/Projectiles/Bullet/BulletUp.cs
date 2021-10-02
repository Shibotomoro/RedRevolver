using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//hi this is ted
public class BulletUp : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 50;

    public Rigidbody2D RB;

    private void Start()
    {
        RB.velocity = transform.up * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag != "Level" && hitInfo.gameObject.tag != "Player" && hitInfo.gameObject.tag != "NonShootable")
        {
            EnemyDeath enemyDeath = hitInfo.GetComponent<EnemyDeath>();
            if (enemyDeath != null)
            {
                enemyDeath.TakeDamage(damage);
            }
            Enemy enemy = hitInfo.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }
            Destroy(gameObject);
        }
    }
}