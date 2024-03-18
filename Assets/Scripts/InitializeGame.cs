using System.Collections;
using System.Collections.Generic;
//using UnityEditor.Build.Reporting;
using UnityEngine;

public class InitializeGame : MonoBehaviour
{
    public Quaternion playerSpawnLookDirection;
    public Vector3 playerSpawnLocation;

    //THIS scene's next possible levels
    // (Always set upon entering a new scene)
    public SceneScriptable NextScenes;

    public List<GameObject> dontDestroyOnLoadGameObjects;

    //if the game manager is in the game, instantiates:
        //the game manager
        //the player
        //the main camera
        //the UI Canvas

    private void Awake()
    {
        
        if(GameObject.Find("GameManager") == null) 
        {
            //the game manager
            dontDestroyOnLoadGameObjects.Add(SetGameObjectName(Instantiate(LoadPrefabFromString("GameManager")), "GameManager"));

            //the player
            dontDestroyOnLoadGameObjects.Add(SetGameObjectName(Instantiate(LoadPrefabFromString("Player"), playerSpawnLocation, playerSpawnLookDirection), "Player"));
            
            //the main camera
            SetGameObjectName(Instantiate(LoadPrefabFromString("Main Camera")), "Main Camera");
            dontDestroyOnLoadGameObjects.Add(SetGameObjectName(Instantiate(LoadPrefabFromString("Cinemachine Camera")), "Follow Camera"));

            //UI
            dontDestroyOnLoadGameObjects.Add(SetGameObjectName(Instantiate(LoadPrefabFromString("Canvas")), "Canvas"));

            //Decal Pool
            dontDestroyOnLoadGameObjects.Add(SetGameObjectName(Instantiate(LoadPrefabFromString("DecalPool")), "DecalPool"));
        }

        if (NextScenes != null)
        {
            LevelManager.AddNextScenes(NextScenes);
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

}
