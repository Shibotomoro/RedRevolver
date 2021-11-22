using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeverController : MonoBehaviour
{
    public bool isOn;
    public float destroyDoorTime = 0.5f;
    public float waitForDoorRespawn = 4.0f;
    public Animator anim;
    public GameObject door;

    private bool inUse = false;
    //Vector keep track of our scale
    private Vector3 initScale;

    public void LeverToggle()
    {
        if (!isOn && !inUse)
        {
            inUse = true;
            isOn = true;
            anim.SetBool("isOn", isOn);
            DestroyDoor();
        }
        else if (isOn && !inUse)
        {
            isOn = false;
            anim.SetBool("isOn", isOn);
        }
    }

    public void DestroyDoor()
    {
        StartCoroutine(DoorDestroy());
        StartCoroutine(DoorRespawn());
    }

    IEnumerator DoorDestroy()
    {
        yield return new WaitForSeconds(destroyDoorTime);
        initScale = door.transform.localScale;
        door.transform.localScale = Vector3.zero;
    }

    IEnumerator DoorRespawn()
    {
        yield return new WaitForSeconds(waitForDoorRespawn);
        door.transform.localScale = initScale;
        inUse = false;
        isOn = false;
        anim.SetBool("isOn", isOn);
    }
}
