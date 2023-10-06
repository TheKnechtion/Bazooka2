using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    private NavMeshAgent agent;

    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        //Always setting destination to the players position
        
    }

    public void MoveToPlayer(bool n)
    {
        if (n == true)
        {
            agent.destination = PlayerInfo.instance.playerPosition;
        }
    }
}
