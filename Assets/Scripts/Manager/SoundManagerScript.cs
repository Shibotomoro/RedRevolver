using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerScript : MonoBehaviour
{
    public static AudioClip playerDeath;
    public static AudioClip playerRespawn;
    public static AudioClip playerDash;
    public static AudioClip playerShoot;
    public static AudioClip playerShoot2;
    public static AudioClip trampoline;
    private static AudioSource audioSrc;

    private void Start()
    {
        playerDeath = Resources.Load<AudioClip>("playerDeath");
        playerRespawn = Resources.Load<AudioClip>("playerRespawn");
        playerDash = Resources.Load<AudioClip>("playerDash");
        playerShoot = Resources.Load<AudioClip>("playerShoot");
        playerShoot2 = Resources.Load<AudioClip>("playerShoot2");
        trampoline = Resources.Load<AudioClip>("trampoline");

        audioSrc = GetComponent<AudioSource>();
        
    }

    public static void PlaySound(string clip)
    {
        switch (clip)
        {
            
            case "playerDeath":
                audioSrc.PlayOneShot(playerDeath, 0.1f); 
                break;
            case "playerRespawn":
                audioSrc.PlayOneShot(playerRespawn, 0.1f);
                break;
            case "trampoline":
                audioSrc.PlayOneShot(trampoline);
                break;
            case "playerDash":
                audioSrc.PlayOneShot(playerDash, 0.2f);
                break;
            case "playerShoot":
                audioSrc.PlayOneShot(playerShoot, 0.1f);
                break;
            case "playerShoot2":
                audioSrc.PlayOneShot(playerShoot2, 0.1f);
                break;
            default:
                break;
        }
    }
}
