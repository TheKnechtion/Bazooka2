using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build.Reporting;
using UnityEngine;

public class InitializeGame : MonoBehaviour
{
    public Quaternion playerSpawnLookDirection;
    public Vector3 playerSpawnLocation;


    public List<GameObject> dontDestroyOnLoadGameObjects;

    //if the game manager is in the game, instantiates:
        //the game manager
        //the player
        //the main camera
        //the win/lose UI

    GameObject reference;

    private void Awake()
    {
        
        if(GameObject.Find("GameManager") == null) 
        {
            //the game manager
            dontDestroyOnLoadGameObjects.Add(SetGameObjectName(Instantiate(LoadPrefabFromString("GameManager")), "GameManager"));



            //the player
            dontDestroyOnLoadGameObjects.Add(SetGameObjectName(Instantiate(LoadPrefabFromString("Player"), playerSpawnLocation, playerSpawnLookDirection), "Player"));
            //SetGameObjectName(Instantiate(LoadPrefabFromString("Player")), "Player");

            //the main camera
            SetGameObjectName(Instantiate(LoadPrefabFromString("Main Camera")), "Main Camera");
            dontDestroyOnLoadGameObjects.Add(SetGameObjectName(Instantiate(LoadPrefabFromString("Cinemachine Camera")), "Follow Camera"));
            //SetGameObjectName(Instantiate(LoadPrefabFromString("Top View Camera")), "Top View Camera");

            //the win/lose UI
            dontDestroyOnLoadGameObjects.Add(SetGameObjectName(Instantiate(LoadPrefabFromString("Canvas")), "Canvas"));

        }
    }

    

    
    GameObject LoadPrefabFromString(string prefabName)
    {        
        return (Resources.Load(prefabName) as GameObject);
    }

    GameObject SetGameObjectName(GameObject gameObject, string replacementName)
    {
        gameObject.name = replacementName;
        return gameObject;
    }

    void SetPlayerPosition()
    {

    }




}
