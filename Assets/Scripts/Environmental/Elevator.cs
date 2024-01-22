using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
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
    private void OnTriggerEnter(Collider other)
    {
        if (canActivate && other.transform.tag == "Player" && isActive)
        {
            other.transform.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;

            isElevating = true;
            canActivate = false;
        }
    }

    /*
    private void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            other.transform.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;
        }
    }
    */


    private void Start()
    {
        startHeight = transform.position.y;
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
