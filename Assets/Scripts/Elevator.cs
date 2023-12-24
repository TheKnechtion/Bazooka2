using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : MonoBehaviour
{
    [SerializeField] float yValue;
    [SerializeField] float waitTime;
    [SerializeField] bool goesBackDown;

    float startHeight;
    bool isElevating = false;
    bool canActivate = true;
    bool isGoingDown = false;
    float incrementVectorY = 0f;


    private void OnTriggerEnter(Collider other)
    {
        if(canActivate && other.transform.tag == "Player")
        {
            isElevating = true;
            canActivate = false;
        }
    }

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
