using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    public Text numFruitText, healthText, stageText, numBulletsText;

    public Image fruitSprite;

    public void UpdateMenu()
    {
        numFruitText.text = PlayerStats.fruitsCollected.ToString();
        healthText.text = PlayerStats.currentHealth.ToString();
        numBulletsText.text = PlayerController.amountOfBullets.ToString();
        stageText.text = "Not Implemented";
    }
}
