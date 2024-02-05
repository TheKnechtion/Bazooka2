using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SneakingNavigation : MonoBehaviour
{
    private NavMeshAgent agent;
    [SerializeField] private float searchRange;

    private NavMeshHit hit;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    // Start is called before the first frame update
    void Start()
    {

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

    public Vector3 GetRandomNavPoint(Vector3 next)
    {
        //TODO: Implement search
        next += Random.onUnitSphere * searchRange;
        next.y = gameObject.transform.position.y;

        if (NavMesh.SamplePosition(next, out hit, 1, NavMesh.AllAreas))
        {
            next = hit.position;
        }

        return next;
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
