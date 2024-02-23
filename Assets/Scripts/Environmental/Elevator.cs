using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
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


    [Tooltip("When selected, the elevator will activate when stepped on by the player.")]
    [SerializeField] bool isActivatedWhenSteppedOn;

    [Tooltip("The height the elevator will end at (only used for StepOn Elevators)")]
    [SerializeField] float endHeight;

    public List<float> floorHeights;
    public List<bool> activeHeights;




    [Tooltip("Choose which direction the elevator moves.")]
    [SerializeField] orientation elevatorMovementDirection;

    [Tooltip("This is the amount of time before the elevator can start descending.")]
    [SerializeField] float waitTime;

    [Tooltip("When selected, the elevator will automatically return to its start position after waitTime seconds.")]
    [SerializeField] bool autoReturnToStartPosition;

    [Tooltip("When unselected, the elevator won't move.")]
    [SerializeField] bool isActive;

    [Tooltip("This is the initial floor the elevator is on.")]
    [SerializeField] int initialFloor;

    [Tooltip("This is the initial floor the elevator will go to when activated.")]
    [SerializeField] int initialTargetFloor;

    IEnumerator currentCoroutine = null;


    bool canActivate = true;

    float elevatorHeight = 0f;
    float bottomHeight;
    float topHeight;



    bool isElevating = false;
    bool isGoingDown = false;


    position currnetLocation;

    public int targetFloor { get; set; }
    int currentFloor = 1;

    int previousFloor;


    public int topFloor { get; set; }

    public int bottomFloor { get; set; }

    bool isReturningToPreviousFloor = false;

    GameObject currentPlayer;

    public void TargetFloor(int floor)
    {
        targetFloor = floor;

        if(targetFloor != currentFloor)
        {
            previousFloor = currentFloor;
        }
    }

    public void ActivateTargetFloor(int floor)
    {
        activeHeights[floor - 1] = true; 
    }

    public void DeactivateTargetFloor(int floor)
    {
        activeHeights[floor - 1] = false;
    }

    public void ReturnToPreviousFloor()
    {
        if(isReturningToPreviousFloor)
        {
            return;
        }

        if (!activeHeights[previousFloor - 1])
        {
            return;
        }

        targetFloor = previousFloor;
        previousFloor = currentFloor;
        isReturningToPreviousFloor = true;
    }


    //[SerializeField] bool startAtEndPosition;


    //public bool objectBeneathElevator;

    bool playerOnObject = false;

    string obj_tag;


    private void Start()
    {
        elevatorHeight = transform.position.y;

        if (elevatorMovementDirection == orientation.DownToUp)
        {
            bottomHeight = elevatorHeight;
            topHeight = endHeight;
            currnetLocation = position.Down;
        }
        else
        {
            bottomHeight = endHeight;
            topHeight = elevatorHeight;
            currnetLocation = position.Up;
        }

        if(initialFloor != 0)
        {
            currentFloor = initialFloor;
            targetFloor = currentFloor;
        }
        else
        {
            targetFloor = 1;
        }

        previousFloor = initialTargetFloor;

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
            currentPlayer = other.gameObject;
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

        if(isActivatedWhenSteppedOn)
        {
            Move();
        }

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



    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            //currentPlayer = null;
            playerOnObject = false;
            //other.transform.SetParent(null);
            //GameObject.DontDestroyOnLoad(other.transform);
        }
    }

    Vector3 playerPosition;

    private void FixedUpdate()
    {
        /*
        if (objectBeneathElevator)
        {
            ColliderUtility.DeactivateColliders(colliders);
            collidersActive = false;
        }
        */



        if(!isActive)
        {
            return;
        }

        /*
        if (playerOnObject)
        {
            playerPosition = currentPlayer.transform.position;
            currentPlayer.transform.position = new Vector3(playerPosition.x,this.transform.position.y+0.05f, playerPosition.z);
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

        if(targetFloor == currentFloor)
        {
            return;
        }

        if (activeHeights[targetFloor - 1])
        {
            GoToFloor();
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
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, elevatorHeight, gameObject.transform.position.z);

        elevatorHeight += 0.05f;

        if (elevatorHeight >= topHeight)
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


    public void GoToFloor()
    {

        if (this.transform.position.y - floorHeights[targetFloor-1] > 0)
        {
            GoDown_Button();
        }
        else if(this.transform.position.y - floorHeights[targetFloor - 1] < 0)
        {
            GoUp_Button();
        }


    }

    void GoDown_Button()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, elevatorHeight, gameObject.transform.position.z);

        elevatorHeight -= 0.05f;


        if (elevatorHeight <= floorHeights[targetFloor - 1])
        {
            currentFloor = targetFloor;
            isReturningToPreviousFloor = false;
        }
        else
        {

        }

    }


    void GoUp_Button()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, elevatorHeight, gameObject.transform.position.z);

        elevatorHeight += 0.05f;

        if (elevatorHeight >= floorHeights[targetFloor - 1])
        {
            currentFloor = targetFloor;
            isReturningToPreviousFloor = false;
        }
        else
        {

        }
    }

    void GoDown()
    {
        gameObject.transform.position = new Vector3(gameObject.transform.position.x, elevatorHeight, gameObject.transform.position.z);

        elevatorHeight -= 0.05f;

        if (elevatorHeight <= bottomHeight)
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

    public void ActivateFloor(int floorToActivate)
    {
        activeHeights[floorToActivate - 1] = true;
    }

    public void DeactivateFloor(int floorToDeactivate)
    {
        activeHeights[floorToDeactivate - 1] = false;
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




}
