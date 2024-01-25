using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Door : MonoBehaviour
{
    bool roomCleared = false;
    bool leaveRoom;
    [SerializeField] string roomName;
    RoomDatabase roomDatabase;

    private void Awake()
    {
        roomDatabase = new RoomDatabase();
    }

    //public static event EventHandler OnNextRoom;
    private void OnTriggerEnter(Collider other)
    {
        //checks the doubly linked list room database for whether or not this current node has been beaten
        //roomCleared = GameObject.Find("GameManager").GetComponent<GameManager>().currentNode.isRoomBeaten;

        //if the player collides with the door and the room is cleared, the player may freely travel to the next node
        if (other.gameObject.tag == "Player")
        {
            GameObject.Find("GameManager").GetComponent<GameManager>().TravelToThisRoom(roomDatabase.Room_Database[0].sceneName);

        }
    }

    



}
