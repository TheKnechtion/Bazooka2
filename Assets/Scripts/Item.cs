using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    string itemTag;
    WeaponInfo tempWeaponInfo;
    WeaponController tempWeaponController = new WeaponController();

    private void Awake()
    {
        //if the current room is beaten, this destroys itself on spawn
        if (GameObject.Find("GameManager").GetComponent<GameManager>().currentNode.isRoomBeaten) { Destroy(this.gameObject); };


    }

    private void OnTriggerEnter(Collider other)
    {
        itemTag = gameObject.tag;

        if (other.gameObject.tag == "Player")
        {
            
            tempWeaponInfo = tempWeaponController.MakeWeapon(gameObject.name);

            //on collision, add the weapon to the player's owned weapons
            other.gameObject.GetComponent<PlayerInfo>().currentWeapon = tempWeaponInfo;

            Destroy(gameObject);
        }
    }


}
