using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject playerPrefab;
    public GameObject[] spawnLocations;
    public GameObject[] activeRooms;

    private int roomTracker;

    public CinemachineVirtualCamera CVC;

    private void Awake()
    {
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
        GameObject Player = Instantiate(playerPrefab, spawnLocations[roomTracker].transform.position, Quaternion.identity);
        CVC.Follow = Player.transform;
    }
}
