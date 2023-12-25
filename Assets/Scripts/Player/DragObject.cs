using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragObject : MonoBehaviour
{

    //[SerializeField] PhysicMaterial frictionless;
    //[SerializeField] Collider physicsCollider;
    [SerializeField] float dragObjectSpeed;


    float distance;
    PlayerController _playerController;
    PlayerController _playerManagerController;

    BoxCollider object_rb;




    bool isDragging = false;
    private void Awake()
    {
        _playerController = new PlayerController();
        _playerController.PlayerInteract.Activate.performed += HandleDrag;
        _playerController.PlayerInteract.Activate.canceled -= HandleDrag;

        GameManager.OnSceneChange += CleanUpDragObjects;

    }

    bool canDrag = false;

    Collider playerCollider;

    private void OnTriggerStay(Collider other)
    {
        if(other.transform.tag == "Player")
        {
            canDrag = true;
            playerCollider = other;
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
        }
    }

    void StartDragging(Collider dragObject)
    {
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



    private void OnEnable()
    {
        _playerController.PlayerInteract.Enable();
    }
    private void OnDisable()
    {
        _playerController.PlayerInteract.Disable();
    }




}
