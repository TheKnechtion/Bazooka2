using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    //holds the position of an enemy spawn node
    Vector3 spawnPosition;

    EnemySpawnManager enemySpawnManager;

    bool triggered = false;

    private void Start()
    {
        //if this room is beaten, destroy this object
        if (GameObject.Find("GameManager").GetComponent<GameManager>().currentNode.isRoomBeaten) { Destroy(this.gameObject); };

        //finds the position of an enemy spawn node
        spawnPosition = GameObject.Find("EnemySpawnNode").transform.position;

        enemySpawnManager = GameObject.Find("GameManager").GetComponent<EnemySpawnManager>();
    }


    //triggers if the player or their projectile collides with this game object's collider
    private void OnTriggerEnter(Collider other)
    {
        //spawn an unseen enemy from the enemy database
        enemySpawnManager.SpawnEnemy(spawnPosition);

        //destroy the button game object
        Destroy(this.gameObject);
    }






}
