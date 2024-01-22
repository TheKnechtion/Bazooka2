using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestroyableObject : MonoBehaviour, IDamagable
{
    public int health = 4;

    public bool ArmoredTarget { get;  set; }

    private void Start()
    {
        ArmoredTarget = false;
    }

    public void TakeDamage(int passedDamage)
    {
        health -= passedDamage;

        if (health <= 0)
        {
            Die();
        }
    }

    public virtual void Die()
    {
        Destroy(this.gameObject);
    }


    Transform? parentObj;
    PlayerManager tempManager;

    public void PickupAbleOBJ_Destroy()
    {
        //Destroy(gameObject);

        parentObj = this.gameObject.transform.parent;

        if (parentObj == null) 
        {
            Destroy(gameObject);
        }


        
        if (parentObj.name == "AttachPoint")
        {
            tempManager = parentObj.parent.parent.parent.parent.GetComponent<PlayerManager>();
            tempManager.CanCarryObjectOnBack = true;
            tempManager.isCarryingObjectOnBack = false;


            Destroy(gameObject.transform.parent.gameObject);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        
    }



}
