using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChooser : MonoBehaviour
{
    //Scriptable object containing NEXT possible scene choices
    private SceneScriptable NextScenes;

    [SerializeField] private FadeScreen fadeScreen;

    //Buttons
    //[SerializeField] private GameObject Buttons;
    private bool mouseToggled;

    private bool canTransition;

    private void Awake()
    {
        //if (Buttons == null)
        //{
        //    Debug.LogWarning("! Level-Select Buttons not set !");
        //}

        canTransition = false;

        SceneManager.activeSceneChanged += SceneChange;
        GameManager.OnLevelCompleted += NextSceneSequence;

        RoomSelectScreen.OnRoomSelected += OnRoomSelected;

        HelicopterEvac.MoveToNext += EvacSingleNext;
        TransitionObject.OnEndReached += EvacSingleNext;

        fadeScreen.OnFadedFull += OnFadeFull;
    }

    private void EvacSingleNext(object sender, EventArgs e)
    {
        fadeScreen.screenAnimator.SetBool("ScreenFade", true);
        StartCoroutine(TransitionOnFade(NextScenes.SceneChoices[0]));
    }

    private void OnFadeFull(object sender, EventArgs e)
    {
        canTransition = true;
    }

    private void OnRoomSelected(object sender, int e)
    {
        fadeScreen.screenAnimator.SetBool("ScreenFade", true);
        StartCoroutine(TransitionOnFade(NextScenes.SceneChoices[e]));
    }

    private void Start()
    {
        NextScenes = LevelManager.NextScenes;

        mouseToggled = false;

        if (NextScenes == null)
        {
            Debug.LogWarning("! Next-Scenes options not set !");
        }

        SceneManager.activeSceneChanged += SceneChanged;
    }

    private void SceneChanged(Scene arg0, Scene arg1)
    {
        canTransition = false;
    }

    private void NextSceneSequence(object sender, EventArgs e)
    {
        //ToggleButtons(true);
        //ToggleMouse(true, CursorLockMode.Confined);
    }
    private void SceneChange(Scene arg0, Scene arg1)
    {
        NextScenes = LevelManager.NextScenes;
    }
    private IEnumerator TransitionOnFade(string name)
    {
        while (!canTransition)
        {
            yield return null;
        }

        ChooseLevel(name);

        while (canTransition)
        {
            yield return null;
        }

        fadeScreen.screenAnimator.SetBool("ScreenFade", false);
    }
    private void ChooseLevel(string name)
    {
        if (NextScenes != null)
        {
            //TODO: Load next scene

            LevelManager.EnterNewScene(name);
        }
    }
}
