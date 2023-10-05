using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestroyableObject : MonoBehaviour, IDamagable
{

    public int health = 4;

    float i = 0;
    bool isRoomBeaten = false;

    Vector3 position;
    private void Awake()
    {
        //checks if the game manager exists in the scene
        if (GameObject.Find("GameManager") != null)
        {
            //if it exists, check if the current room has been beaten
            isRoomBeaten = GameObject.Find("GameManager").GetComponent<GameManager>().currentNode.isRoomBeaten;
            
            //if it has been beaten already, destroy this game object
            if (isRoomBeaten) { Destroy(this.gameObject); }
        }
    }


    private void OnCollisionEnter(Collision collision)
    {
        position = this.gameObject.transform.position;

        //GameObject.Find("GameManager").GetComponent<EnemySpawnManager>().SpawnEnemy(new Vector3(2 - i + position.x, 1, i + position.z));
        i++;

    }

    public void TakeDamage(int passedDamage)
    {
        health -= passedDamage;

        if (health <= 0)
        {
           Die();
        }
    }

    public void Die()
    {
        Destroy(this.gameObject);
    }
}
