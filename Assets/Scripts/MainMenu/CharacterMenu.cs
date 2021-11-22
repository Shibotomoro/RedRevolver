using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterMenu : MonoBehaviour
{
    public Text numFruitText, healthText, stageText, numBulletsText;

    [SerializeField] GameObject pauseMenu;
    public static bool isPaused = false;

    public Image fruitSprite;

    private PlayerController pc;
    private GameObject player;

    public void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        pc = player.GetComponent<PlayerController>();
        pauseMenu.SetActive(false);

    }

    public void Update()
    {
        UpdateMenu();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }
    public void UpdateMenu()
    {
        numFruitText.text = PlayerStats.fruitsCollected.ToString();
        healthText.text = PlayerStats.currentHealth.ToString();
        numBulletsText.text = pc.amountOfBullets.ToString();
        stageText.text = "Not Implemented";
    }

    
    public void PauseGame()
    {
        isPaused = true;
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        
    }

    public void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        
    }

    public void ExitToMainMenu()
    {
        //TimeCount.instance.EndTimer();
        SceneManager.LoadScene("MainMenu");
    }
}
