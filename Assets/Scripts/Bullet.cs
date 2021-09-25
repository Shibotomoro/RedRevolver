using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;

    public Rigidbody2D RB;

    private void Start()
    {
        RB.velocity = transform.right * speed; }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        if (hitInfo.gameObject.tag != "Level" && hitInfo.gameObject.tag != "Player")
        {
            Destroy(gameObject);
        }
    }
}
