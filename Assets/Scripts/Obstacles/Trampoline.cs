using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float bounce = 5f;
    private GameObject player;
    private Animator anim;
    private bool isPushing;
    private BoxCollider2D boxCollider;

    [SerializeField] private float pushingTimer = .1f;


    private void Start()
    {
        anim = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        anim.SetBool("isPushing", isPushing);
        if (isPushing)
        {
            if (pushingTimer <= 0)
            {
                isPushing = false;
                pushingTimer = .1f;
            }
            else
            {
                pushingTimer -= Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            isPushing = true;
            player.GetComponent<PlayerController>().dashTimeLeft = 0.0f;
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounce, ForceMode2D.Impulse);
            boxCollider.enabled = false;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            boxCollider.enabled = true;
        }
    }

}
