using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum StealthState {HIDDEN, SPOTTED}
public class SneakingBehavior : EnemyBehavior
{
    private SneakingNavigation sneakNav;

    private StealthState stealthState;
    private Vector3 targetPos; //Uses Player singelton
    private Vector3 nextPos;

    [Header("How close enemy must be to begin attack")]
    [SerializeField] private float enagageDistance;
    private float remainingDistance;

    [Header("Attack Attributes")]
    [SerializeField] private int damage;
    [SerializeField] private GameObject attackZone;

    private IDamagable damagable;

    private bool isFleeing;

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
                HandleHunting();

                break;
            case StealthState.SPOTTED:

                //Run away behavior
                Flee();

                break;
            default:
                break;
        }

        Debug.DrawRay(nextPos, Vector3.up * 5, Color.green);
    }

    private void HandleHunting()
    {
        remainingDistance = sneakNav.GetDistance();

        switch (currentState)
        {
            case EnemyState.IDLE:
                attackZone.SetActive(false);

                sneakNav.SetSpeed(8);
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

                sneakNav.SetSpeed(2);
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

                sneakNav.SetSpeed(6);

                if (!isFleeing)
                {
                    Flee();
                }

                if (gameObject.transform.position == nextPos)
                {
                    isFleeing = false;
                    currentState = EnemyState.IDLE;
                }

                break;
            default:
                break;
        }
    }

    private void Flee()
    {
        isFleeing = true;
        nextPos = sneakNav.GetRandomNavPoint(gameObject.transform.position);
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
}
