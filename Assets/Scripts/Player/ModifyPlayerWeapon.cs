using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyPlayerWeapon : MonoBehaviour
{
    public int amount;


    private void OnTriggerEnter(Collider other)
    {
        /*
        if (other.gameObject.tag == "Player")
        {
            switch (tag)
            {
                case "TakeMaxProj":
                    other.gameObject.GetComponent<PlayerInfo>().Decrease_Max_Proj(amount);
                    break;

                case "GiveMaxProj":
                    other.gameObject.GetComponent<PlayerInfo>().Give_HP(amount);
                    break;


            }
        }
        */
    }



}
