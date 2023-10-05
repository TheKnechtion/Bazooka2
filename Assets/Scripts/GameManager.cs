using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState {Playing, Lose, Win }
public class GameManager : MonoBehaviour
{   

    //linked list that holds room data
    RoomDatabase roomDatabase = new RoomDatabase();
    
    //the current room/scene the player is loaded into
    public DoublyNode currentNode;
    private DoublyNode prevNode;

    private EnemySpawnManager enemySpawner;
    bool spawnedEnemies;
    

    //text displayed if the player wins or loses
    GameObject winnerText;
    GameObject loserText;

    GameObject timerText;
    private Timer evacTimer;
    private GameObject exit;
    private Transform ExitPosition;

    //Used to export winning results to the txt file.
    //Other scripts that want to print results
    Exporter txtExporter;

    private bool playerWin, playerLose, exitSpawned;

    public static bool EvacTime = false;

    private GameState state;


    private void Awake()
    {
        //state = GameState.Playing;

        txtExporter = new Exporter();

        enemySpawner = gameObject.GetComponent<EnemySpawnManager>();

        //Door.OnNextRoom += Door_OnNextRoom;

        //creates an exporter usable to the game manager
        //txtExporter = new Exporter();

        SceneManager.activeSceneChanged += SceneManager_changedRoom;

        //create the room database as a linked list
        roomDatabase.CreateLinkedList();

        //sets the initial current node equal to the head node
        currentNode = roomDatabase.headNode;

        //Everything related to the Exit
        exit = Resources.Load("Evac_Exit") as GameObject;
        exit = SpawnExit(exit);
        exit.SetActive(false);
        EvacExit.OnPlayerExit += Exit_OnPlayerExit;

        evacTimer = new Timer(25.0f);

        //prevent the game manager game object from being destroyed between scenes
        DontDestroyOnLoad(this.gameObject);
        DontDestroyOnLoad(exit);
    }



    //This UNITY method detects when the scene changes, which helps out with spawning enemies.
    //There was an issue where I could detect when rooms changed but enemies would spawn within the frame
    //and in the 'beaten' room before I switch to the next room.
    private void SceneManager_changedRoom(Scene arg0, Scene arg1)   
    {
        spawnedEnemies = false;
        SpawnEnemies();
    }

    //private void Door_OnNextRoom(object sender, EventArgs e)
    //{
    //    spawnedEnemies = false;
    //}


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

        exitSpawned = false;
        //prevNode = currentNode;
        //SpawnEnemies(); 
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
        if (currentNode == roomDatabase.headNode)
        {
            SpawnEnemies();
        }
        

        //checks if the this node of the tail node/final room, then checks if the final room has been beaten
        //if(currentNode.nextNode == null && currentNode.isRoomBeaten && !didOnce)
        //{
        //    //activates the player win ui element
        //    PlayerWins();

        //    didOnce = true;
        //}

        CheckAllRoomsCleared();

        if (EvacTime)
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

    private void SpawnEnemies()
    {
        System.Random n = new System.Random();

        
        if (EvacTime)
        {
            Debug.Log("Evac en Spawn");
            enemySpawner.GetSpawnPoints();
            enemySpawner.SpawnEnemies(n.Next(0, 2));
            spawnedEnemies = true;
        }
        else if (!currentNode.isRoomBeaten && !spawnedEnemies)
        {
            Debug.Log("Rgular spawn");
            enemySpawner.GetSpawnPoints();
            //enemySpawner.SpawnEnemies(n.Next(0, 2));
            enemySpawner.SpawnEnemies(0);
            spawnedEnemies = true;
        }
    }

    private void CheckPlayerWin()
    {
        if (evacTimer.timerFinished() && !playerWin)
        {
            EvacTime = false;
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
        if (EvacTime)
        {

            evacTimer.tickTimer(Time.deltaTime);
            //Debug.Log("EVAC TIME: " + evacTimer.TimeLeft);
        }
    }

   
    private void CheckAllRoomsCleared()
    {
        //Check if all nodes are cleared
        if (roomDatabase.roomList.Last().isRoomBeaten)
        {
            Debug.Log("All Cleared");

            //We want to set the the Exit to activate
            if (!exitSpawned)
            {
                Debug.Log("Spawning ecit");
                //SpawnExit(exit);
                exit.SetActive(true);
                exitSpawned = true;
                EvacTime = true;
            }           
        }

    }

    private GameObject SpawnExit(GameObject exit)
    {
        return Instantiate(exit, ExitPosition);
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
