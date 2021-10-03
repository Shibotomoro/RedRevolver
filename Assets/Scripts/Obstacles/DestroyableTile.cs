using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableTile : MonoBehaviour
{
    private PlayerController playerController;
    private GameObject player;
    public GameObject breakAnimationEffect;

    private bool playerInRange;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (playerInRange && playerController.isDashing)
        {
            Instantiate(breakAnimationEffect, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
