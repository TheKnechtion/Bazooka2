using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class RobotBehavior : EnemyBehavior
{
    /// <summary>
    /// Refernce to this Robot's rigidbody
    /// </summary>
    private Rigidbody myBody;

    /// <summary>
    /// The area transform that will interact with the Player for grabbing.
    /// </summary>
    /// 
    [Tooltip("The area transform that will interact with the Player for grabbing")]
    [SerializeField] private Transform GrabZone;
    private Vector3 GrabPoint;

    /// <summary>
    /// How long the robot will be stunned for after letting go.
    /// </summary>
    /// 
    [Tooltip("How long the robot will be stunned for after letting go")]
    [SerializeField] private float StunLength;

    /// <summary>
    /// The key-input assigned to escape
    /// </summary>
    [SerializeField] private KeyCode EscapeKey;

    private bool StunCalled;
    private bool Grabbing;

    private int EscapedPressCount;
    private int EscapedPressRequired;

    //Had to do this, currentState was conflicting with switch-statement,
    //Not sure why...
    private EnemyState RobotState;

    /// <summary>
    /// This will be referneced to the player when grabbed, not a dependency
    /// </summary>
    private GameObject PlayerObject;

    public UnityEvent OnGrabEvent;
    public UnityEvent OnLetGoEvent;



    [SerializeField] private GameObject DebugUIObj;
    private TextMeshProUGUI DebugUI;

    protected override void Awake()
    {
        if (TryGetComponent<Animator>(out Animator anim))
        {
            movementAnimator = anim;
        }
        else if (GetComponentInChildren<Animator>() != null)
        {
            movementAnimator = GetComponentInChildren<Animator>();
        }

        if (animController)
        {
            RuntimeAnimatorController newCon = Instantiate(animController);
            movementAnimator.runtimeAnimatorController = newCon;
        }
        else
        {
            Debug.LogWarning("! No animController Set !");
        }
    }
    protected override void Start()
    {
        base.Start();

        myBody = GetComponent<Rigidbody>();

        RobotState = EnemyState.IDLE;

        if (GrabZone == null)
        {
            GrabZone = transform.Find("GrabZone");
            GrabPoint = GrabZone.position;
        }

        StunCalled = false;
        Grabbing = false;

        EscapedPressCount = 0;
        EscapedPressRequired = 5;

        DebugUI = DebugUIObj.GetComponentInChildren<TextMeshProUGUI>();   
    }

    protected override void Update()
    {
        if (GrabZone != null)
        {
            GrabPoint = GrabZone.position;
        }

        isAggrod = Physics.CheckSphere(gameObject.transform.position, enemyAttackRange_BecomeAggro, playerMask);

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

        Debug.Log("Pre Switch: "+ RobotState);

        if (PlayerInfo.instance.HeatlthState == PlayerHealthState.DEAD)
        {
            RobotState = EnemyState.IDLE;
        }

        if (Time.timeScale == 1.0f)
        {
            switch (RobotState)
            {
                case EnemyState.IDLE:
                    Debug.Log("Switch Idle");
                    nav.StopMovement();

                    if (!StunCalled)
                    {
                        if (isAggrod)
                        {
                            RobotState = EnemyState.CHASE;
                        }
                    }

                    break;
                case EnemyState.CHASE:
                    Debug.Log("Switch Chase");

                    if (movementAnimator != null)
                    {
                        movementAnimator.SetBool("Walk", true);
                    }

                    if (nav != null)
                    {
                        nav.MoveToPlayer(true, false);
                    }
                    if (!isAggrod)
                    {
                        RobotState = EnemyState.IDLE;
                    }
                    if (Grabbing)
                    {
                        RobotState = EnemyState.ATTACK;
                    }

                    break;
                case EnemyState.ATTACK:
                    Debug.Log("Switch Attack");
                    nav.StopMovement();

                    if (movementAnimator != null)
                    {
                        movementAnimator.SetBool("Grab", true);
                    }

                    if (Grabbing)
                    {
                        PlayerObject.transform.position = GrabPoint;
                        PlayerObject.transform.rotation = Quaternion.LookRotation(-enemyLookDirection);
                    }

                    if (!StunCalled && Input.GetKeyDown(EscapeKey))
                    {
                        EscapedPressCount++;
                        if (EscapedPressCount >= EscapedPressRequired)
                        {
                            Grabbing = false;

                            if (PlayerObject != null)
                            {
                                OnLetGo(PlayerObject);
                            }
                        }
                    }

                    if (DebugUI != null)
                    {
                        if (!DebugUI.isActiveAndEnabled)
                        {
                            DebugUI.gameObject.SetActive(true);
                        }

                        displayInputs();
                    }

                    break;
                default:
                    RobotState = EnemyState.IDLE;
                    break;
            }
            Debug.Log("Post Switch: " + RobotState);
        }       

    }

    protected override void FixedUpdate()
    {

    }

    protected override void HandleEnemyAggro()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        OnGrab(other.gameObject);
    }
    public void OnGrab(GameObject other)
    {        

        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement t) &&
            other.TryGetComponent<PlayerManager>(out PlayerManager m))
        {
            GrabZone.gameObject.SetActive(false);

            nav.CancelPath();
            PlayerObject = other.gameObject;

            OnGrabEvent.Invoke();
            Grabbing = true;
            EscapedPressCount = 0;

            t.enabled = false;
            m.enabled = false;

        }
    }

    public void OnLetGo(GameObject other)
    {
        if (other.TryGetComponent<PlayerMovement>(out PlayerMovement t) &&
            other.TryGetComponent<PlayerManager>(out PlayerManager m))
        {
            OnLetGoEvent.Invoke();

            t.enabled = true;
            m.enabled = true;
           
            RobotState = EnemyState.IDLE;


            if (!StunCalled)
            {
                StartCoroutine(RobotStun(StunLength));
            }

            if (DebugUI != null)
            {
                DebugUI.gameObject.SetActive(false);
            }

        }
    }
    private IEnumerator RobotStun(float time)
    {
        if (movementAnimator != null)
        {
            movementAnimator.SetBool("Grab", false);
            movementAnimator.SetBool("Shutdown", true);
        }

        StunCalled = true;
        EscapedPressCount = 0;
        float t = 0.0f;
        GrabZone.gameObject.SetActive(false);

        while (t < time)
        {
            t += Time.deltaTime;
            yield return null;
        }

        StunCalled = false;

        GrabZone.gameObject.SetActive(true);
        movementAnimator.SetBool("Shutdown", false);


        yield return null;
    }

    private IEnumerator SmoothGrab(GameObject passedObj, Vector3 finalPos, float time)
    {
        float t = 0.0f;
        while(t < 1 || passedObj.transform.position != finalPos)
        {
            passedObj.transform.position = Vector3.Lerp(passedObj.transform.position, finalPos, t);
            t += Time.deltaTime;
            yield return null;
        }

        passedObj.transform.position = finalPos;

        yield return null;
    }

    private void displayInputs()
    {
        DebugUI.text = $"{EscapedPressCount}/{EscapedPressRequired}";
    }
}
