using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    //store the lineRenderer on the player
    [SerializeField] LineRenderer lineRenderer;

    //store the current player position
    Vector3 positionOne;
    Vector3 positionTwo;

    public LayerMask ignoreProjectileLayerObjects;

    public GameObject GunBarrelLocation;

    public static GameObject projectileSpawnLocation;

    public static Vector3 playerLookDirection;

    float cursorVariable;


    Vector3 projectionVector;
    public static Vector3 shootVector;

    // Start is called before the first frame update
    void Start()
    {
        PlayerManager.OnPlayerAim += StartAiming;
        PlayerManager.OnPlayerStopAim += StopAiming;
        //projectionVector = GunBarrelLocation.transform.position - AimCursor.cursorLocation;
    }

    


    // Update is called once per frame
    void Update()
    {
        positionOne = GunBarrelLocation.transform.position;

        projectileSpawnLocation = GunBarrelLocation;

        //set line renderer position to current player location+
        lineRenderer.SetPosition(0, positionOne);

        //set line renderer position to current player location
        //lineRenderer.SetPosition(2, positionOne);

        projectionVector = AimCursor.cursorVector.normalized;
        
        cursorVariable = (GunBarrelLocation.transform.position.y - AimCursor.cursorLocation.y) / (projectionVector.y);

        positionTwo = AimCursor.cursorLocation + (cursorVariable * projectionVector);
        //.cursorLocation

        playerLookDirection = (positionTwo - this.transform.position).normalized;
    }

    private void LateUpdate()
    {
        
    }

    RaycastHit hit;
    RaycastHit hitTwo;

    Color darkGreen = new Color(.03f, 0.69f, 0.0f);
    Color lightGreen = new Color(.00f, 1.00f, 0.0f);

    Color darkRed = new Color(0.77f, 0.00f, 0.00f);
    Color lightRed = new Color(1.00f, 0.42f, 0.42f);



    bool isAiming = false;

    static Vector3 boundlessLookVector;

    //Updates after update, used for physics related calculations and functions
    private void FixedUpdate()
    {        
        //if the player isn't aiming
        if (Physics.Raycast(positionOne, gameObject.transform.forward, out hit, 15f) && !isAiming)
        {
            lineRenderer.SetPosition(1, hit.point);
            UpdateShootVector(hit.point);
            lineHitTest(hit);
        }
        else
        {
            boundlessLookVector = gameObject.transform.forward;
            boundlessLookVector = new Vector3(boundlessLookVector.x, 0, boundlessLookVector.z);
            boundlessLookVector = positionOne + (15.0f * boundlessLookVector);
            lineRenderer.SetPosition(1, boundlessLookVector);

            UpdateShootVector(boundlessLookVector);
        }



        if (isAiming)
        {
            lineRenderer.SetPosition(1, positionTwo);


            UpdateShootVector(positionTwo);

            //check for object from player to cursor when aiming
            if (Physics.Raycast(positionOne, (positionTwo - positionOne).normalized, out hitTwo, 1.05f * (positionTwo - positionOne).magnitude))
            {
                lineHitTest(hitTwo);
                lineRenderer.SetPosition(1, hitTwo.point);
            }
            else
            {
                SetLineColor(lightGreen, darkGreen);
            }


            //check for object from cursor to game object
            if (Physics.Raycast(AimCursor.cursorLocation, AimCursor.cursorVector, out hit))
            {
                if (hit.transform.gameObject.CompareTag("ActivatableObject") || hit.transform.gameObject.CompareTag("LimitedBounceObject") || hit.transform.gameObject.TryGetComponent<IDamagable>(out IDamagable componentTwo))
                {
                    AimCursor.cursorImage.color = lightRed;
                }
                else
                {
                    AimCursor.cursorImage.color = lightGreen;
                }
            }
        }
    }


    void lineHitTest(RaycastHit hitThis)
    {
        if (hitThis.transform.gameObject.CompareTag("ActivatableObject") || hitThis.transform.gameObject.CompareTag("LimitedBounceObject") || hitThis.transform.gameObject.TryGetComponent<IDamagable>(out IDamagable component))
        {
            SetLineColor(lightRed, darkRed);
        }
        else
        {
            SetLineColor(lightGreen, darkGreen);
        }
    }


    void SetLineColor(Color innerColor, Color outerColor)
    {
        gameObject.GetComponent<LineRenderer>().material.SetColor("_LineColor", innerColor);
        gameObject.GetComponent<LineRenderer>().material.SetColor("_OutlineColor", outerColor);
    }

    void UpdateShootVector(Vector3 updateVector)
    {
        shootVector = (updateVector - positionOne).normalized;
        shootVector = new Vector3(shootVector.x, 0, shootVector.z).normalized;
    }


    void StartAiming(object sender, EventArgs e)
    {
        isAiming = true;
    }
    void StopAiming(object sender, EventArgs e)
    {
        isAiming = false;
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(AimCursor.cursorLocation, positionTwo);

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


