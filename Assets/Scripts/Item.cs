using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    string itemTag, wepName;
    WeaponInfo tempWeaponInfo;
    WeaponController tempWeaponController = new WeaponController();

    private void Awake()
    {
        wepName = gameObject.name;

        //if the current room is beaten, this destroys itself on spawn
        if (GameObject.Find("GameManager").GetComponent<GameManager>().currentNode.isRoomBeaten) { Destroy(this.gameObject); }
    }

    private void OnTriggerEnter(Collider other)
    {
        itemTag = gameObject.tag;

        if (other.gameObject.tag == "Player")
        {
            other.gameObject.GetComponent<PlayerInfo>().AddWeapon(wepName);
            Destroy(this.gameObject);
        }
    }


}
