using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PauseManager : MonoBehaviour
{

    public static event EventHandler OnPause;


    PlayerController _playerController;

    bool GamePaused = false;


    void Awake()
    {
        _playerController = new PlayerController();

        _playerController.PlayerMenuActions.Pause.performed += PauseGame;
        _playerController.PlayerMenuActions.Pause.canceled -= PauseGame;
    }



    public void PauseGame(InputAction.CallbackContext e)
    {
        if (!GamePaused)
        {
            Time.timeScale = 0.0f;
            GamePaused = true;
            PlayerManager._playerController.PlayerActions.Disable();
        }
        else
        {
            Time.timeScale = 1.0f;
            GamePaused = false;
            PlayerManager._playerController.PlayerActions.Enable();
        }
        OnPause?.Invoke(this, EventArgs.Empty);
    }


    private void OnEnable()
    {
        //begins player movement functions
        _playerController.PlayerMenuActions.Enable();
    }


    private void OnDisable()
    {
        //ends player movement functions
        _playerController.PlayerMenuActions.Disable();
    }



}
