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


    [SerializeField] float gravityMagnitude;

    //text displayed if the player wins or loses
    GameObject winnerText;
    GameObject loserText;
    GameObject timerText;


    //Used to export winning results to the txt file.
    //Other scripts that want to print results
    Exporter txtExporter;

    private bool playerWin, playerLose, exitSpawned;

    //Events for UI
    public static event EventHandler OnLevelCompleted;
    public static event EventHandler OnPlayerWin;
    public static event EventHandler OnPlayerLose;
    public static event EventHandler OnEvacStart;
    public static event EventHandler OnEvacStop;

    public static event EventHandler<bool> OnNeedLevelSelect;

    public static event EventHandler OnSceneChange;

    private EnemySpawnManager enemySpawner;
    bool spawnedEnemies, evacSpawnedEnemies;

    
    public static Timer evacTimer;
    private GameObject exit;
    private Transform ExitPosition;

    bool canSpawn = false;
    bool evacStarted = false;

    public static bool EvacTime = false;

    private GameState state;

    private void Awake()
    {
        Physics.gravity = Physics.gravity * gravityMagnitude;

        //state = GameState.Playing;

        txtExporter = new Exporter();

        enemySpawner = gameObject.GetComponent<EnemySpawnManager>();

        //creates an exporter usable to the game manager
        //txtExporter = new Exporter();

        //create the room database as a linked list
        //roomDatabase.CreateLinkedList();

        //sets the initial current node equal to the head node
        //currentNode = roomDatabase.headNode;

        //Everything related to the Exit
            //exit = Resources.Load("Evac_Exit") as GameObject;
            //exit = SpawnExit(exit);
            //exit.SetActive(false);
        EvacExit.OnPlayerExit += Exit_OnPlayerExit;

        evacTimer = new Timer(40.0f);

        //prevent the game manager game object from being destroyed between scenes
        DontDestroyOnLoad(this.gameObject);
        //DontDestroyOnLoad(exit);





        SceneManager.activeSceneChanged += SceneChanged;
        //SceneManager.sceneLoaded += SceneManager_roomLoaded;
    }

    private void SceneChanged(Scene arg0, Scene arg1)
    {
        UI_Manager.StopShow_RoomSelect();
    }



    //This UNITY method detects when the scene changes, which helps out with spawning enemies.
    //There was an issue where I could detect when rooms changed but enemies would spawn within the frame
    //and in the 'beaten' room before I switch to the next room.
    /*
    private void SceneManager_changedRoom(Scene arg0, Scene arg1)   
    {

    }


    void SceneManager_roomLoaded(Scene scene, LoadSceneMode mode)
    {
        currentScene = SceneManager.GetSceneAt(currentRoom);
        SceneManager.SetActiveScene(currentScene);
        OnSceneChange?.Invoke(this, EventArgs.Empty);
    }
    */

    //Event for when player enters the Exit
    private void Exit_OnPlayerExit(object sender, EventArgs e)
    {
        Debug.Log("PLayer should be winning");
        playerWin = true;
        //PlayerWins();
    }

    private void Start()
    {
        canSpawn = true;

        //finds the winner and loser UI elements in the Canvas GameObject
        winnerText = GameObject.Find("Winner");
        loserText = GameObject.Find("Loser");

        exitSpawned = false;

        BehaviorTankBoss.OnTankKilled += BossKilled;
            //BehaviorTankBoss.OnTankKilled += OnNeedSceneSwitch;

        TransitionObject.OnEndReached += OnNeedSceneSwitch;


        //OnSceneChange?.Invoke(this, EventArgs.Empty);
    }

    private void OnNeedSceneSwitch(object sender, bool e)
    {
        if (!e)
        {
            OnNeedLevelSelect?.Invoke(this, e);
        }
        else
        {
            int NextSceneCount = LevelManager.GetHeldSceneCount();

            if (NextSceneCount > 1) //There is a the player must choose
            {
                OnNeedLevelSelect?.Invoke(this, e);
            }
            else //There is no path, only 1 'next' level
            {
                //This feels be a 'lil' icky, directly switching from here.
                //Might move it to LevelChooser soon.
                LevelManager.MoveToNextScene();
            }
        }        
    }

    /// <summary>
    /// 
    /// We kill bosses at the end of levels, so this is happens at the end 
    /// of any level.
    /// 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void BossKilled(object sender, EventArgs e)
    {
        //Activate the Helicopter Evac (maybe Door transition)
        

        //LevelChooser mouse and stuff
        OnLevelCompleted.Invoke(this, EventArgs.Empty);
    }


    //used by the Door script to travel to previous rooms
    public void TravelToPreviousRoom()
    {
        //move to the previously linked node
        currentNode = currentNode.previousNode;
    }

    int currentRoom = 1;

    //used by the Door script to travel to next rooms
    public void TravelToNextRoom()
    {
        //move to the next linked node

        OnSceneChange?.Invoke(this, EventArgs.Empty);
        currentRoom++;
        SceneManager.LoadScene(currentRoom, LoadSceneMode.Single);
    }


    //used by the Door script to travel to next rooms
    public void TravelToThisRoom(string roomName)
    {
        //move to the next linked node

        OnSceneChange?.Invoke(this, EventArgs.Empty);
        SceneManager.LoadScene(roomName, LoadSceneMode.Single);
    }
    private void Update()
    {
        #region Unused
        //checks if the this node of the tail node/final room, then checks if the final room has been beaten
        //if(currentNode.nextNode == null && currentNode.isRoomBeaten && !didOnce)
        //{
        //    //activates the player win ui element
        //    PlayerWins();

        //    didOnce = true;
        //}


        //---------------//
        //if (!currentNode.spawnedEnemies)
        //{
        //    SpawnEnemies();
        //}


        //if (!currentNode.spawnedEnemies)
        //{
        //    SpawnEnemies();
        //    currentNode.spawnedEnemies = true;
        //}

        //if (!currentNode.isRoomBeaten && !currentNode.spawnedEnemies)
        //{
        //    SpawnEnemies();
        //    currentNode.spawnedEnemies = true;
        //}

        //if (EvacTime)
        //{
        //    if (!didOnce)             This was for testing the event without clearing all rooms
        //    {
        //        OnEvacStart?.Invoke(this, EventArgs.Empty);
        //        didOnce= true;
        //    }           
        //}
        /*
        if (canSpawn)
        {
            SpawnEnemies();
            canSpawn = false;
        }
        
       CheckAllRoomsCleared();

        if (EvacTime)
        {
            CheckPlayerWin();
        }

        //Debug.Log("Spawned Enemies: "+currentNode.spawnedEnemies);
        //Debug.Log("Spawned EVAcEnemies: " + currentNode.spawnedEnemiesEvac);
        */
        #endregion

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

    /*
    private void CheckPlayerWin()
    {
       // Debug.Log(evacTimer.TimeLeft);

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
    */
    private void FixedUpdate()
    {
        if (EvacTime)
        {

            evacTimer.tickTimer(Time.deltaTime);
            //Debug.Log("EVAC TIME: " + evacTimer.TimeLeft);
        }
    }

   /*
    private void CheckAllRoomsCleared()
    {
        //Check if all nodes are cleared
        if (currentNode.nextNode == null & currentNode.isRoomBeaten)
        {
            //We want to set the the Exit to activate if not already
            if (!exitSpawned)
            {
                //Raise the evacTimer event for UI Timer
                OnEvacStart?.Invoke(this, EventArgs.Empty);
                exit.SetActive(true);
                    //timerText.SetActive(true);
                exitSpawned = true;
                EvacTime = true;
            }           
        }

    }
   */

    //used to track the player's HP upon beating the game or dying
    int playerHP;

    //activates the player win ui element
    public void PlayerWins()
    {
        //Raise the win event for UI
        OnPlayerWin?.Invoke(this, EventArgs.Empty); 

        playerHP = GameObject.Find("Player").GetComponent<PlayerInfo>().currentHP;

        currentNode.isWinner = true;

            //winnerText.SetActive(true);

        //uses the txtExporter attached to this script to output results
        txtExporter.Export("Winner", playerHP, "Results.txt");
    }

    //activates the player lose ui element
    public void PlayerLoses()
    {
        //Raise the Lose event for UI
        OnPlayerLose?.Invoke(this, EventArgs.Empty);

        playerHP = 0;

        //uses the txtExporter attached to this script to output results
        txtExporter.Export("Loser", playerHP, "Results.txt");

            //loserText.SetActive(true);
    }

    

}
