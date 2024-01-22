using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveBarrel : DestroyableObject
{
    [SerializeField] private float FuseTime;

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
        if (fuseMaterial)
        {
            explosiveMats[0] = render.material;
            explosiveMats[1] = fuseMaterial;
            render.materials = explosiveMats;
        }

        yield return new WaitForSeconds(FuseTime);

        explosive.Explode();

        PickupAbleOBJ_Destroy();
        yield return null;
    }



}
