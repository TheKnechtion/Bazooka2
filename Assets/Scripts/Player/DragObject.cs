using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DragObject : MonoBehaviour
{
    float distance;
    PlayerController _playerController;
    PlayerController _playerManagerController;


    bool isDragging = false;
    private void Awake()
    {
        _playerController = new PlayerController();
        //_playerController.PlayerInteract.Activate.performed += ;
    }


    private void OnTriggerStay(Collider other)
    {
        if(_playerController.PlayerInteract.Activate.IsPressed() && other.transform.tag == "Player")
        {

            if(isDragging)
            {
                transform.SetParent(null);
                isDragging = false;
                PlayerManager._playerController.PlayerActions.Enable();
            }
            else
            {
                transform.SetParent(other.transform, true);
                isDragging = true;
                PlayerManager._playerController.PlayerActions.Disable();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        transform.SetParent(null);
        isDragging = false;
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
