using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class freezePlayerY : MonoBehaviour
{
    [Tooltip("Turn on or off freezing the player's y-position when they first make contact with this object")]
    [SerializeField] bool freezePlayersHeight;

    [Tooltip("Turn on or off setting the player's y-position when they first make contact with this object")] 
    [SerializeField] bool setPlayersHeight;

    [Tooltip("This is the height to set player's y-position to when they first make contact with this object.")]
    [SerializeField] float heightToSetPlayer;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag != "Player")
        {
            return;
        }

        if (freezePlayersHeight)
        {
            other.transform.GetComponent<Rigidbody>().constraints |= RigidbodyConstraints.FreezePositionY;
        }

        if (setPlayersHeight)
        {
            other.transform.position = new Vector3(other.transform.position.x, heightToSetPlayer, other.transform.position.z);
        }
    }
}
