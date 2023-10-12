using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : MonoBehaviour
{
    EnemyBehavior character;
    //ParticleSystem particleObject;
    UnityEngine.Object deathEffect;


    private CapsuleCollider coll;
    private EnemyBehavior eb;
    private Navigation nav;
    private Renderer rend;

    // Start is called before the first frame update
    void Start()
    {
        coll = GetComponent<CapsuleCollider>();
        eb= GetComponent<EnemyBehavior>();
        nav = GetComponent<Navigation>();
        rend = GetComponent<Renderer>();

        character = GetComponent<EnemyBehavior>();
        character.OnDeath += Character_OnDeath;

        //particleObject = GetComponentInChildren<ParticleSystem>();
        //particleObject.Stop();
    }

    private void Character_OnDeath(object sender, System.EventArgs e)
    {
        DisableComponents();
        //particleObject.Play();

        deathEffect = Resources.Load("DeathEffect");
        Instantiate(deathEffect, transform.position, Quaternion.identity);

        //Destroy(gameObject, particleObject.duration);
        Destroy(gameObject, 3.0f);
    }

    private void DisableComponents()
    { 
        rend.enabled = false;
        coll.enabled = false;
        eb.enabled= false;
        nav.enabled = false;
    }
}
