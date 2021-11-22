using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretSequence : MonoBehaviour
{
    private bool playerInRange;
    private bool secretUnlocked = false;
    public float waitForAnimation = 0.80f;
    public GameObject Collectible;
    public GameObject SpawnAnimation;

    private int dashCounter = 0;

    private void Update()
    {
        if (playerInRange && !secretUnlocked)
        {
            if (Input.GetButtonDown("Horizontal"))
            { 
                dashCounter++;
                if (dashCounter > 50)
                {
                    secretUnlocked = true;
                    StartCoroutine(SpawnCollectible());
                }
            }
            else if (Input.anyKeyDown)
            {
                dashCounter = 0;
            }
        }

        if (!playerInRange)
        {
            dashCounter = 0;
        }
    }

    IEnumerator SpawnCollectible()
    {
        Instantiate(SpawnAnimation, transform.position, Quaternion.identity);
        yield return new WaitForSeconds(waitForAnimation);
        Instantiate(Collectible, transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

}
