using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoRefill : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D coll)
    {
        if (coll.gameObject.tag == "Player")
        {
            coll.gameObject.GetComponent<PlayerController>().RefillAmmo();
            Destroy(gameObject);
        }
    }
}
