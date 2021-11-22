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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            showJump = true;
        }
        else
        {
            showJump = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerController player))
        {
            showJump = false;
        }
    }
}
