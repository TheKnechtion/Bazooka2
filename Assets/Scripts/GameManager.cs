using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public enum GameState {Playing, Lose, Win }
public class GameManager : MonoBehaviour
{
    //linked list that holds room data
    RoomDatabase roomDatabase = new RoomDatabase();
    
    //the current room/scene the player is loaded into
    public DoublyNode currentNode;

    //text displayed if the player wins or loses
    GameObject winnerText;
    GameObject loserText;

    GameObject timerText;
    private Timer evacTimer;
    private EvacExit Exit;
    private GameObject exit;
    private Transform ExitPosition;

    //Used to export winning results to the txt file.
    //Other scripts that want to print results
    Exporter txtExporter;

    private bool playerWin, playerLose, evacTime;
    private GameState state;


    private void Awake()
    {
        //state = GameState.Playing;

        txtExporter = new Exporter();

        //creates an exporter usable to the game manager
        //txtExporter = new Exporter();

        //create the room database as a linked list
        roomDatabase.CreateLinkedList();

        //sets the initial current node equal to the head node
        currentNode = roomDatabase.headNode;

        //Everything related to the Exit
        exit = Resources.Load("Evac_Exit") as GameObject;
        EvacExit.OnPlayerExit += Exit_OnPlayerExit;

        evacTimer = new Timer(3.0f);

        //prevent the game manager game object from being destroyed between scenes
        DontDestroyOnLoad(this.gameObject);
    }


    //Event for when player enters the Exit
    private void Exit_OnPlayerExit(object sender, EventArgs e)
    {
        Debug.Log("PLayer should be winning");
        playerWin = true;
        //PlayerWins();
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
        //if(currentNode.nextNode == null && currentNode.isRoomBeaten && !didOnce)
        //{
        //    //activates the player win ui element
        //    PlayerWins();

        //    didOnce = true;
        //}

        CheckAllRoomsCleared();

        if (evacTime)
        {
            CheckPlayerWin();
        }

        #region Game State machine
        //I imagine we use this later in development
        //switch (state)
        //{
        //    case GameState.Playing:
        //        //code for Playing state
        //        break;
        //    case GameState.Lose:
        //        //code for when player Dies
        //        break;
        //    case GameState.Win:
        //        //code for when player Wins
        //        break;
        //    default:
        //        break;
        //}
        #endregion
    }

    private void CheckPlayerWin()
    {
        if (evacTimer.timerFinished() && !playerWin)
        {
            evacTime = false;
            evacTimer.Zero();
            PlayerLoses();
        }

        if (evacTimer.TimeLeft>0 && playerWin)
        {
            PlayerWins();
        }
    }

    private void FixedUpdate()
    {
        if (evacTime)
        {

            evacTimer.tickTimer(Time.deltaTime);
            Debug.Log("EVAC TIME: " + evacTimer.TimeLeft);
        }
    }
    private void CheckAllRoomsCleared()
    {
        //Check if all nodes are cleared
        if (roomDatabase.roomList.Last().isRoomBeaten)
        {
            //We want to set the the Exit to activate
            //Exit.gameObject.SetActive(true);

            SpawnExit(exit);
            evacTime = true;
        }

    }

    private void SpawnExit(GameObject exit)
    {
        exit = Instantiate(exit, ExitPosition);
        if (exit)
        {
            Debug.Log("Exit has spawned");
        }
        else
        {
            Debug.Log("There is no exit Spawned");
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
