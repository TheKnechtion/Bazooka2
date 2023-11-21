using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class BehaviorTurret : EnemyBehavior
{
    [SerializeField] private float DotProduct;

    private Quaternion initRotation;
    // Start is called before the first frame update
    void Start()
    {
        initRotation = transform.rotation;
    }

    protected override void Update()
    {
        inShootRange = false;
        isAggrod = false;

        wallDetectPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z);
        distanceToPlayer = Vector3.Distance(playerPosition, enemyPosition);

        //track the enemy position
        enemyPosition = this.transform.position;

        //track the player position
        playerPosition = PlayerInfo.instance.playerPosition;

        //used by the enemy aggro system to see how far the player is from the enemy
        enemyPlayerTracker = Vector3.Distance(playerPosition, enemyPosition);

        //creates an enemy look direction based on the enemy position and the player's current position
        enemyLookDirection = (playerPosition - enemyPosition).normalized;

        HandleEnemyAggro();
    }

    protected override void HandleEnemyAggro()
    {
        //Determines aggro of the enemy
        isAggrod = Physics.CheckSphere(gameObject.transform.position, enemyAttackRange_BecomeAggro, playerMask);
        inShootRange = Physics.CheckSphere(gameObject.transform.position, enemyAttackRange_AttackRange, playerMask);

        //If player is within detection range
        if (isAggrod)
        {
            Ray wallDetect = new Ray(wallDetectPosition, enemyLookDirection);
            RaycastHit hit;

            //If the player isn't blocked by a wall
            if (!Physics.Raycast(wallDetect, out hit, distanceToPlayer, environmentMask))
            {
                DotProduct = Vector3.Dot(transform.forward, enemyLookDirection);
                //Debug.Log(DotProduct);

                if (DotProduct > 0.7f)
                {
                    Debug.Log("I see you");
                    transform.LookAt(playerPosition);
                }
                else
                {
                    transform.rotation = initRotation;
                    Debug.Log("Out of sight");
                }
            }
        }
    }
}
