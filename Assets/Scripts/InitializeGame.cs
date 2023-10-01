using System.Collections;
using System.Collections.Generic;
using UnityEditor.Build.Reporting;
using UnityEngine;

public class InitializeGame : MonoBehaviour
{
    //if the game manager is in the game, instantiates:
        //the game manager
        //the player
        //the main camera
        //the win/lose UI
    private void Awake()
    {
        
        if(GameObject.Find("GameManager") == null) 
        {
            //the game manager
            SetGameObjectName(Instantiate(LoadPrefabFromString("GameManager")), "GameManager");

            //the player
            SetGameObjectName(Instantiate(LoadPrefabFromString("Player")), "Player");

            //the main camera
            SetGameObjectName(Instantiate(LoadPrefabFromString("Main Camera")), "Main Camera");

            //the win/lose UI
            SetGameObjectName(Instantiate(LoadPrefabFromString("Canvas")), "Canvas");

        }
    }

    GameObject LoadPrefabFromString(string prefabName)
    {        
        return (Resources.Load(prefabName) as GameObject);
    }

    void SetGameObjectName(GameObject gameObject, string nameToChangeTo)
    {
        gameObject.name = nameToChangeTo;
    }


}
