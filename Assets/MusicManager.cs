using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;
    public AudioSource music;
    public AudioClip level1Music;
    
    public static string musicToPlay;
    private static int current = -3;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
    }


    private void Start()
    {
        level1Music = Resources.Load<AudioClip>("Level 1 Music");
        
        music = gameObject.GetComponent<AudioSource>();

    }

    public void ChangeMusic()
    {


    }
    void Update()
    {
        if (current != -2)
        {
            if (musicToPlay == "Level 1 Music")
            {
                if (current == 0)
                {
                    current = -1;
                    music.clip = level1Music;
                    music.Play();
                }
            }

        }

    }
    public void PlayMusic(string clip)
    {
        if (current != -1)
        {
            if (clip == "Level 1 Music")
            {
                music.volume = 0.054f;
                current = 0;
            }
        }
        musicToPlay = clip;
    }
    public void StopMusic()
    {
        current = -2;
        music.Stop();

    }
}