using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public enum TurretState {SEARCHING, ENGAGED, LOSTSIGHT }
public class BehaviorTurret : EnemyBehavior
{
    [SerializeField] private TurretState turretState;
    [SerializeField] private float DotProduct;

    //Used for going BACK to searching when player leaves vision
    [SerializeField] private float timeBeforeDeAggro;
    private float timeToDeagro;

    private bool playerWasSpotted;
    private bool inRange;
    bool wasInRange;

    private Quaternion initRotation;
    // Start is called before the first frame update
    void Start()
    {
        //A reference, so we can reset later
        timeToDeagro = timeBeforeDeAggro;
        wasInRange = false;
        initRotation = transform.rotation;

        turretState = TurretState.SEARCHING;
    }

    protected override void Update()
    {
        playerWasSpotted = false;
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

        //float time = Time.deltaTime;

        HandleEnemyAggro();

        switch (turretState)
        {
            case TurretState.SEARCHING:
                Debug.Log("Looking for you...");
                break;
            case TurretState.ENGAGED:
                Debug.Log("I SEE YOU");
                if (!isAggrod)
                {
                    turretState = TurretState.LOSTSIGHT;
                }
                else 
                {
                    HandleShooting();
                }
                break;
            case TurretState.LOSTSIGHT:
                Debug.Log("Whered you go...");
                break;
            default:
                break;
        }


        #region Debug Logs
        //Debug.Log("inRange "+inRange);
        //Debug.Log("Spotted: "+ playerWasSpotted);
        //Debug.Log("Aggro Time: " + timeToDeagro);
        //Debug.Log(playerWasSpotted);
        #endregion
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();

        if (turretState == TurretState.LOSTSIGHT)
        {
            if (timeToDeagro > 0 )
            {
                timeToDeagro -= Time.deltaTime;
            }
            else
            {
                turretState = TurretState.SEARCHING;
                timeToDeagro = timeBeforeDeAggro;
                transform.rotation = initRotation;
            }

            //if (timeBeforeDeAggro <= 0)
            //{
            //    turretState = TurretState.SEARCHING;
            //    timeToDeagro = timeBeforeDeAggro;
            //    transform.rotation = initRotation;
            //}
        }
    }

    protected override void HandleEnemyAggro()
    {
        //Determines aggro of the enemy
        inRange = Physics.CheckSphere(gameObject.transform.position, enemyAttackRange_BecomeAggro, playerMask);
        inShootRange = Physics.CheckSphere(gameObject.transform.position, enemyAttackRange_AttackRange, playerMask);

        //If player is within detection range
        if (inRange)
        {
            Ray wallDetect = new Ray(wallDetectPosition, enemyLookDirection);
            RaycastHit hit;
            DotProduct = Vector3.Dot(transform.forward, enemyLookDirection);
            wasInRange = true;

            //If the player isn't blocked by a wall
            if (!Physics.Raycast(wallDetect, out hit, distanceToPlayer, environmentMask))
            {
                if (PlayerSpotted())
                {
                    turretState = TurretState.ENGAGED;
                    transform.LookAt(playerPosition);
                    timeBeforeDeAggro = 0.0f;
                    isAggrod = true;                   
                }

                //if (DotProduct > 0.7f)
                //{
                //    isAggrod = true;
                //    playerWasSpotted = true;
                //}
            }
        }
    }

    private bool PlayerSpotted()
    {
        DotProduct = Vector3.Dot(transform.forward, enemyLookDirection);

        if (DotProduct > 0.7f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
