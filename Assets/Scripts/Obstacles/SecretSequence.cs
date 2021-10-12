using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecretSequence : MonoBehaviour
{
    private bool playerInRange;
    private bool secretUnlocked = false;
    public GameObject Collectible;

    private KeyCode[] sequence = new KeyCode[]
    {
        KeyCode.T,
        KeyCode.E,
        KeyCode.S,
        KeyCode.T,
        //KeyCode.W,
        //KeyCode.A,
        //KeyCode.T,
        //KeyCode.C,
        //KeyCode.H,
        //KeyCode.Space,
        //KeyCode.T,
        //KeyCode.H,
        //KeyCode.E,
        //KeyCode.Space,
        //KeyCode.Y,
        //KeyCode.A,
        //KeyCode.R,
        //KeyCode.D,
    };

    private int sequenceIndex;

    private void Update()
    {
        if (playerInRange && !secretUnlocked)
        {
            if (Input.GetKeyDown(sequence[sequenceIndex]))
            {
                if (++sequenceIndex == sequence.Length)
                {
                    secretUnlocked = true;
                    Instantiate(Collectible, transform.position, Quaternion.identity);
                }
            }
            else if (Input.anyKeyDown)
            {
                sequenceIndex = 0;
            }
        }
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
