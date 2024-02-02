using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;
using System;

public class Navigation : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected EnemyBehavior eb;

    protected Vector3 playerPos, thisPos, targetPos, reverseDirection;
    public float distance;

    public float stoppingDistance;
    protected const float stopCheckradius = 1.5f;

    public event EventHandler OnStoppedMoving;

    //This is used to determine how far to spread out between other enemies
    private float spaceDistance;

    // Start is called before the first frame update
    protected virtual void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        eb = GetComponent<EnemyBehavior>();
        eb.OnDeath += OnDeath;
    }

    private void OnDeath(object sender, EventArgs e)
    {
        agent.SetDestination(gameObject.transform.position);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //Always setting destination to the players position
        thisPos = gameObject.transform.position;
        playerPos = PlayerInfo.instance.playerPosition;

        if (checkIfMoving())
        {
            OnStoppedMoving?.Invoke(this, EventArgs.Empty);
        }
    }

    public virtual void MoveToPlayer(bool isAggroed, bool stopAtDistance)
    {
        agent.isStopped = false;
        
        distance = Vector3.Distance(playerPos, thisPos);
        if (isAggroed == true)
        {
            agent.isStopped = false;
            if (stopAtDistance)
            {

                //agent.stoppingDistance = stoppingDistance;
                agent.SetDestination(playerPos);
                if (isInRange(distance, stoppingDistance) ||
                    (distance  < stoppingDistance))
                {
                    agent.SetDestination(transform.position);
                }

                if (distance < stoppingDistance)
                    StartCoroutine(backUP());

            }
            else
            { 
                agent.stoppingDistance = 0;
                agent.SetDestination(playerPos);
            }
        }
        else 
        {
            //agent.SetDestination(transform.position);
        }       
    }

    protected bool checkIfMoving()
    {
        if (agent.velocity == Vector3.zero)
        {
            return true;
        }
        return false;
    }

    public void StopMovement()
    {
        agent.isStopped = true;
    }

    public void ResumeMovement()
    {
        agent.isStopped = false; 
    }


    protected IEnumerator backUP()
    {
        reverseDirection = (thisPos - playerPos);
        targetPos = reverseDirection.normalized * 10;
        agent.destination = targetPos;

        Debug.DrawRay(thisPos, reverseDirection.normalized * 10, Color.red);

        yield return new WaitForSeconds(2f);
        yield return null;
    }

    protected bool isInRange(float pointA, float pointB)
    {
        if ((pointA <= pointB + stopCheckradius) && (pointA >= pointB - stopCheckradius))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
