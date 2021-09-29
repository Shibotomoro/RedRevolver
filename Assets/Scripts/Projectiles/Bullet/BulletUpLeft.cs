using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//hi this is ted
public class BulletUpLeft : MonoBehaviour
{
    public float speed = 20f;
    public int damage = 50;

    public Rigidbody2D RB;

    private float moveX = -1f;
    private float moveY = 1f;

    private void Start()
    {
        Vector3 moveDir = new Vector3(moveX, moveY).normalized;
        RB.velocity = moveDir * speed;
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag != "Level" && hitInfo.gameObject.tag != "Player")
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