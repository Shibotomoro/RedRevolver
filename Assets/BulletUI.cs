using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BulletUI : MonoBehaviour
{
    public Image[] bulletSprites;

    private PlayerController pc;
    private GameObject player;
    
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.GetComponent<PlayerController>();
        bulletSprites[0].enabled = false;
        bulletSprites[1].enabled = false;
        bulletSprites[2].enabled = false;
        bulletSprites[3].enabled = false;
        bulletSprites[4].enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        UpdateBulletUI();
    }

    public void UpdateBulletUI()
    {

        for (int i = 0; i < pc.amountOfBullets - 1; i++)
        {
            bulletSprites[i].enabled = false;
        }

        for (int i = pc.amountOfBullets; i < 6; i++)
        {
            bulletSprites[i].enabled = false;
        }
        bulletSprites[pc.amountOfBullets - 1].enabled = true;
    }

}
