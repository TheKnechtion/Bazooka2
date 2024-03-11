using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RobotBehavior : EnemyBehavior
{
    /// <summary>
    /// The area transform that will interact with the Player for grabbing.
    /// </summary>
    /// 
    [Tooltip("The area transform that will interact with the Player for grabbing")]
    [SerializeField] private Transform GrabZone;

    /// <summary>
    /// How long the robot will be stunned for after letting go.
    /// </summary>
    /// 
    [Tooltip("How long the robot will be stunned for after letting go")]
    [SerializeField] private float StunLength;

    private bool StunCalled;
    private bool Grabbing;

    public UnityEvent OnGrabEvent;
    public UnityEvent OnLetGoEvent;
    protected override void Start()
    {
        base.Start();

        if (GrabZone == null)
        {
            GrabZone = transform.Find("GrabZone");
        }

        StunCalled = false;
        Grabbing = false;
    }

    protected override void Update()
    {
        isAggrod = false;

        wallDetectPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 2, gameObject.transform.position.z);
        distanceToPlayer = Vector3.Distance(playerPosition, enemyPosition);

        //track the enemy position
        enemyPosition = gameObject.transform.position;

        //track the player position
        playerPosition = PlayerInfo.instance.playerPosition;

        //used by the enemy aggro system to see how far the player is from the enemy
        enemyPlayerTracker = Vector3.Distance(playerPosition, enemyPosition);

        //creates an enemy look direction based on the enemy position and the player's current position
        enemyLookDirection = (playerPosition - enemyPosition).normalized;

        if (PlayerInfo.instance.HeatlthState == PlayerHealthState.DEAD)
        {
            currentState = EnemyState.IDLE;
        }
        else
        {
            HandleEnemyAggro();
        }

        switch (currentState)
        {
            case EnemyState.IDLE:

                break;
            case EnemyState.CHASE:

                if (nav != null)
                {
                    nav.MoveToPlayer(true, false);
                }

                break;
            case EnemyState.ATTACK:                

                break;
            default:
                break;
        }

        if (!Grabbing)
        {
            nav.MoveToPlayer(true, false);
        }
        else
        {
            nav.StopMovement();
        }

    }

    protected override void HandleEnemyAggro()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        OnGrab(other);
    }
    private void OnTriggerExit(Collider other)
    {
        OnLetGo(other);
    }

    public void OnGrab(Collider other)
    {
        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement t) &&
            other.TryGetComponent<PlayerManager>(out PlayerManager m))
        {
            OnGrabEvent.Invoke();
            Grabbing = true;

            t.enabled = false;
            m.enabled = false;
        }
    }

    public void OnLetGo(Collider other)
    {
        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement t) &&
            other.TryGetComponent<PlayerManager>(out PlayerManager m))
        {
            OnLetGoEvent.Invoke();
            Grabbing = false;

            t.enabled = true;
            m.enabled = true;

            if (!StunCalled)
            {
                StartCoroutine(RobotStun(StunLength));
            }
        }
    }

    private IEnumerator RobotStun(float time)
    {
        StunCalled = true;

        float t = 0.0f;

        //Set state to stunned

        while(t < time)
        {


            t += Time.deltaTime;
            yield return null;
        }

        StunCalled = false;

        yield return null;
    }
}
