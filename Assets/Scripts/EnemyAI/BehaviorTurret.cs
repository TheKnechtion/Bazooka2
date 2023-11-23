using System.Collections;
using System.Collections.Generic;
using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public enum TurretState {SEARCHING, ENGAGED, LOSTSIGHT }
public class BehaviorTurret : EnemyBehavior
{
    [SerializeField] private TurretState turretState;
    private float DotProduct;
    [SerializeField] private float AimingTime;

    [SerializeField] private float TurningAngle;
    private Vector3 leftTurn;
    private Vector3 rightTurn;
    private Vector3 currentRotation;

    //Used for going BACK to searching when player leaves vision
    [SerializeField] private float timeBeforeDeAggro;
    private float timeToDeagro;

    private bool inRange;
    private bool isTurning;
    private bool swapDirection;

    private Quaternion initRotation;

    private float t;
    private float timeToTurn;

    // Start is called before the first frame update
    void Start()
    {
        t = 0;
        timeToTurn = 1.5f; 
        isTurning = false;
        swapDirection = false;

        //A reference, so we can reset later
        timeToDeagro = timeBeforeDeAggro;

        initRotation = transform.rotation;
        leftTurn = new Vector3(0, initRotation.y-TurningAngle,0);
        rightTurn = new Vector3(0, initRotation.y+TurningAngle,0);
        //rightTurn = Quaternion.Euler(0, initRotation.y + TurningAngle, 0);

        //Pass the weapon script that attacthed to the object
        weaponController = gameObject.GetComponent<WeaponController>();

        //set the enemy name to that of the game object
        enemyName = this.gameObject.name;

        //create's the correct weapon for an enemy based on the spawned enemy's name
        currentEnemyWeapon = weaponController.MakeWeapon(enemyName);

        turretState = TurretState.SEARCHING;
    }

    protected override void Update()
    {
        inShootRange = false;
        isAggrod = false;

        currentRotation = transform.localEulerAngles;

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
                if (!isTurning)
                {
                    if (!swapDirection)
                    {
                        StartCoroutine(swivelView(currentRotation, leftTurn));
                    }
                    else
                    {
                        StartCoroutine(swivelView(currentRotation, rightTurn));
                    }

                }                
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

    private IEnumerator swivelView(Vector3 current, Vector3 TargetRotate)
    {
        Debug.Log("Swivelinggg");
        isTurning = true;
        while (t < timeToTurn)
        {
            transform.localEulerAngles = Vector3.Lerp(current, TargetRotate, t);
            t +=  0.5f * Time.deltaTime;
        }


        yield return new WaitForSeconds(AimingTime);
        t = 0;
        swapDirection = !swapDirection;
        isTurning = false;

        yield return null;
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
