using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sander_Movement : Activatable
{
    [SerializeField] float speed;

    bool didOnce = false;


    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Collide" && !didOnce)
        {
            Deactivate();
            //isActive = false;
            speed = 0;
            didOnce = true;
        }
    }



    void FixedUpdate()
    {
        if(isActive)
        {
            this.transform.position += (this.transform.right * speed);
        }
    }




}
