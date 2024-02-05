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

    [Header("How close enemy must be to begin attack")]
    [SerializeField] private float enagageDistance;
    private float remainingDistance;

    [Header("Attack Attributes")]
    [SerializeField] private int damage;
    [SerializeField] private GameObject attackZone;

    private IDamagable damagable;

    protected override void Awake()
    {
        remainingDistance = 0f;
    }
    protected override void Start()
    {
        stealthState = StealthState.HIDDEN;
        currentState = EnemyState.IDLE;

        sneakNav = GetComponent<SneakingNavigation>();

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
                FleeArea();

                break;
            default:
                break;
        }
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
                    sneakNav.MoveTo(new Vector3(-45,0,0));
                }

                currentState = EnemyState.IDLE;

                break;
            default:
                break;
        }
    }

    private void FleeArea()
    {

    }


    protected override void FixedUpdate()
    {

    }

    private void OnDrawGizmos()
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
}
