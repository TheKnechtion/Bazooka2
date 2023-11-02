using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BehaviorTankBoss : EnemyBehavior
{

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        //ensures that if the room  is beaten, this won't spawn again
        //if (GameObject.Find("GameManager").GetComponent<GameManager>().currentNode.isRoomBeaten) { Destroy(this.gameObject); };

        //Pass the weapon script that attacthed to the object
        weaponController = gameObject.GetComponent<WeaponController>();

        //set the enemy name to that of the game object
        enemyName = this.gameObject.name;

        //create's the correct weapon for an enemy based on the spawned enemy's name
        currentEnemyWeapon = weaponController.MakeWeapon(enemyName);

        //sets the initial state of an enemy to docile
        isAggrod = false;
        inShootRange = false;

        nav = GetComponent<Navigation>();
        nav.stoppingDistance = 0;

        targetToLookAt = PlayerInfo.instance.gameObject.transform;

        currentState = EnemyState.IDLE;
        Debug.Log("My Pos: " + gameObject.transform.position);
    }

    protected override void HandleEnemyAggro()
    {
        //Determines aggro of the enemy
        isAggrod = Physics.CheckSphere(gameObject.transform.position, enemyAttackRange_BecomeAggro, playerMask);
        inShootRange = Physics.CheckSphere(gameObject.transform.position, enemyAttackRange_AttackRange, playerMask);

        if (isAggrod)
        {
            transform.LookAt(targetToLookAt);
            //If agroed, we want to chase
            currentState = EnemyState.CHASE;

            Ray wallDetect = new Ray(gameObject.transform.position, enemyLookDirection);
            RaycastHit hit;

            nav.MoveToPlayer(isAggrod, false);

            #region Detecting Wall Raycast
            //Debug.DrawRay(gameObject.transform.position, enemyLookDirection.normalized, Color.black);
            //if (Physics.Raycast(wallDetect, out hit, 10, environmentMask))
            //{
            //    //Debug.Log("I hit a wall");
            //    nav.MoveToPlayer(isAggrod, false);

            //}
            //else
            //{
            //    nav.MoveToPlayer(isAggrod, true);
            //    //Debug.Log("Not hitting wall");
            //}
            #endregion

            if (SetToAttack)
            {
                Physics.Raycast(weaponProjectileSpawnNode.transform.position, transform.forward, out hit);

                if (inShootRange && hit.collider.gameObject.tag == "Player")
                {
                    currentState = EnemyState.ATTACK;
                    HandleShooting();
                }
            }
        }
        else
        { currentState= EnemyState.IDLE; }
    }
}
