using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class EntityInfo : MonoBehaviour, IDamagable
{
    public string Name { get; set; }

    public int health = 2;

    private void Update()
    {
        if (health <= 0) { Destroy(gameObject); }
    }

    public void TakeDamage(int passedDamage)
    {
        health -= passedDamage;
    }

    //public int CurrentHP { get; set; }

    //public int MaxHP { get; set; }

    //public int CurrentEXP { get; set; }

    //List<WeaponInfo> OwnedWeapons { get; set; }

    private void OnDestroy()
    {

    }

}
