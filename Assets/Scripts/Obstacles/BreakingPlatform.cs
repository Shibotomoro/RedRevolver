using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakingPlatform : MonoBehaviour
{
    public float timeToBreak = 1f;
    public float timeToRespawn = 1f;
    public GameObject breakAnimation;
    public GameObject parentObj;
    public GameObject player;

    private Vector3 holdScale;
    private PlayerController playerController;

    private void Awake()
    {
        playerController = player.GetComponent<PlayerController>();
    }

    private void Start()
    {
        parentObj = transform.parent.gameObject;
        holdScale = transform.localScale;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (playerController.isGrounded)
            {
                StartCoroutine(Break());
            }
        }
    }

    IEnumerator Break()
    {
        yield return new WaitForSeconds(timeToBreak);
        transform.localScale = Vector3.zero;
        parentObj.transform.localScale = Vector3.zero;
        Instantiate(breakAnimation, transform.position, Quaternion.identity);
        
        yield return new WaitForSeconds(timeToRespawn);
        transform.localScale = holdScale;
        parentObj.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
