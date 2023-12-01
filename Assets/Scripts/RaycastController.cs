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



    Vector3 projectionVector;


    Vector3 mousePosition;

    // Start is called before the first frame update
    void Start()
    {

        //projectionVector = GunBarrelLocation.transform.position - AimCursor.cursorLocation;
    }

    


    // Update is called once per frame
    void Update()
    {

        positionOne = GunBarrelLocation.transform.position;

        projectileSpawnLocation = GunBarrelLocation;




        //set line renderer position to current player location
        lineRenderer.SetPosition(0, positionOne);
        
        //set line renderer position to current player location
        //lineRenderer.SetPosition(2, positionOne);
    }

    RaycastHit hit;

    Color darkGreen = new Color(.03f, 0.69f, 0.0f);
    Color lightGreen = new Color(.00f, 1.00f, 0.0f);

    Color darkRed = new Color(0.77f, 0.00f, 0.00f);
    Color lightRed = new Color(1.00f, 0.42f, 0.42f);



    //Updates after update, used for physics related calculations and functions
    private void FixedUpdate()
    {
       

        if (Physics.Raycast(positionOne, gameObject.transform.forward, out hit))
        {
            //lineRenderer.SetPosition(1, hit.point);
            lineRenderer.SetPosition(1, hit.point);

        }
        else
        {
            lineRenderer.SetPosition(1, (positionOne - transform.position).normalized + transform.position);

        }




        if (hit.transform.gameObject.CompareTag("ActivatableObject") || hit.transform.gameObject.CompareTag("LimitedBounceObject") || hit.transform.gameObject.TryGetComponent<IDamagable>(out IDamagable component))
        {
            gameObject.GetComponent<LineRenderer>().material.SetColor("_LineColor", lightRed);
            gameObject.GetComponent<LineRenderer>().material.SetColor("_OutlineColor", darkRed);
            lineRenderer.SetPosition(2, hit.point);
        }
        else
        {
            gameObject.GetComponent<LineRenderer>().material.SetColor("_LineColor", lightGreen);
            gameObject.GetComponent<LineRenderer>().material.SetColor("_OutlineColor", darkGreen);
            lineRenderer.SetPosition(2, hit.point);
            //Bounce(hit);
        }

    }

    Vector2 collisionNormal;
    Vector2 direction2D;
    Vector3 direction;
    void Bounce(RaycastHit bouncePoint)
    {
            direction = GunBarrelLocation.transform.right;


            collisionNormal = new Vector2(bouncePoint.normal.x, bouncePoint.normal.z).normalized;

            direction2D = (new Vector2(direction.x, direction.z));

            direction2D = (direction2D - 2 * (Vector2.Dot(direction2D, collisionNormal)) * collisionNormal);

            direction = new Vector3(direction2D.x, 0, direction2D.y);

            lineRenderer.SetPosition(2, bouncePoint.point + (direction.normalized * 5.0f));

            
        
    }


}


