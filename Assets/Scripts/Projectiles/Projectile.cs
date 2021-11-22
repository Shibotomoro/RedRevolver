using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Projectile : MonoBehaviour
{
    public Transform target;

    public float speed = 5.0f;
    public float rotateSpeed = 200f;

    public GameObject explosionEffect;

    private Rigidbody2D RB;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        RB = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {

        Vector2 direction = (Vector2)target.position - RB.position;

        direction.Normalize();

        float rotateAmount = Vector3.Cross(direction, transform.right).z;

        RB.angularVelocity = -rotateAmount * rotateSpeed;

        RB.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag != "Level" && hitInfo.gameObject.tag != "NonShootable")
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
