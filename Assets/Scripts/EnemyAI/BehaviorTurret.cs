using System.Collections;
using System.Collections.Generic;
using UnityEditor;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public enum TurretState {SEARCHING, ENGAGED, LOSTSIGHT }
public class BehaviorTurret : EnemyBehavior
{
    [SerializeField] private TurretState turretState;

    [SerializeField] private float VisionRange;
    private float DotProduct;


    [SerializeField] private float AimingTime;

    [SerializeField] private float TurningAngle;
    private Vector3 leftTurn;
    private Vector3 rightTurn;
    private Vector3 currentRotation;

    private Quaternion leftTurnQuat;
    private Quaternion rightTurnQuat;
    private Quaternion currentRotationQuat;

    //Used for going BACK to searching when player leaves vision
    [SerializeField] private float DeAggroTime;
    private float timeToDeagro;

    private bool inRange;
    private bool isTurning;
    private bool swapDirection;

    private Quaternion initRotation;
    private Vector3 initRotationEuler;

    private float t;
    private float timeToTurn;

    // Start is called before the first frame update
    void Start()
    {
        setStats();

        t = 0;
        timeToTurn = 1.5f; 
        isTurning = false;
        swapDirection = false;

        //A reference, so we can reset later
        initRotation = transform.rotation;
        initRotationEuler = transform.eulerAngles;

        leftTurn = new Vector3(0, initRotationEuler.y-TurningAngle,0);
        rightTurn = new Vector3(0, initRotationEuler.y + TurningAngle,0);

        leftTurnQuat = Quaternion.Euler(leftTurn);
        rightTurnQuat = Quaternion.Euler(rightTurn);    

        //Pass the weapon script that attacthed to the object
            //weaponController = gameObject.GetComponent<WeaponController>();
        weaponGrabber = gameObject.GetComponent<DataBaseWeaponGrabber>();


        //set the enemy name to that of the game object
        //enemyName = this.gameObject.name;

        //create's the correct weapon for an enemy based on the spawned enemy's name
        //currentEnemyWeapon = weaponController.MakeWeapon(enemyName);
        currentEnemyWeapon = weaponGrabber.MakeWeapon(weaponName);

        turretState = TurretState.SEARCHING;
    }

    protected override void Update()
    {
        inShootRange = false;
        isAggrod = false;

        currentRotationQuat = transform.rotation;
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
                //Debug.Log("Looking for you...");

                timeToDeagro = DeAggroTime;
                if (!isTurning)
                {
                    if (!swapDirection)
                    {
                        //StartCoroutine(swivelView(currentRotation, leftTurn));
                        StartCoroutine(swivelView(currentRotationQuat, leftTurnQuat));
                    }
                    else
                    {
                        //StartCoroutine(swivelView(currentRotation, rightTurn));
                        StartCoroutine(swivelView(currentRotationQuat, rightTurnQuat));
                    }

                }                
                break;
            case TurretState.ENGAGED:
                //Debug.Log("I SEE YOU");
                StopAllCoroutines();
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
                //Debug.Log("Whered you go...");
                break;
            default:
                break;
        }


        #region Debug Logs
        //Debug.Log("inRange "+inRange);
        //Debug.Log("Spotted: "+ playerWasSpotted);
        //Debug.Log("Aggro Time: " + timeToDeagro);
        //Debug.Log(playerWasSpotted);
        //Debug.Log("To Deagro: "+timeToDeagro);
        //Debug.Log("Ref Deagro: "+DeAggroTime);

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
                transform.rotation = initRotation;
                turretState = TurretState.SEARCHING;
            }
        }
    }

    private IEnumerator swivelView(Quaternion current, Quaternion TargetRotate)
    {
        isTurning = true;
        while (t < timeToTurn)
        {
            transform.rotation = Quaternion.Slerp(current, TargetRotate, t);
            t += 0.1f * Time.deltaTime;
        }


        yield return new WaitForSeconds(AimingTime);
        t = 0;
        swapDirection = !swapDirection;
        isTurning = false;

        yield return null;
    }

    //private IEnumerator swivelView(Vector3 current, Vector3 TargetRotate)
    //{
    //    isTurning = true;
    //    while (t < timeToTurn)
    //    {
    //        transform.eulerAngles = Vector3.Lerp(current, TargetRotate, t);
    //        t +=  0.5f * Time.deltaTime;
    //    }


    //    yield return new WaitForSeconds(AimingTime);
    //    t = 0;
    //    swapDirection = !swapDirection;
    //    isTurning = false;

    //    yield return null;
    //}
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

        if (DotProduct > VisionRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    //private Vector3 ViewDirection(float EulerRotation, float DegreeView)
    //{
    //    DegreeView += EulerRotation;
    //    return new Vector3(Mathf.Sin(DegreeView * Mathf.Deg2Rad), 0 , Mathf.Cos(DegreeView * Mathf.Deg2Rad));
    //}
    //protected override void OnDrawGizmosSelected()
    //{
    //    base.OnDrawGizmosSelected();

    //    Handles.color = Color.green;
    //    Vector3 leftViewSight  = ViewDirection(currentRotation.y, TurningAngle);
    //    Vector3 rightViewSight = ViewDirection(currentRotation.y, -TurningAngle);

    //    Handles.DrawLine(transform.position, leftViewSight.normalized * enemyAttackRange_AttackRange);
    //    Handles.DrawLine(transform.position, rightViewSight.normalized * enemyAttackRange_AttackRange);
    //}

    
}
