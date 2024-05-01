using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using System;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class PauseManager : MonoBehaviour
{

    public static event EventHandler OnPause;

    [SerializeField] GameObject PauseMenu;

    PlayerController _playerController;

    bool GamePaused = false;


    void Awake()
    {
        _playerController = new PlayerController();

        _playerController.PlayerMenuActions.Pause.performed += PauseGame;
        _playerController.PlayerMenuActions.Pause.canceled -= PauseGame;


    }


    public void ScrollSelection(InputAction.CallbackContext e)
    {

    }


        public void PauseGame(InputAction.CallbackContext e)
    {
        if (!GamePaused)
        {
            PauseMenu.SetActive(true);

            Time.timeScale = 0.0f;
            GamePaused = true;
            PlayerManager._playerController.PlayerActions.Disable();
        }
        else
        {
            PauseMenu.SetActive(false);

            Time.timeScale = 1.0f;
            GamePaused = false;
            PlayerManager._playerController.PlayerActions.Enable();
        }
        OnPause?.Invoke(this, EventArgs.Empty);
    }

    public void ResumeGame()
    {
        Time.timeScale = 1.0f;
        GamePaused = false;
        PlayerManager._playerController.PlayerActions.Enable();

        EventSystem.current.SetSelectedGameObject(null);

        PauseMenu.SetActive(false);

        OnPause?.Invoke(this, EventArgs.Empty);
    }


    public void QuitGame()
    {
        ResumeGame();

        GameObject.Find("MainCanvas").transform.Find("Replay").GetComponent<MenuStartGame>().DestroyCurrentUI();

        SceneManager.LoadScene(0);
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
