using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    NavMeshAgent agent;
    Vector3 playerPos, thisPos;
    float distance;




    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Always setting destination to the players position
        thisPos = gameObject.transform.position;
        playerPos = PlayerInfo.instance.playerPosition;
    }

    public void MoveToPlayer(bool n, bool stopAtDistance)
    {
        distance = Vector3.Distance(playerPos, thisPos);
        if (n == true)
        {
            agent.isStopped = false;
            if (stopAtDistance)
            {
                
                agent.stoppingDistance = 5;
                agent.destination = playerPos;
                
            }
            else
            { 
                agent.stoppingDistance = 0;
                agent.destination = playerPos;
            }
        }
        else 
        {
            //agent.SetDestination(transform.position);
        }
    }
}
