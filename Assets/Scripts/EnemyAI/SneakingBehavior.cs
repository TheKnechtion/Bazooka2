using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.AI;
using UnityEngine.AI;

public enum StealthState {HIDDEN, SPOTTED}
public class SneakingBehavior : EnemyBehavior, ISpottable
{
    private SneakingNavigation sneakNav;

    [SerializeField] private StealthState stealthState;
    private Vector3 targetPos; //Uses Player singelton
    private Vector3 nextPos;

    private const float AnimCrossFadeTime = 0.2f;

    [Header("How close enemy must be to begin attack")]
    [SerializeField] private float enagageDistance;
    private float remainingDistance;

    [Header("Attack Attributes")]
    [SerializeField] private int damage;
    [SerializeField] private GameObject attackZone;

    [Header("Movement Attributes")]
    [Tooltip("Speed when enemy is behind player")]
    [SerializeField] private float huntSpeed;
    [Tooltip("Speed when enemy is fleeing/catching up")]
    [SerializeField] private float runSpeed;
    [SerializeField] private float walkSpeed;

    private IDamagable damagable;

    private bool IsFleeing;
    private bool FoundPoint;
    private bool Attacking;
    protected override void Awake()
    {
        base.Awake();
        remainingDistance = 0f;
    }
    protected override void Start()
    {
        stealthState = StealthState.HIDDEN;
        currentState = EnemyState.IDLE;

        sneakNav = GetComponent<SneakingNavigation>();
        agent = GetComponent<NavMeshAgent>();

        nextPos = Vector3.zero;

        IsFleeing = false;
        FoundPoint = false;
        Attacking = false;

        attackZone.SetActive(false);
    }

    protected override void Update()
    {
        if (PlayerInfo.instance != null)
        {
            targetPos = PlayerInfo.instance.playerPosition;
        }

        switch (stealthState)
        {
            case StealthState.HIDDEN:

                // Allow hunting and attack behavior

                break;
            case StealthState.SPOTTED:

                //Run away behavior
                currentState = EnemyState.FLEE;

                break;
            default:
                break;
        }

        HandleHunting();


        Debug.DrawRay(nextPos, Vector3.up * 5, Color.green);
    }

    private void HandleHunting()
    {
        remainingDistance = sneakNav.GetDistance();

        switch (currentState)
        {
            case EnemyState.IDLE:

                attackZone.SetActive(false);

                sneakNav.SetSpeed(walkSpeed);
                sneakNav.MoveTo(targetPos);

                if (remainingDistance <= enagageDistance)
                {
                    currentState = EnemyState.CHASE;
                }                

                break;
            case EnemyState.CHASE:
                //Approach player from behind
                //When in range, switch to ATTACK

                attackZone.SetActive(true);

                sneakNav.SetSpeed(huntSpeed);
                sneakNav.MoveTo(targetPos);

                if (remainingDistance > enagageDistance)
                {
                    currentState = EnemyState.IDLE;
                }

                break;
            case EnemyState.ATTACK:


                if (damagable != null && !Attacking)
                {
                    //Play attack animation and Damage
                    StartCoroutine(AttackAndReset(0.3f));
                }
                attackZone.SetActive(false);


                sneakNav.CancelPath();
                currentState = EnemyState.FLEE;

                break;

            case EnemyState.FLEE:

                sneakNav.SetSpeed(runSpeed);

                if (!IsFleeing)
                {
                    StartCoroutine(FleeArea());
                }
                else if (ApproximatePosition(gameObject.transform.position, nextPos))
                {
                    IsFleeing = false;

                    stealthState = StealthState.HIDDEN;
                    currentState = EnemyState.IDLE;
                }

                break;
            default:
                break;
        }

        movementAnimator.SetFloat("MoveSpeed", agent.speed);

        //Debug.Log(nextPos);
    }

    private IEnumerator FleeArea()
    {
        sneakNav.CancelPath();

        IsFleeing = true;
        FoundPoint = false;
        while (!FoundPoint)
        {
            FoundPoint = sneakNav.GetValidRandomPoint(gameObject.transform.position, out nextPos);
            if (nextPos == gameObject.transform.position)
            {
                FoundPoint = false;
            }

            yield return null;
        }

        sneakNav.MoveTo(nextPos);
    }  

    private IEnumerator AttackAndReset(float time)
    {
        Attacking = true;
        movementAnimator.Play("attack");
        damagable.TakeDamage(damage);

        float t = 0.0f;
        while (t < time)
        {
            t += Time.deltaTime;
            yield return null;
        }
        Attacking = false;
        yield return null;
    }

    private bool ApproximatePosition(Vector3 start, Vector3 compareTo)
    {
        return Mathf.Abs(start.x - compareTo.x) <= 0.5f &&
               Mathf.Abs(start.y - compareTo.y) <= 0.5f &&
               Mathf.Abs(start.z - compareTo.z) <= 0.5f;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.TryGetComponent<IDamagable>(out damagable))
        {
            currentState = EnemyState.ATTACK;
        }
    }
    public void Spot()
    {
        stealthState = StealthState.SPOTTED;
    }
}
