using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    //store the lineRenderer on the player
    [SerializeField] LineRenderer lineRenderer;

    //store the current player position
    Vector3 positionOne;


    public LayerMask ignoreProjectileLayerObjects;

    public GameObject GunBarrelLocation;

    public static GameObject projectileSpawnLocation;



    // Start is called before the first frame update
    void Start()
    {

    }

    


    // Update is called once per frame
    void Update()
    {

        positionOne = GunBarrelLocation.transform.position;

        projectileSpawnLocation = GunBarrelLocation;




        //set line renderer position to current player location
        lineRenderer.SetPosition(0, positionOne);
        
        //set line renderer position to current player location
        lineRenderer.SetPosition(2, positionOne);
    }

    RaycastHit hit;

    //Updates after update, used for physics related calculations and functions
    private void FixedUpdate()
    {
        Physics.Raycast(positionOne, gameObject.transform.forward, out hit);
        lineRenderer.SetPosition(1, hit.point);

    }

}

