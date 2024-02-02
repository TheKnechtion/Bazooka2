using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class PauseManager : MonoBehaviour
{

    public static event EventHandler OnPause;
    public static event EventHandler OnPlay;

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
        Handle_PauseGame();
    }

    public void Handle_PauseGame()
    {
        if (!GamePaused)
        {
            Time.timeScale = 0.0f;
            PlayerManager._playerController.PlayerActions.Disable();
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            GamePaused = true;
            UI_Manager.Show_PauseMenu();
            
            //OnPause?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            Time.timeScale = 1.0f;
            PlayerManager._playerController.PlayerActions.Enable();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
            GamePaused = false;
            UI_Manager.StopShow_PauseMenu();

            //OnPlay?.Invoke(this, EventArgs.Empty);
        }
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
