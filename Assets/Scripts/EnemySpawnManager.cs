using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

//used by the game manager to control enemy spawn
public class EnemySpawnManager : MonoBehaviour
{
    //the stats of the enemy that will be spawned
    EnemyInfo tempEnemyInfo;

    //the database of all enemy stats
    public List<EnemyInfo> enemyDatabase;

    //this is the spawned enemy
    GameObject tempGameObject;

    //this tracks all enemies the player hasn't seen yet, based on the enemy database
    List<string> listOfUnseenEnemiesInCurrentPlaythough;


    private Transform[] spawnPositions;
    public GameObject spawnObject;

    public static int enemyCount;

    public static event EventHandler OnEnemyDeath;

    private void Awake()
    {
        GameManager.OnSceneChange += SpawnEnemiesOnSceneChange;


        //create an enemy database that will be used by the spawn manager
        enemyDatabase = new EnemyDatabase().enemyDatabase;
    }


    public void Start()
    {
        

        //create the list of enemies the player hasn't seen yet
        listOfUnseenEnemiesInCurrentPlaythough = new List<string>();

        //fill the list of enemies the player hasn't seen yet with info from the enemy database
        foreach(EnemyInfo enemy in enemyDatabase) {listOfUnseenEnemiesInCurrentPlaythough.Add(enemy.name);}
    }

    //used by a button to spawn an enemy the player hasn't seen yet at the location of an enemy spawn node
    public void SpawnEnemy(Vector3 position)
    {
        //stores info for an enemy the player hasn't seen yet
        tempEnemyInfo = enemyDatabase.First(enemy => listOfUnseenEnemiesInCurrentPlaythough.Contains(enemy.name));

        //load the appropriate enemy prefab based on the enemy name
        tempGameObject = LoadResource(tempEnemyInfo.name);

        //instantiate the prefab at the passed in position (an enemy spawn node)
        tempGameObject = Instantiate(tempGameObject, position, new Quaternion(0,0,0,0));

        //sets the name of the game object to the enemy's name in the database
        tempGameObject.name = tempEnemyInfo.name;

        //sets the health of the enemy from the database
        tempGameObject.GetComponent<EnemyBehavior>().health = tempEnemyInfo.HP;

        /*
        //sets the def of the enemy from the database (no use in this game, but it sets it)
        tempGameObject.GetComponent<EnemyBehavior>().DEF = tempEnemyInfo.DEF;

        //sets the MP of the enemy from the database (no use in this game, but it sets it)
        tempGameObject.GetComponent<EnemyBehavior>().MP = tempEnemyInfo.MP;

        //sets the def of the enemy from the database
        //(This is currently only used for the knight. It sets a value in the weapon database for the knight's weapon.)
        tempGameObject.GetComponent<EnemyBehavior>().AP = tempEnemyInfo.AP;
        */

        //removes the spawned enemy from the list of enemies the player hasn't encountered this playthrough
        listOfUnseenEnemiesInCurrentPlaythough.Remove(tempEnemyInfo.name);
    }

    public void SpawnEnemy(Vector3 position, int n)
    {
        //stores info for an enemy the player hasn't seen yet

        tempEnemyInfo = enemyDatabase[n];
        //tempEnemyInfo = enemyDatabase.First(enemy => listOfUnseenEnemiesInCurrentPlaythough.Contains(enemy.name));

        //load the appropriate enemy prefab based on the enemy name
        tempGameObject = LoadResource(tempEnemyInfo.name);

        //instantiate the prefab at the passed in position (an enemy spawn node)
        tempGameObject = Instantiate(tempGameObject, position, new Quaternion(0, 0, 0, 0));

        //sets the name of the game object to the enemy's name in the database
        tempGameObject.name = tempEnemyInfo.name;

        //sets the health of the enemy from the database
        tempGameObject.GetComponent<EnemyBehavior>().health = tempEnemyInfo.HP;

        /*
        //sets the def of the enemy from the database (no use in this game, but it sets it)
        tempGameObject.GetComponent<EnemyBehavior>().DEF = tempEnemyInfo.DEF;

        //sets the MP of the enemy from the database (no use in this game, but it sets it)
        tempGameObject.GetComponent<EnemyBehavior>().MP = tempEnemyInfo.MP;

        //sets the def of the enemy from the database
        //(This is currently only used for the knight. It sets a value in the weapon database for the knight's weapon.)
        tempGameObject.GetComponent<EnemyBehavior>().AP = tempEnemyInfo.AP;
        */

        //removes the spawned enemy from the list of enemies the player hasn't encountered this playthrough
        listOfUnseenEnemiesInCurrentPlaythough.Remove(tempEnemyInfo.name);

    }

