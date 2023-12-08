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

        _playerController.PlayerActions.Pause.performed += PauseGame;
    }



    public void PauseGame(InputAction.CallbackContext e)
    {
        if (!GamePaused)
        {
            Time.timeScale = 0.0f;
            GamePaused = true;
        }
        else
        {
            Time.timeScale = 1.0f;
            GamePaused = false;
        }
        OnPause?.Invoke(this, EventArgs.Empty);
    }


    private void OnEnable()
    {
        //begins player movement functions
        _playerController.PlayerActions.Enable();
    }


    private void OnDisable()
    {
        //ends player movement functions
        _playerController.PlayerActions.Disable();
    }



}
