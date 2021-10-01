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
        if (hitInfo.gameObject.tag != "Level")
        {
            Instantiate(explosionEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }

    //private AttackDetails attackDetails;

    //private float speed;
    //private float travelDistance;
    //private float xStartPos;
    //[SerializeField] private float gravity;
    //[SerializeField] private float damageRadius;

    //private bool isGravityOn;
    //private bool hasHitGround;

    //private Rigidbody2D rb;

    //[SerializeField] private LayerMask whatIsGround;
    //[SerializeField] private LayerMask whatIsPlayer;
    //[SerializeField] private Transform damagePosition;

    //private void Start()
    //{
    //    rb = GetComponent<Rigidbody2D>();
    //    rb.gravityScale = 0.0f;
    //    rb.velocity = transform.right * speed;

    //    xStartPos = transform.position.x;
    //    isGravityOn = false;
    //}

    //private void Update()
    //{
    //    if (!hasHitGround)
    //    {
    //        attackDetails.position = transform.position;
    //        if (isGravityOn)
    //        {
    //            float angle = Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg;
    //            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
    //        }
    //    }
    //}

    //private void FixedUpdate()
    //{
    //    if (!hasHitGround)
    //    {
    //        Collider2D damageHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsPlayer);
    //        Collider2D groundHit = Physics2D.OverlapCircle(damagePosition.position, damageRadius, whatIsGround);
    //        if (damageHit)
    //        {
    //            damageHit.transform.SendMessage("Damage", attackDetails);
    //            Destroy(gameObject);
    //        }

    //        if (groundHit)
    //        {
    //            hasHitGround = true;
    //            rb.gravityScale = 0.0f;
    //            rb.velocity = Vector2.zero;
    //        }

    //        if (Mathf.Abs(xStartPos - transform.position.x) >= travelDistance && !isGravityOn)
    //        {
    //            isGravityOn = true;
    //            rb.gravityScale = gravity;
    //        }
    //    }
    //}

    //public void FireProjectile(float speed, float travelDistance, float damage)
    //{
    //    this.speed = speed;
    //    this.travelDistance = travelDistance;
    //    attackDetails.damageAmount = damage;
    //}

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(damagePosition.position, damageRadius);
    //}
}
