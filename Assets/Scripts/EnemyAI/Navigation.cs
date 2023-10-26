using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;

public class Navigation : MonoBehaviour
{
    NavMeshAgent agent;
    Vector3 playerPos, thisPos, targetPos;
    float distance;

    public float stoppingDistance;

    //This is used to determine how far to spread out between other enemies
    private float spaceDistance;
    private RaycastHit[] enemiesNearby = new RaycastHit[7];

    [SerializeField] private LayerMask enemyMask;


    // Start is called before the first frame update
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spaceDistance = 3.0f;
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
            //StartCoroutine(spaceOut());
            agent.isStopped = false;
            if (stopAtDistance)
            {
                
                agent.stoppingDistance = stoppingDistance;
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

    private IEnumerator spaceOut()
    {
        //Ray sphereRay = Physics.SphereCast(gameObject.transform.position, spaceDistance, gameObject.transform.position, );
        int hit = Physics.SphereCastNonAlloc(gameObject.transform.position, spaceDistance, gameObject.transform.position, enemiesNearby);

        //detect is near other enemies,
        if (hit > 0)
        {
            for (int i = 0; i < enemiesNearby.Length; i++)
            { 
                //Confirms that we are detecting enemies
                GameObject hitobject = enemiesNearby[i].collider.GetComponent<GameObject>();
                if(hitobject.TryGetComponent<EnemyInfo>(out EnemyInfo enemy))
                {
                    Debug.Log("Enemy detected");
                    agent.destination = new Vector3(targetPos.x, targetPos.y, targetPos.z +5);
                }
            }
        }
            //if true, move away,
            //else, do notthing
        yield return null;
    }
}
