using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationTankBoss : Navigation
{
    public override void MoveToPlayer(bool isAggroed, bool stopAtDistance)
    {
        distance = Vector3.Distance(playerPos, thisPos);
        if (isAggroed == true)
        {
            //StartCoroutine(spaceOut());
            agent.SetDestination(playerPos);
        }
    }

    public void stopMovement()
    {
        agent.isStopped = true;
        //Debug.Log("Stopped movement");
    }
    public void resumeMovement() 
    { 
        agent.isStopped = false;
        //Debug.Log("Resume movement");
    }
}
