using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SneakingNavigation : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private float searchRange;
    [Tooltip("The amount of error from max search range.")]
    [Min(0)]
    [SerializeField] private float searchBuffer;
    private float distanceToNext;

    private NavMeshHit hit;
    private Vector3 directionToNext;

    private int FleeingType;
    private Vector3 PlatormSpot;

    private bool TurnedToNewDest;

    //Debugging
    Vector3 testINit;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }
    void Start()
    {
        directionToNext = Vector3.zero;
        TurnedToNewDest = false;


        //To make sure that it will detect navmesh
        if (NavMesh.SamplePosition(gameObject.transform.position, out hit, 2, NavMesh.AllAreas))
        {
            transform.position = hit.position;
            PlatormSpot = hit.position;
        }
    }

    void Update()
    {
        //Debug.DrawRay(testINit, directionToNext.normalized * searchRange, Color.red);
    }

    public void MoveTo(Vector3 t)
    {
        agent.SetDestination(t);
    }

    public void SetSpeed(float t)
    {
        agent.speed = t;
    }

    public void StopMovement()
    {
        //agent.SetDestination(gameObject.transform.position);

        if (agent.isActiveAndEnabled && agent != null)
        {
            agent.isStopped = false;
        }
    }

    public void ResumeMovement()
    {
        if (agent.isActiveAndEnabled && agent != null)
        {
            agent.isStopped = false;
        }
    }

    public void CancelCurrentDestination()
    {
        agent.SetDestination(gameObject.transform.position);
    }
    public bool GetValidRandomPoint(Vector3 initialPos, out Vector3 nextPos)
    {
        //testINit = initialPos;
        Vector3 randDir;
        Vector3 next;
        Vector3 finalPos;

        TurnedToNewDest = false;

        FleeingType = Random.Range(0, 2);

        if (FleeingType == 1)
        {
            randDir = Random.insideUnitSphere;
            next = initialPos + (randDir * (searchRange - Random.Range(0, searchBuffer)));
            next.y = 0;

            directionToNext = next - initialPos;
            directionToNext.y = 0;

            finalPos = initialPos + directionToNext.normalized * searchRange;

            if (NavMesh.SamplePosition(finalPos, out hit, 1, NavMesh.AllAreas))
            {
                float dot = Vector3.Dot(transform.forward, directionToNext.normalized);
                if (dot <= 0.0f)
                {
                    nextPos = hit.position;
                    return true;
                }
            }
        }
        else
        {
            nextPos = PlatormSpot;
            return true;
        }

        nextPos = initialPos;
        return false;
    }

    

    public float GetDistance()
    {
        return agent.remainingDistance;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gameObject.transform.position, 1 * searchRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(gameObject.transform.position, 1 * searchRange - searchBuffer);
    }
}
