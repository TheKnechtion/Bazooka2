using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freezePlayerY : MonoBehaviour
{
    [SerializeField] float heightToSetPlayer;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            if(other.transform.position.y != heightToSetPlayer)
            {
                other.transform.position = new Vector3(other.transform.position.x, heightToSetPlayer, other.transform.position.z);
            }

            other.transform.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePositionY;
        }
        
    }
}
