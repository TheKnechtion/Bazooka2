using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Door : MonoBehaviour
{
    bool roomCleared = false;
    bool leaveRoom;
    //public static event EventHandler OnNextRoom;
    private void OnTriggerEnter(Collider other)
    {
        //checks the doubly linked list room database for whether or not this current node has been beaten
        roomCleared = GameObject.Find("GameManager").GetComponent<GameManager>().currentNode.isRoomBeaten;

        //if the player collides with the door and the room is cleared, the player may freely travel to the next node
        if (other.gameObject.tag == "Player")
        {
            //if the door has the "previous" tag, it transitions to the previous node
            //if (this.gameObject.tag == "Previous") { GameObject.Find("GameManager").GetComponent<GameManager>().TravelToPreviousRoom(); }

            //if the door has the "next" tag, it transitions to the next node
            if (this.gameObject.tag == "Next")
            {
                GameObject.Find("GameManager").GetComponent<GameManager>().TravelToNextRoom();

            }

            //transitions to the unity scene that has a name matching the name of the door game object
            //SceneManager.LoadSceneAsync(this.gameObject.name);

            //OnNextRoom?.Invoke(this, EventArgs.Empty);
        }
    }
}
