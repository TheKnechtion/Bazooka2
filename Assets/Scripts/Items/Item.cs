using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public int heal = 0;

    string itemTag, itemName;
    WeaponInfo tempWeaponInfo;
    WeaponController tempWeaponController = new WeaponController();

    public static event EventHandler OnWeaponPickUp;

    private void Awake()
    {
        itemName = gameObject.name;

        //if the current room is beaten, this destroys itself on spawn
        //if (GameObject.Find("GameManager").GetComponent<GameManager>().currentNode.isRoomBeaten) { Destroy(this.gameObject); }
    }


    /*
    private void FixedUpdate()
    {
        gameObject.GetComponent<Rigidbody>().rotation = new Quaternion(0, 1, 0, 0);
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        itemTag = gameObject.tag;

        if (other.gameObject.tag == "Player" && itemTag == "Weapon")
        {
            other.gameObject.GetComponent<PlayerInfo>().AddWeapon(itemName);
            OnWeaponPickUp?.Invoke(this, EventArgs.Empty);
            Destroy(this.gameObject);
        }

        if(other.gameObject.tag == "Player" && itemTag == "Health")
        {
            other.gameObject.GetComponent<PlayerInfo>().Heal_HP(heal);
            Destroy(this.gameObject);
        }

    }






}
