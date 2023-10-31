using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    NavMeshAgent agent;
    Vector3 playerPos, thisPos, targetPos, reverseDirection;
    public float distance;

    public float stoppingDistance;
    private const float stopCheckradius = 1.5f;

    public bool Stopped;

    //This is used to determine how far to spread out between other enemies
    private float spaceDistance;

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

    public void MoveToPlayer(bool isAggroed, bool stopAtDistance)
    {
        agent.isStopped = false;
        
        distance = Vector3.Distance(playerPos, thisPos);
        if (isAggroed == true)
        {
            //StartCoroutine(spaceOut());
            agent.isStopped = false;
            if (stopAtDistance)
            {

                //agent.stoppingDistance = stoppingDistance;
                agent.destination = playerPos;
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
                agent.destination = playerPos;
            }
        }
        else 
        {
            //agent.SetDestination(transform.position);
        }

       
    }

    private IEnumerator backUP()
    {
        reverseDirection = (thisPos - playerPos);
        targetPos = reverseDirection.normalized * 10;
        agent.destination = targetPos;

        Debug.DrawRay(thisPos, reverseDirection.normalized * 10, Color.red);

        yield return new WaitForSeconds(2f);
        yield return null;
    }

    private bool isInRange(float pointA, float pointB)
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
