using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatform : MonoBehaviour
{
    public GameObject platformPathStart;
    public GameObject platformPathEnd;
    public GameObject shootTrigger;

    private PlayerController playerControllerScript;
    private Rigidbody2D playerRB;

    public float speed = 10f;
    public float velocityDampener = 0.75f;
    private Vector3 startPosition;
    private Vector3 endPosition;
    public Vector3 platformVelocityTracker;
    private bool playerIsOnPlatform;

    private void Start()
    {
        startPosition = platformPathStart.transform.position;
        endPosition = platformPathEnd.transform.position;
    }

    private void Update()
    {
        if (shootTrigger.GetComponent<ShootTrigger>().trigger)
        {
            if (transform.position == startPosition)
            {
                StartCoroutine(Vector3LerpCoroutine(gameObject, endPosition, speed));
            }
        }
        else if (!shootTrigger.GetComponent<ShootTrigger>().trigger)
        {
            if (transform.position == endPosition)
            {
                StartCoroutine(Vector3LerpCoroutine(gameObject, startPosition, speed));
            }
        }
    }
    
    IEnumerator Vector3LerpCoroutine(GameObject obj, Vector3 target, float speed)
    {
        Vector3 startPosition = obj.transform.position;
        float time = 0f;
        float startTime = 0.0f;

        while (obj.transform.position != target)
        {
            if (playerIsOnPlatform && playerControllerScript.isJumping)
            {
                platformVelocityTracker = (obj.transform.position - startPosition) / (time - startTime);
                platformVelocityTracker *= velocityDampener;
                if (platformVelocityTracker.y >= 0)
                {
                    playerRB.velocity = new Vector2(playerRB.velocity.x + platformVelocityTracker.x, playerRB.velocity.y + platformVelocityTracker.y);
                }
            }

            obj.transform.position = Vector3.Lerp(startPosition, target,
                (time / Vector3.Distance(startPosition, target)) * speed);
            time += Time.deltaTime;
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            coll.gameObject.transform.SetParent(gameObject.transform, true);
            playerRB = coll.gameObject.GetComponent<Rigidbody2D>();
            playerControllerScript = coll.gameObject.GetComponent<PlayerController>();
            playerIsOnPlatform = true;
        }
    }

    private void OnCollisionExit2D(Collision2D coll)
    {
        if (coll.gameObject.CompareTag("Player"))
        {
            coll.gameObject.transform.parent = null;
            playerIsOnPlatform = false;
        }
    }
}
