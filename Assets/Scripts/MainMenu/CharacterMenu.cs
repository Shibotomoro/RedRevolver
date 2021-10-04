using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    public Text numFruitText, healthText, stageText, numBulletsText;
    public Image[] bulletSprites;

    public Image fruitSprite;

    private PlayerController pc;
    private GameObject player;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.GetComponent<PlayerController>();
        bulletSprites[0].enabled = false;
        bulletSprites[1].enabled = false;
        bulletSprites[2].enabled = false;
        bulletSprites[3].enabled = false;
        bulletSprites[4].enabled = false;
        

    }

    public void Update()
    {
        UpdateBulletUI();
    }
    public void UpdateMenu()
    {
        numFruitText.text = PlayerStats.fruitsCollected.ToString();
        healthText.text = PlayerStats.currentHealth.ToString();
        numBulletsText.text = pc.amountOfBullets.ToString();
        stageText.text = "Not Implemented";
    }

    public void UpdateBulletUI()
    {
        
        for(int i=0; i < pc.amountOfBullets-1; i++)
        {
            bulletSprites[i].enabled = false;
        }

        for (int i = pc.amountOfBullets; i < 6; i++)
        {
            bulletSprites[i].enabled = false;
        }
        bulletSprites[pc.amountOfBullets-1].enabled = true;
    }
    
}
