using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathScript : MonoBehaviour
{
    EnemyBehavior character;
    //ParticleSystem particleObject;
    GameObject deathEffect;


    private Collider coll;
    private EnemyBehavior eb;
    private Navigation nav;
    private Renderer rend;

    [SerializeField] private Animator anim;

    //This shouldn't be the final destroy, only temporary
    [SerializeField] bool easyDestroy;

    // Start is called before the first frame update
    void Start()
    {
        if (!easyDestroy)
        {
            coll = GetComponent<Collider>();
            eb = GetComponent<EnemyBehavior>();
            nav = GetComponent<Navigation>();
            rend = GetComponentInChildren<Renderer>();
        }

        anim = GetComponent<Animator>();

        character = GetComponent<EnemyBehavior>();
        character.OnDeath += Character_OnDeath;

        //particleObject = GetComponentInChildren<ParticleSystem>();
        //particleObject.Stop();
    }

    private void Character_OnDeath(object sender, System.EventArgs e)
    {
        if (easyDestroy)
        { 
            deathEffect = (GameObject)Resources.Load("DeathEffect");
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        else
        {
            DisableComponents();
            //particleObject.Play();

            deathEffect = (GameObject)Resources.Load("DeathEffect");
            Instantiate(deathEffect, transform.position, Quaternion.identity);
        }
        
        //DisableComponents();
        ////particleObject.Play();

        //deathEffect = (GameObject)Resources.Load("DeathEffect");
        //Instantiate(deathEffect, transform.position, Quaternion.identity);

        ////Destroy(gameObject, particleObject.duration);
        Destroy(gameObject, 3.0f);
    }

    private void DisableComponents()
    { 
        //rend.enabled = false;
        coll.enabled = false;
        eb.enabled= false;
        nav.enabled = false;

        anim.SetBool("Killed", true);
    }
}
