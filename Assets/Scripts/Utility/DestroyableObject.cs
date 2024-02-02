using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DestroyableObject : MonoBehaviour, IDamagable
{
    public int health;

    [Tooltip("Some items are meshes, others are objects. Assign the correct ones")]
    [SerializeField] private GameObject swapModel;
    private GameObject destroyModel;

    [SerializeField] private Mesh destroyedMesh;
    private MeshFilter renderMesh;
    public bool ArmoredTarget { get;  set; }

    public event EventHandler OnDestroyed;

    private bool dontDestroy;

    private void Start()
    {

        renderMesh = GetComponent<MeshFilter>();

        dontDestroy = false;
        if (swapModel != null)
        {
            dontDestroy = true;

            destroyModel = Instantiate(swapModel, gameObject.transform.position, gameObject.transform.rotation);

            destroyModel.SetActive(false);
        }

        if (destroyedMesh != null && renderMesh != null) 
        {
            dontDestroy = true;
        }



        ArmoredTarget = false;
    }

    public void TakeDamage(int passedDamage)
    {
        health -= passedDamage;

        if (health <= 0)
        {
            if (!dontDestroy)
            {
                Die();
            }
            else
            {
                Die();

                if (destroyModel)
                {
                    destroyModel.SetActive(true);
                }
                else if (destroyedMesh != null)
                {
                    renderMesh.mesh = destroyedMesh;
                }
            }
        }
    }

    public virtual void Die()
    {
        OnDestroyed?.Invoke(this, EventArgs.Empty);
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
            //Destroy(gameObject);
            Die();
        }


        
        if (parentObj.name == "AttachPoint")
        {
            tempManager = parentObj.parent.parent.parent.parent.GetComponent<PlayerManager>();
            tempManager.CanCarryObjectOnBack = true;
            tempManager.isCarryingObjectOnBack = false;


            Destroy(gameObject.transform.parent.gameObject);
            //Destroy(gameObject);
            Die();
        }
        else
        {
            //Destroy(gameObject);
            Die();
        }
        
    }
}
