using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freezePlayerY : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePositionY;
        }
        
    }
}
