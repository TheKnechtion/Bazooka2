using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDecorator : MonoBehaviour
{
    [SerializeField] private GameObject PrefabEnemy;

    [Header("Override Attributes")]
    [SerializeField] private bool Override_HP;
    [SerializeField] private bool Override_Armored;
    [SerializeField] private bool Override_Stationary;
    [SerializeField] private bool Override_AggroRange;
    [SerializeField] private bool Override_AttackRange;
    [SerializeField] private bool Override_Weapon;
    [SerializeField] private bool Override_VerticalAim;

    [Header("Override Values")]
    [SerializeField] private int NewHP;
    [SerializeField] private bool IsArmored;
    [SerializeField] private bool IsStationary;
    [SerializeField] private float NewAggroRange;
    [SerializeField] private float NewAttackRange;

    [Range(1f, 60f)]
    [Tooltip("Put 1 for NO vertical aiming")]
    [SerializeField] private float MaxVerticalAim;

    [SerializeField] private GameObject NewWeapon;
    public void SpawnEnemy()
    {
        if (PrefabEnemy != null)
        {
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit spawn, 2.0f, NavMesh.AllAreas))
            {
                GameObject enemy = Instantiate(PrefabEnemy, spawn.position, transform.rotation);

                ConfigureEnemy(enemy);
            }
        }
    }

    private void ConfigureEnemy(GameObject enemy)
    {
        EnemyBehavior eb = enemy.GetComponent<EnemyBehavior>();

        if (Override_Stationary)
        {
            eb.gameObject.GetComponent<Navigation>().DisableMovement = Override_Stationary;
        }
        if (Override_HP)
        {
            eb.maxHealth = NewHP;
        }
        if (Override_AggroRange)
        {
            eb.enemyAttackRange_BecomeAggro = NewAggroRange;
        }
        if (Override_AttackRange)
        {
            eb.enemyAttackRange_AttackRange = NewAttackRange; 
        }
        if (Override_Weapon)
        {
            eb.gameObject.GetComponent<EnemyWeaponController>().InitWeapon(NewWeapon);
        }
        if (Override_VerticalAim)
        {
            eb.maxAimingAngle = MaxVerticalAim;
        }
        if (Override_Armored)
        {
            eb.ArmoredTarget = IsArmored;
        }
    }
}
