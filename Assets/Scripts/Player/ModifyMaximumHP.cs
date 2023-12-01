using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModifyMaximumHP : MonoBehaviour
{
    public int amount;
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            switch(tag)
            {
                case "TakeHP":
                    other.gameObject.GetComponent<PlayerInfo>().Take_HP(amount);
                    break;

                case "GiveHP":
                    other.gameObject.GetComponent<PlayerInfo>().Give_HP(amount);
                    break;
            }
        }
    }
}
