using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Elevator : Interactable
{
    enum orientation
    {
        UpToDown,
        DownToUp
    }

    enum position
    {
        Up,
        Down
    }

    [SerializeField] float endHeight;

    [SerializeField] List<float> floorHeights;

    [Tooltip("Choose which direction the elevator moves.")]
    [SerializeField] orientation elevatorMovementDirection;

    [Tooltip("This is the amount of time before the elevator can start descending.")]
    [SerializeField] float waitTime;

    [Tooltip("When selected, the elevator will automatically return to its start position after waitTime seconds.")]
    [SerializeField] bool autoReturnToStartPosition;

    [Tooltip("When unselected, the elevator won't move.")]
    [SerializeField] bool isActive;


    IEnumerator currentCoroutine = null;


    bool canActivate = true;

    float startHeight;
    float bottomHeight;
    float topHeight;

    public bool isElevating = false;
    public bool isGoingDown = false;

    float incrementVectorY = 0f;

    position currnetLocation;




    //[SerializeField] bool startAtEndPosition;


    //public bool objectBeneathElevator;

    bool playerOnObject = false;

    string obj_tag;


    private void Start()
    {
        startHeight = transform.position.y;

        incrementVectorY = startHeight;

        if (elevatorMovementDirection == orientation.DownToUp)
        {
            bottomHeight = startHeight;
            topHeight = endHeight;
            currnetLocation = position.Down;
        }
        else
        {
            bottomHeight = endHeight;
            topHeight = startHeight;
            currnetLocation = position.Up;
        }

        /*
        if (startAtEndPosition)
        {
            transform.position = new Vector3(this.transform.position.x, endHeight, this.transform.position.z);
            //ColliderUtility.DeactivateColliders(colliders);
        }
        */
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag != "Player")
        {
            return;
        }
        else
        {
            playerOnObject = true;
        }


        if (canActivate && isActive)
        {
            //other.transform.GetComponent<Rigidbody>().constraints &= ~RigidbodyConstraints.FreezePositionY;
            //other.transform.SetParent(transform);
            canActivate = false;
        }
        else
        {
            return;
        }


        Move();
    }


    void Move()
    {
        if (elevatorMovementDirection == orientation.DownToUp)
        {
            isElevating = true;
        }
        else
        {
            isGoingDown = true;
        }
    }


    public void GoToFloor(int floorNumber)
    {
        
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            playerOnObject = false;
            //other.transform.SetParent(null);
            //GameObject.DontDestroyOnLoad(other.transform);
        }
    }



    private void FixedUpdate()
    {
        /*
        if (objectBeneathElevator)
        {
            ColliderUtility.DeactivateColliders(colliders);
            collidersActive = false;
        }
        */

        if (isElevating)
        {
            Elevate();
            return;
        }

        if (isGoingDown)
        {
            GoDown();
            return;
        }

        /*
        if (startAtEndPosition && isActive)
        {
            startAtEndPosition = false;
            isGoingDown = true;
            autoReturnToStartPosition = true;

            incrementVectorY = this.transform.position.y;

            GoDown();
        }
        */

    }

    void Elevate()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, incrementVectorY, gameObject.transform.position.z);

        incrementVectorY += 0.05f;

        if (incrementVectorY >= topHeight)
        {
            isElevating = false;
        }
        else
        {

        }

        if (isElevating == false && elevatorMovementDirection == orientation.DownToUp && autoReturnToStartPosition)
        {
            StartCoroutine(WaitToGoDown());
        }
        else
        {
            StartCoroutine(WaitToActivate());
        }

    }


    void GoDown()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, incrementVectorY, gameObject.transform.position.z);

        incrementVectorY -= 0.05f;

        if (incrementVectorY <= bottomHeight)
        {
            /*
            if(!collidersActive)
            {
                ColliderUtility.ActivateColliders(colliders);
                collidersActive = true;
            }
            */

            isGoingDown = false;
        }
        else
        {
            return;
        }

        if (isGoingDown == false && elevatorMovementDirection == orientation.UpToDown && autoReturnToStartPosition)
        {
            StartCoroutine(WaitToGoUp());
        }
        else
        {
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
    private IEnumerator WaitToGoUp()
    {
        yield return new WaitForSeconds(waitTime);
        isElevating = true;
    }
    private IEnumerator WaitToActivate()
    {
        yield return new WaitForSeconds(waitTime);
        canActivate = true;
    }
    void StartCoroutine_Custom(IEnumerator coroutine)
    {
        //CoroutineCheck(currentCoroutine);
        //currentCoroutine = coroutine;

        StartCoroutine(coroutine);
    }


    void CoroutineCheck(IEnumerator coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
    }


    private IEnumerator WaitToDo(bool condition)
    {
        yield return new WaitForSeconds(waitTime);
        condition = true;
        yield return condition;

    }

}
