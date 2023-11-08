using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDamagable 
{
    public bool ArmoredTarget { get;  set; }
    public void TakeDamage(int passedDamage);
    public void Die();
}
