using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.InputSystem;

public class DragObject : MonoBehaviour
{

    //[SerializeField] PhysicMaterial frictionless;
    //[SerializeField] Collider physicsCollider;
    [SerializeField] float dragObjectSpeed;
    [SerializeField] private string objName;

    float distance;
    PlayerController _playerControllerRef;
    PlayerController _playerManagerController;

    BoxCollider object_rb;



    bool isDragging = false;
    private void Awake()
    {
        _playerControllerRef = new PlayerController();
        _playerControllerRef.PlayerInteract.Activate.performed += HandleDrag;
        _playerControllerRef.PlayerInteract.Activate.canceled -= HandleDrag;

        GameManager.OnSceneChange += CleanUpDragObjects;

    }

    bool canDrag = false;
    bool isBeingUsed = false;

    Collider playerCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            UI_Manager.Show_InteractUI($"Drag {objName}");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == "Player" && !isDragging && !isBeingUsed)
        {
            if(!other.transform.GetComponent<PlayerManager>().CheckPlayerBack())
            {
                canDrag = true;
                playerCollider = other;
                UI_Manager.Show_InteractUI($"Drag {objName}");
            }

        }



    }

    void CleanUpDragObjects(object sender, System.EventArgs e)
    {
        Destroy(gameObject);
    }


    void HandleDrag(InputAction.CallbackContext e)
    {
        if (canDrag && isDragging)
        {
            StopDragging();
        }
        else if (canDrag)
        {
            StartDragging(playerCollider);
        }
    }




    private void OnTriggerExit(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            canDrag = false;
            isDragging = false;
            StopDragging();
            UI_Manager.StopShow_InteractUI();
        }
    }

    void StartDragging(Collider dragObject)
    {
        UI_Manager.StopShow_InteractUI();
        transform.SetParent(dragObject.transform, true);
        isDragging = true;
        PlayerManager._playerController.PlayerActions.Disable();
        PlayerMovement.dragObjectSpeed = dragObjectSpeed;
    }

    void StopDragging()
    {
        transform.SetParent(null);
        isDragging = false;
        PlayerMovement.dragObjectSpeed = 1.0f;
        PlayerManager._playerController.PlayerActions.Enable();
    }

    public void UseObject()
    {
        StopDragging();
        isBeingUsed = true;
        UI_Manager.StopShow_InteractUI();
        Destroy(this);
    }




    private void OnEnable()
    {
        _playerControllerRef.PlayerInteract.Enable();
    }
    private void OnDisable()
    {
        _playerControllerRef.PlayerInteract.Disable();
    }




}
