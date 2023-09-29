using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    string itemTag;
    [SerializeField] private string wepName;
    WeaponInfo tempWeaponInfo;
    //WeaponController tempWeaponController;

    private void Awake()
    {
        wepName = gameObject.name;
        itemTag= gameObject.tag;

    }

    private void OnTriggerEnter(Collider other)
    {
        itemTag = gameObject.tag;

        if (other.gameObject.tag == "Player")
        {
            
            //tempWeaponInfo = tempWeaponController.MakeWeapon(gameObject.name);

            //on collision, add the weapon to the player's owned weapons
                
            //other.gameObject.GetComponent<PlayerInfo>().currentWeapon = tempWeaponInfo;
            other.gameObject.GetComponent<PlayerInfo>().AddWeapon(wepName);

            Destroy(gameObject);
        }
    }


}
