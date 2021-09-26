using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Transform respawnPoint;
    public GameObject playerPrefab;

    public CinemachineVirtualCamera CVC;

    private void Awake()
    {
        instance = this;
    }

    public void Respawn()
    {
        GameObject Player = Instantiate(playerPrefab, respawnPoint.position, Quaternion.identity);
        CVC.Follow = Player.transform;
    }



    //private float respawnTimeStart;

    //private bool respawn;

    //private CinemachineVirtualCamera CVC;

    //private void Start()
    //{
    //    Invoke("InitCamera", 1f);
    //}

    //private void InitCamera()
    //{
    //    CVC = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
    //}

    //public void Respawn()
    //{
    //    respawnTimeStart = Time.time;
    //    respawn = true;
    //}

    //private void CheckRespawn()
    //{
    //    if (Time.time >= respawnTimeStart + respawnTime && respawn)
    //    {
    //        var playerTemp = Instantiate(player, respawnPoint);
    //        CVC.m_Follow = playerTemp.transform;
    //        respawn = false;
    //    }
    //}

}
