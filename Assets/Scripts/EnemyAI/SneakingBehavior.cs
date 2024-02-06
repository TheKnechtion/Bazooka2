using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StealthState {HIDDEN, SPOTTED}
public class SneakingBehavior : EnemyBehavior, ISpottable
{
    private SneakingNavigation sneakNav;

    [SerializeField] private StealthState stealthState;
    private Vector3 targetPos; //Uses Player singelton
    private Vector3 nextPos;

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

    private bool isFleeing;
    private bool foundPoint;

    protected override void Awake()
    {
        remainingDistance = 0f;
    }
    protected override void Start()
    {
        stealthState = StealthState.HIDDEN;
        currentState = EnemyState.IDLE;

        sneakNav = GetComponent<SneakingNavigation>();

        nextPos = Vector3.zero;

        isFleeing = false;
        foundPoint = false;

        attackZone.SetActive(false);
    }

    protected override void Update()
    {
        targetPos = PlayerInfo.instance.playerPosition;


        /*
         * Hunting = Hidden & Aggro;
         * Attacking = Hidden & In Range
         * 
         * Fleeing = Spotted 
         */

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

                //Deal damage to player within trigger attack-zone
                //Play attack anim

                //Scurry after Attacking (Will be same as spotted scurry)

                if (damagable != null)
                {
                    damagable.TakeDamage(damage);
                }
                attackZone.SetActive(false);


                currentState = EnemyState.FLEE;

                break;

            case EnemyState.FLEE:

                sneakNav.SetSpeed(runSpeed);

                if (!isFleeing)
                {
                    StartCoroutine(FleeArea());
                }
                else if (gameObject.transform.position == nextPos)
                {
                    isFleeing = false;

                    stealthState = StealthState.HIDDEN;
                    currentState = EnemyState.IDLE;
                }

                break;
            default:
                break;
        }
    }

    private IEnumerator FleeArea()
    {
        isFleeing = true;
        foundPoint = false;
        while (!foundPoint)
        {
            foundPoint = sneakNav.GetValidRandomPoint(gameObject.transform.position, out nextPos);
            if (nextPos == gameObject.transform.position)
            {
                foundPoint = false;
            }

            yield return null;
        }

        sneakNav.MoveTo(nextPos);
    }

    protected override void FixedUpdate()
    {

    }    

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && other.TryGetComponent<IDamagable>(out damagable))
        {
            currentState = EnemyState.ATTACK;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        
    }
    private void OnDrawGizmos()
    {

    }

    public void Spot()
    {
        stealthState = StealthState.SPOTTED;
    }
}
