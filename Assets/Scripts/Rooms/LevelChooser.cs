using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChooser : MonoBehaviour
{
    //Scriptable object containing NEXT possible scene choices
    private SceneScriptable NextScenes;

    //Buttons
    [SerializeField] private GameObject Buttons;
    private bool mouseToggled;

    private void Awake()
    {
        if (Buttons == null)
        {
            Debug.LogWarning("! Level-Select Buttons not set !");
        }

        SceneManager.activeSceneChanged += SceneChange;
        GameManager.OnLevelCompleted += NextSceneSequence;

        RoomSelectScreen.OnRoomSelected += OnRoomSelected;
    }

    private void OnRoomSelected(object sender, int e)
    {
        ChooseOption(e);
    }

    private void Start()
    {
        NextScenes = LevelManager.NextScenes;

        mouseToggled = false;

        if (NextScenes == null)
        {
            Debug.LogWarning("! Next-Scenes options not set !");
        }
    }

    private void OnEnable()
    {
        
    }

    private void Update()
    {
        if (Buttons.activeInHierarchy)
        {
            //if (!mouseToggled)
            //{
            //    ToggleMouse(true, CursorLockMode.Confined);
            //}
        }
    }
    private void NextSceneSequence(object sender, EventArgs e)
    {
        //ToggleButtons(true);
        //ToggleMouse(true, CursorLockMode.Confined);
    }
    private void SceneChange(Scene arg0, Scene arg1)
    {
        //ToggleButtons(false);
        //ToggleMouse(false, CursorLockMode.Locked);

        NextScenes = LevelManager.NextScenes;
    }

    private void ChooseOption(int n)
    {
        ChooseLevel(NextScenes.SceneChoices[0]);
    }

    /*
    private void OptionOne()
    {
        ChooseLevel(NextScenes.SceneChoices[0]);
    }

    private void OptionTwo()
    {
        ChooseLevel(NextScenes.SceneChoices[1]);
    }
    */
    private void ToggleButtons(bool setting)
    {
        Buttons.SetActive(setting);
    }

    private void ToggleMouse(bool setting, CursorLockMode lockState)
    {
        Cursor.visible = setting;
        Cursor.lockState = lockState;
    }


    private void ChooseLevel(string name)
    {
        if (NextScenes != null)
        {
            //TODO: Load next scene

            LevelManager.EnterNewScene(name);

            this.gameObject.SetActive(false);
        }
    }
}
