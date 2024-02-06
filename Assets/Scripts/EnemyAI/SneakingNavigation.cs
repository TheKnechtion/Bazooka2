using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;
using UnityEngine.AI;

public class SneakingNavigation : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private float searchRange;
    [Tooltip("The amount of error from max search range.")]
    [SerializeField] private float searchBuffer;
    private float distanceToNext;

    private NavMeshHit hit;
    private Vector3 directionToNext;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.angularSpeed = 360;
    }

    // Start is called before the first frame update
    void Start()
    {
        directionToNext = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {

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
        agent.SetDestination(gameObject.transform.position);
    }
    public bool GetValidRandomPoint(Vector3 initialPos, out Vector3 nextPos)
    {
        /*
        directionNext = Random.insideUnitSphere.normalized;
        directionNext.y = 0;
        Vector3 finalPos = initialPos + (directionNext.normalized * searchRange);

        if (NavMesh.SamplePosition( finalPos, out hit, 1, NavMesh.AllAreas))
        {
            Vector3 dirToNext = finalPos - directionNext;
            float dot = Vector3.Dot(gameObject.transform.forward, dirToNext);
            if (dot <= 0.0f)
            {
                nextPos = hit.position;
                return true;
            }
        }
        */

        Vector3 randDir = Random.insideUnitSphere;
        Vector3 next = initialPos + (randDir * searchRange);
        next.y = 0;

        directionToNext = next - initialPos;

        if (NavMesh.SamplePosition(next, out hit, 1, NavMesh.AllAreas))
        {
            float dot = Vector3.Dot(transform.forward, directionToNext.normalized);
            if (dot <= 0.0f)
            {
                nextPos = hit.position;
                return true;
            }
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
    }
}
