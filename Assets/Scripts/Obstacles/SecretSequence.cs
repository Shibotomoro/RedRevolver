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

    private KeyCode[] sequence = new KeyCode[]
    {
        KeyCode.UpArrow,
        KeyCode.UpArrow,
        KeyCode.DownArrow,
        KeyCode.DownArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.LeftArrow,
        KeyCode.RightArrow,
        KeyCode.A,
        KeyCode.B
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
                    StartCoroutine(SpawnCollectible());
                }
            }
            else if (Input.anyKeyDown)
            {
                sequenceIndex = 0;
            }
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
