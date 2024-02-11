using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChooser : MonoBehaviour
{
    //Scriptable object containing NEXT possible scene choices
    public SceneScriptable NextScenes { get; set; }

    //Buttons
    [SerializeField] private GameObject Buttons;

    private void Awake()
    {
        if (Buttons == null)
        {
            Debug.LogWarning("! Level-Select Buttons not set !");
        }
        else
        {
            EnableButton(false);
        }

        SceneManager.activeSceneChanged += SceneChange;
    }

    private void SceneChange(Scene arg0, Scene arg1)
    {
        EnableButton(false);

        //TODO: Grab the NextScenes scriptable
    }

    private void OnEnable()
    {
        if (NextScenes == null)
        {
            Debug.LogWarning("! Next-Scenes options not set !");
        }
        else if (NextScenes.SceneChoices.Length == 1)
        {
            //Load the 1 scene that is set to transition

            ChooseLevel(NextScenes.SceneChoices[0]);
        }
    }

    public void OptionOne()
    {
        ChooseLevel(NextScenes.SceneChoices[0]);
    }

    public void OptionTwo()
    {
        ChooseLevel(NextScenes.SceneChoices[1]);
    }

    private void EnableButton(bool setting)
    {
        Buttons.SetActive(setting);
    }

    private void ChooseLevel(string name)
    {
        if (NextScenes != null)
        {
            //TODO: Load next scene

            SceneManager.LoadScene(name, LoadSceneMode.Single);
        }
    }
}
