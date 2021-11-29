using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline : MonoBehaviour
{
    public float bounce = 25f;
    private GameObject player;
    private Animator anim;
    private bool isPushing;


    private void Start()
    {
        anim = GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        anim.SetBool("isPushing", isPushing);
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            SoundManagerScript.PlaySound("trampoline");
            isPushing = true;
            player.GetComponent<PlayerController>().dashTimeLeft = 0.0f;
            player.GetComponent<Rigidbody2D>().velocity = Vector2.up * bounce;
        }
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            isPushing = false;
        }
    }

}
