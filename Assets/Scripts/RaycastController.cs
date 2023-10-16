using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastController : MonoBehaviour
{
    //store the lineRenderer on the player
    [SerializeField] LineRenderer lineRenderer;

    //store the current player position
    Vector3 playerPosition;

    //variable that holds value for the x,y center of the screen in pixels
    Vector2 centerScreen;

    //variable that holds value of location of the mouse cursor
    Vector3 mousePos;


    public LayerMask ignoreProjectileLayerObjects;

    Camera mainCamera;

    Vector3 mouseWorldPosition;


    public static Vector3 playerLookVector;


    // Start is called before the first frame update
    void Start()
    {
        
        mainCamera = Camera.main;
    }

    


    // Update is called once per frame
    void Update()
    {

    }

    //Updates after update, used for physics related calculations and functions
    private void FixedUpdate()
    {
        playerPosition = gameObject.transform.position;

        playerPosition = new Vector3(playerPosition.x, 1.0f, playerPosition.z);

        mousePos = Input.mousePosition;

        mousePos.z = Vector3.Distance(mainCamera.transform.position, playerPosition);

        mouseWorldPosition = mainCamera.ScreenToWorldPoint(mousePos);

        mouseWorldPosition.y = playerPosition.y;

        playerLookVector = mouseWorldPosition-playerPosition;


        //set line renderer position to current player location
        lineRenderer.SetPosition(0, playerPosition);

        lineRenderer.SetPosition(1, mouseWorldPosition);

        //set line renderer position to current player location
        lineRenderer.SetPosition(2, playerPosition);

    }

}
