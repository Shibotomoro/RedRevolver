using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject player;
    public GameObject playerPrefab;
    public GameObject[] spawnLocations;
    public GameObject[] activeRooms;

    private static float playerPosX = 0;
    private static float playerPosY = 0;
    private static bool reload;

    private int roomTracker = 0;

    public CinemachineVirtualCamera CVC;

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
        GameObject Player = Instantiate(playerPrefab, spawnLocations[roomTracker].transform.position, Quaternion.identity);
        CVC.Follow = Player.transform;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
