using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pivot : MonoBehaviour
{
    public GameObject player;

    public void Start()
    {
        
    }

    void FixedUpdate()
    {
        Vector3 difference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;
        difference.Normalize();

        //Angle of mouse to player
        float rotationZ = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;
        

        if (rotationZ > 30 && rotationZ < 60) 
        {
            gameObject.transform.eulerAngles = new Vector3(0,0,45);
            rotationZ = transform.eulerAngles.z;
        }
        else if(rotationZ > 60 && rotationZ < 120)
        {
            gameObject.transform.eulerAngles = new Vector3(0, 0, 90);
            rotationZ = transform.eulerAngles.z;
        }
        else if (rotationZ > 120 && rotationZ < 150)
        {
            gameObject.transform.eulerAngles = new Vector3(0, 0, 135);
            rotationZ = transform.eulerAngles.z;
        }
        else if (rotationZ > 150 && rotationZ < 210)
        {
            gameObject.transform.eulerAngles = new Vector3(0, 0, 180);
            rotationZ = transform.eulerAngles.z;
        }

        else if (rotationZ < -30 && rotationZ > -60)
        {
            gameObject.transform.eulerAngles = new Vector3(0, 0, -45);
            rotationZ = transform.eulerAngles.z;
        }
        else if (rotationZ < -60 && rotationZ > -120)
        {
            gameObject.transform.eulerAngles = new Vector3(0, 0, -90);
            rotationZ = transform.eulerAngles.z;
        }
        else if (rotationZ < -120 && rotationZ > -150)
        {
            gameObject.transform.eulerAngles = new Vector3(0, 0, -135);
            rotationZ = transform.eulerAngles.z;
        }
        else if (rotationZ < 30 && rotationZ > -30)
        {
            gameObject.transform.eulerAngles = new Vector3(0, 0, 0);
            rotationZ = transform.eulerAngles.z;
        }


        
        //if(rotationZ < -90 || rotationZ > 90)
        //{
            
        //    if(player.transform.eulerAngles.y == 0)
        //    {
        //        transform.localRotation = Quaternion.Euler(180, 0, -rotationZ);
        //    }
        //    else if(player.transform.eulerAngles.y == 180) 
        //    {
        //        transform.localRotation = Quaternion.Euler(180, 180, rotationZ);
        //    }
        //}

    }
}
