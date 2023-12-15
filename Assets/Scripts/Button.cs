using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour, IActivate
{
    [SerializeField] GameObject activatedObject;

    public bool activated = false;
    

    Material activatedMat;

    MeshRenderer buttonRenderer;



    //New addition (for camera)
    [SerializeField] private CinemachineVirtualCamera connectedCamera;
    private CameraSwitcher camManager;
    [SerializeField] private bool usesCamera;

    public UnityEvent doThis;

    private void Awake()
    {
        activatedMat = (Material)Resources.Load("Activated");
        buttonRenderer = gameObject.GetComponent<MeshRenderer>();

        
        //New addition (for camera)
            //Can be still be used without a camera         
        if (connectedCamera)
        {
            camManager = GameObject.Find("CameraManager").GetComponent<CameraSwitcher>();
            usesCamera = true;
        }
        else { usesCamera= false; }
    }


    public void Activate()
    {
        if(!activated)
        {
            buttonRenderer.material = activatedMat;
            activatedObject.GetComponent<Act>().Activate();
            activated = true;

            doThis.Invoke();


            if (usesCamera)
             camManager.SwitchToCamera(connectedCamera);
        }
    }


    #region Older spawn code
    /*
    //holds the position of an enemy spawn node
    Vector3 spawnPosition;

    EnemySpawnManager enemySpawnManager;
    private Transform[] spawnPositions;
    [SerializeField] private GameObject spawnObject;

    private int enemyType;

    private System.Random rng;

    bool triggered = false;
    private void Start()
    {
        //if this room is beaten, destroy this object
        if (GameObject.Find("GameManager").GetComponent<GameManager>().currentNode.isRoomBeaten) { Destroy(this.gameObject); };

        //finds the position of an enemy spawn node
        //spawnPosition = GameObject.Find("EnemySpawnNode").transform.position;
        spawnObject = GameObject.Find("EnemySpawns");
        GetSpawnPoints();

        enemySpawnManager = GameObject.Find("GameManager").GetComponent<EnemySpawnManager>();

        triggered = false;

        rng = new System.Random();
        enemyType = rng.Next(0, 2);
    }


    //triggers if the player or their projectile collides with this game object's collider
    private void OnTriggerEnter(Collider other)
    {
        //spawn an unseen enemy from the enemy database
        //enemySpawnManager.SpawnEnemy(spawnPosition);
        if (!triggered)
        {
            triggered = true;
            SpawnEnemies(enemyType);
        }


        //destroy the button game object
        Destroy(this.gameObject);
    }

    private void GetSpawnPoints()
    {
        spawnPositions = spawnObject.GetComponentsInChildren<Transform>();      
    }

    private void SpawnEnemies(int n)
    {
        for (int i = 1; i < spawnPositions.Length; i++)
        {
            enemySpawnManager.SpawnEnemy(spawnPositions[i].transform.position, n);
        }
    }
    */
    #endregion
}
