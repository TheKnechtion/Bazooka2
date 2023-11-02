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
            agent.isStopped = false;
            agent.SetDestination(playerPos);
        }
    }
}
