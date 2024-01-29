using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Elevator : Interactable
{
    [SerializeField] float yValue;
    [SerializeField] float waitTime;
    [SerializeField] bool goesBackDown;
    [SerializeField] bool isActive;

    float startHeight;
    bool isElevating = false;
    bool canActivate = true;
    bool isGoingDown = false;
    float incrementVectorY = 0f;


    [SerializeField] bool startAtTop;
    bool playerOnObject = false;

    string obj_tag;

    private void OnTriggerEnter(Collider other)
    {
        obj_tag = other.gameObject.tag;

        if (canActivate && obj_tag == "Player" && isActive)
        {
            other.transform.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;

            isElevating = true;
            canActivate = false;
        }

        if(obj_tag == "Player")
        {
            playerOnObject = true;
        }
    }

    
    private void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            playerOnObject = false;
        }
    }
    


    private void Start()
    {
        startHeight = transform.position.y;

        if(startAtTop)
        {
            transform.position = new Vector3(this.transform.position.x, yValue, this.transform.position.z);
            ColliderUtility.DeactivateColliders(colliders);
        }


    }

    private void FixedUpdate()
    {
        if(isElevating)
        {
            Elevate();
        }


        if(isGoingDown && goesBackDown)
        {
            GoDown();
        }


        if (startAtTop && isActive)
        {
            startAtTop = false;
            isGoingDown = true;
            goesBackDown = true;

            incrementVectorY = this.transform.position.y;

            GoDown();
        }

        if (isGoingDown && !playerOnObject && collidersActive)
        {
            ColliderUtility.DeactivateColliders(colliders);
            collidersActive = false;
        }
    }

    void Elevate()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, incrementVectorY, gameObject.transform.position.z);

        incrementVectorY += 0.05f;

        if (incrementVectorY >= yValue)
        {
            isElevating = false;
            StartCoroutine(WaitToGoDown());
        }
    }


    void GoDown()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, incrementVectorY, gameObject.transform.position.z);

        incrementVectorY -= 0.05f;

        if (incrementVectorY <= startHeight)
        {

            if(!collidersActive)
            {
                ColliderUtility.ActivateColliders(colliders);
                collidersActive = true;
            }


            isGoingDown = false;
            StartCoroutine(WaitToActivate());
        }
    }

    public void Activate()
    {
        isActive = true;
    }

    public void Deactivate()
    {
        isActive = false;
    }


    private IEnumerator WaitToGoDown()
    {
        yield return new WaitForSeconds(waitTime);
        isGoingDown = true;
    }

    private IEnumerator WaitToActivate()
    {
        yield return new WaitForSeconds(waitTime);
        canActivate = true;
    }

}