    #region Old SpawnEnemyByTag (passes ONLY Vector3, not Transform)
    /*
    public void SpawnEnemyByTag(Vector3 position, string name)
    {
        //stores info for an enemy the player hasn't seen yet

        tempEnemyInfo = enemyDatabase.First(enemy => enemy.name == name);
        //tempEnemyInfo = enemyDatabase.First(enemy => listOfUnseenEnemiesInCurrentPlaythough.Contains(enemy.name));

        //load the appropriate enemy prefab based on the enemy name
        tempGameObject = LoadResource(tempEnemyInfo.name);

        //instantiate the prefab at the passed in position (an enemy spawn node)
        tempGameObject = Instantiate(tempGameObject, new Vector3(position.x, 0, position.z), new Quaternion(0, 0, 0, 0));
        

        //sets the name of the game object to the enemy's name in the database
        tempGameObject.name = tempEnemyInfo.name;

        //sets the health of the enemy from the database
        tempGameObject.GetComponent<EnemyBehavior>().health = tempEnemyInfo.HP;

        //sets the def of the enemy from the database (no use in this game, but it sets it)
        tempGameObject.GetComponent<EnemyBehavior>().DEF = tempEnemyInfo.DEF;

        //sets the MP of the enemy from the database (no use in this game, but it sets it)
        tempGameObject.GetComponent<EnemyBehavior>().MP = tempEnemyInfo.MP;

        //sets the def of the enemy from the database
        //(This is currently only used for the knight. It sets a value in the weapon database for the knight's weapon.)
        tempGameObject.GetComponent<EnemyBehavior>().AP = tempEnemyInfo.AP;
    }
    */
    #endregion

    public void SpawnEnemyByTag(Transform spawnNode, string name)
    {
        //stores info for an enemy the player hasn't seen yet

        //tempEnemyInfo = enemyDatabase.First(enemy => enemy.name == name);
        //tempEnemyInfo = enemyDatabase.First(enemy => listOfUnseenEnemiesInCurrentPlaythough.Contains(enemy.name));

        //load the appropriate enemy prefab based on the enemy name
        tempGameObject = LoadResource(name);

        //instantiate the prefab at the passed in position (an enemy spawn node)
        tempGameObject = Instantiate(tempGameObject, new Vector3(spawnNode.position.x, 0, spawnNode.position.z), spawnNode.rotation);


        #region We sets stats using Scriptable Objects, designers can change easily if needed
        /*
        //sets the name of the game object to the enemy's name in the database
        tempGameObject.name = tempEnemyInfo.name;

        //sets the health of the enemy from the database
        tempGameObject.GetComponent<EnemyBehavior>().health = tempEnemyInfo.HP;

        //sets the def of the enemy from the database (no use in this game, but it sets it)
        tempGameObject.GetComponent<EnemyBehavior>().DEF = tempEnemyInfo.DEF;

        //sets the MP of the enemy from the database (no use in this game, but it sets it)
        tempGameObject.GetComponent<EnemyBehavior>().MP = tempEnemyInfo.MP;

        //sets the def of the enemy from the database
        //(This is currently only used for the knight. It sets a value in the weapon database for the knight's weapon.)
        tempGameObject.GetComponent<EnemyBehavior>().AP = tempEnemyInfo.AP;
        */
        #endregion
    }


    public void UpdateEnemyCount()
    {
        enemyCount--;
        OnEnemyDeath?.Invoke(this, EventArgs.Empty);
        CheckIfAllEnemiesAreBeaten();
    }

    void CheckIfAllEnemiesAreBeaten()
    {

        if (enemyCount == 0 && !gameObject.GetComponent<GameManager>().currentNode.isRoomBeaten)
        {
            gameObject.GetComponent<GameManager>().currentNode.isRoomBeaten = true;
        }
        else if(enemyCount == 0 && !gameObject.GetComponent<GameManager>().currentNode.beatEvacRoom)
        {
            gameObject.GetComponent<GameManager>().currentNode.beatEvacRoom = true;
        }
    }

    public void GetSpawnPoints()
    {
        spawnObject = GameObject.Find("EnemySpawns");
        spawnPositions = spawnObject.GetComponentsInChildren<Transform>();
    }

    public void GetEvacSpawnPoint()
    {
        spawnObject = GameObject.Find("EvacEnemySpawns");
        spawnPositions = spawnObject.GetComponentsInChildren<Transform>();
    }

    public void SpawnEnemies(int n)
    {
        for (int i = 1; i < spawnPositions.Length; i++)
        {
            SpawnEnemy(spawnPositions[i].transform.position, n);
        }
    }

    void SpawnEnemiesOnSceneChange(object sender, EventArgs e)
    {
        GetSpawnPoints();
        SpawnEnemiesByTag();
    }

    public void SpawnEnemies()
    {
        GetSpawnPoints();
        SpawnEnemiesByTag();
    }

    string currentName;
    public void SpawnEnemiesByTag()
    {
        enemyCount = spawnPositions.Length-1;
        for (int i = 1; i < spawnPositions.Length; i++)
        {
            currentName = spawnPositions[i].tag;
            //SpawnEnemyByTag(spawnPositions[i].transform.position, currentName);
            SpawnEnemyByTag(spawnPositions[i].transform, currentName);
        }
        //OnEnemyDeath?.Invoke(this, EventArgs.Empty);
    }

    //used to quickly load an object as a game object
    public GameObject LoadResource(string name)
    {
        //loads an object in the resources folder as a game object
        return Resources.Load(name) as GameObject;
    }


}
