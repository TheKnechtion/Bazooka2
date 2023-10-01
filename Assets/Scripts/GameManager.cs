using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //linked list that holds room data
    RoomDatabase roomDatabase = new RoomDatabase();
    
    //the current room/scene the player is loaded into
    public DoublyNode currentNode;

    //text displayed if the player wins or loses
    GameObject winnerText;
    GameObject loserText;

    //Used to export winning results to the txt file.
    //Other scripts that want to print results
    Exporter txtExporter;

    private void Awake()
    {
        //creates an exporter usable to the game manager
        //txtExporter = new Exporter();

        //create the room database as a linked list
        roomDatabase.CreateLinkedList();

        //sets the initial current node equal to the head node
        currentNode = roomDatabase.headNode;

        //prevent the game manager game object from being destroyed between scenes
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        //finds the winner and loser UI elements in the Canvas GameObject
        winnerText = GameObject.Find("Winner");
        loserText = GameObject.Find("Loser");

        //hides the winner and loser UI elements 
        winnerText.SetActive(false);
        loserText.SetActive(false);
    }

    //used by the Door script to travel to previous rooms
    public void TravelToPreviousRoom()
    {
        //move to the previously linked node
        currentNode = currentNode.previousNode;
    }


    //used by the Door script to travel to next rooms
    public void TravelToNextRoom()
    {
        //move to the next linked node
        currentNode = currentNode.nextNode;
    }


    //Quick way to be sure winner is only printed once
    bool didOnce = false;
    private void Update()
    {
        //checks if the this node of the tail node/final room, then checks if the final room has been beaten
        if(currentNode.nextNode == null && currentNode.isRoomBeaten && !didOnce)
        {
            //activates the player win ui element
            PlayerWins();

            didOnce = true;
        }
    }

    //used to track the player's HP upon beating the game or dying
    int playerHP;

    //activates the player win ui element
    public void PlayerWins()
    {
        playerHP = GameObject.Find("Player").GetComponent<PlayerInfo>().currentHP;

        currentNode.isWinner = true;

        winnerText.SetActive(true);

        //uses the txtExporter attached to this script to output results
        txtExporter.Export("Winner", playerHP, "Results.txt");
    }

    //activates the player lose ui element
    public void PlayerLoses()
    {
        playerHP = 0;

        //uses the txtExporter attached to this script to output results
        txtExporter.Export("Loser", playerHP, "Results.txt");

        loserText.SetActive(true);

    }

}
