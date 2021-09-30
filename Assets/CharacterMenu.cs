using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterMenu : MonoBehaviour
{
    //Text Fields
    public Text stageNumText, hitPointText, numFruitText, thingText;

    //Logic
    public Image fruitSprite;

    public void UpdateMenu()
    {


        hitPointText.text = PlayerStats.currentHealth.ToString();
        numFruitText.text = PlayerStats.fruitCollected.ToString();
        stageNumText.text = "NOT IMPLEMENTED";

    }
}
