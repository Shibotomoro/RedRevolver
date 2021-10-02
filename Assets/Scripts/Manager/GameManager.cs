using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Animator transitionAnim;
    public float transitionTime = 1f;

    public GameObject player;
    public GameObject[] spawnLocations;
    public GameObject[] activeRooms;

    private static float playerPosX = 0;
    private static float playerPosY = 0;
    private static bool reload;

    private int roomTracker = 0;

    private void Awake()
    {
        if (reload)
        {
            player.transform.position = new Vector3(playerPosX, playerPosY, 0);
        }
        instance = this;
        spawnLocations = GameObject.FindGameObjectsWithTag("Respawn");
    }

    private void Update()
    {
        for (int i = 0; i < activeRooms.Length; i++)
        {
            if (activeRooms[i].activeSelf == true)
            {
                roomTracker = i;
            }
        }
    }

    public void Respawn()
    {
        reload = true;
        playerPosX = spawnLocations[roomTracker].transform.position.x;
        playerPosY = spawnLocations[roomTracker].transform.position.y;
        ReloadScene();
    }

    public void ReloadScene()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex));
    }

    public void LoadNextLevel()
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    IEnumerator LoadLevel(int levelIndex)
    {
        transitionAnim.SetTrigger("FadeStart");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(levelIndex);
    }
}
