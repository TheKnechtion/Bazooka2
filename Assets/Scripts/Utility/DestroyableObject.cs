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

    public GameObject[] destroyThese;


    [SerializeField] private Mesh destroyedMesh;
    private MeshFilter renderMesh;

    [SerializeField] private ParticleSystem destroyEffect;
    [SerializeField] private ParticleSystem smokeEffect;
    public bool ArmoredTarget { get;  set; }

    public event EventHandler OnDestroyed;

    private bool dontDestroy;

    private void Start()
    {
        if (gameObject.TryGetComponent<MeshFilter>(out MeshFilter t))
        {
            renderMesh = t;
        }
        else if (gameObject.GetComponentInChildren<MeshFilter>())
        {
            renderMesh = gameObject.GetComponentInChildren<MeshFilter>();
        }

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
        if (health>0)
        {
            health -= passedDamage;

            if (health <= 0)
            {
                Die();
            }
        }        
    }

    public virtual void Die()
    {
        OnDestroyed?.Invoke(this, EventArgs.Empty);

        if (dontDestroy)
        {
            if (destroyEffect != null)
            {
                destroyEffect.Play();
            }
            if (smokeEffect !=null)
            {
                smokeEffect.Play();
            }
            if (destroyModel)
            {
                destroyModel.SetActive(true);
            }
            else if (destroyedMesh != null)
            {
                renderMesh.mesh = destroyedMesh;
            }

            for (int i = 0; i < destroyThese.Length; i++)
            {
                Destroy(destroyThese[i]);
                destroyThese[i] = null;
            }

            this.enabled = false;
        }
        else
        {
            Destroy(gameObject);
        }
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
        else
        {
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
}
