using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialGuideScript : MonoBehaviour
{

    private Animator anim;
    private bool showJump = false;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        anim.SetBool("showJump", showJump);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            showJump = true;
        }
        else
        {
            showJump = false;
        }
    }
}
