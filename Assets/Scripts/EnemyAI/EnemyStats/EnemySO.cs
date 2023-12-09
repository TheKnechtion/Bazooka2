using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = " New Enemy Stats")]
public class EnemySO : ScriptableObject
{
    public string Name;
    public string WeaponName;

    public bool ArmoredTarget;

    public int MP;
    public int AP;
    public int DEF;
    public int Health;

    public float AggroRange;
    public float AtackRange;

    public LayerMask playerMask;
    public LayerMask environmentMask;

}
