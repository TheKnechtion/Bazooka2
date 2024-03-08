using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : DestroyableObject
{
    [SerializeField] private float fuseTime;
    public float FuseTime { 
        get { return fuseTime; } 
        set 
        {
            if (value > 0)
            {
                fuseTime = value;
            }
            else
            {
                fuseTime = 0;
            }
        } 
    }

    [SerializeField] private Explosive explosive;
    private bool isDestroyed;

    [SerializeField] private GameObject meshObject;
    [SerializeField] private Material fuseMaterial;

    Material[] explosiveMats;

    private Renderer render;

    // Start is called before the first frame update
    void Start()
    {
        if (render == null)
        {
            render = meshObject.GetComponent<Renderer>();
            explosiveMats = new Material[2];
        }

        isDestroyed = false;
        explosive = this.GetComponent<Explosive>();

        explosive.CanDestroy += OnFinishedExploding;
    }

    private void OnFinishedExploding(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }

    public override void Die()
    {
        health= 0;
        if (!isDestroyed) 
        {
            isDestroyed= true;
            StartCoroutine(fuse());
            //PickupAbleOBJ_Destroy();
        }
       
        //Custom explosion delete
    }



    private IEnumerator fuse()
    {
        if (fuseMaterial != null)
        {
            explosiveMats[0] = render.material;
            explosiveMats[1] = fuseMaterial;
            render.materials = explosiveMats;
        }

        yield return new WaitForSeconds(fuseTime);

        explosive.Explode();

        PickupAbleOBJ_Destroy();
        yield return null;
    }



}
