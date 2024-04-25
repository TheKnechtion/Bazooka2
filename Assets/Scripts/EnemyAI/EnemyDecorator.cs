using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyDecorator : MonoBehaviour
{
    [SerializeField] private GameObject PrefabEnemy;

    [Header("Override Attributes")]
    [SerializeField] private bool MakeStationary;
    [SerializeField] private bool MakeArmored;
    [SerializeField] private bool Override_HP;
    [SerializeField] private bool Override_AggroRange;
    [SerializeField] private bool Override_AttackRange;
    [SerializeField] private bool Override_Weapon;
    [SerializeField] private bool Override_VerticalAim;

    [Header("Override Values")]
    [SerializeField] private int NewHP;
    [SerializeField] private float NewAggroRange;
    [SerializeField] private float NewAttackRange;

    [Range(0f, 60f)]
    [Tooltip("Put 0 for NO vertical aiming")]
    [SerializeField] private float MaxVerticalAim;

    [SerializeField] private GameObject NewWeapon;
    public void SpawnEnemy()
    {
        if (PrefabEnemy != null)
        {
            if (NavMesh.SamplePosition(transform.position, out NavMeshHit spawn, 2.0f, NavMesh.AllAreas))
            {
                GameObject e = Instantiate(PrefabEnemy, spawn.position, transform.rotation);

                EnemyBehavior eb = e.GetComponent<EnemyBehavior>();
                ConfigureEnemy(eb);
            }
        }
    }

    private void ConfigureEnemy(EnemyBehavior eb)
    {
        if (MakeStationary)
        {
            eb.gameObject.GetComponent<Navigation>().DisableMovement = MakeStationary;
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
            eb.gameObject.GetComponent<EnemyWeaponController>().weaponObj = NewWeapon;
        }
        if (Override_VerticalAim)
        {
            eb.maxAimingAngle = MaxVerticalAim;
        }
        if (MakeArmored)
        {
            eb.ArmoredTarget = MakeArmored;
        }
    }
}
