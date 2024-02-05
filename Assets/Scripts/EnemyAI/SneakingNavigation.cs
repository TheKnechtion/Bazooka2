using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SneakingNavigation : MonoBehaviour
{
    private NavMeshAgent agent;


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

    public Vector3 GetRandomNavPoint()
    {
        //TODO: Implement search
        return Vector3.zero;
    }

    public float GetDistance()
    {
        return agent.remainingDistance;
    }
}
