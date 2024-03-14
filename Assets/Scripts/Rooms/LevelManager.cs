using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class LevelManager 
{
    public static SceneScriptable NextScenes;

    public static event EventHandler OnNewScene;
    public static void AddNextScenes(SceneScriptable next)
    {
        Debug.Log("Next scenes loaded");
        NextScenes = next;
    }

    public static void EnterNewScene(string nextScene)
    {
        Debug.Log("Moving to "+nextScene);

        OnNewScene?.Invoke(null, EventArgs.Empty);

        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }

    //This method will call the first scene.
    //Only called when there is 1 'next' scene.
    public static void MoveToNextScene()
    {
        EnterNewScene(NextScenes.SceneChoices[0]);
    }

    public static int GetHeldSceneCount()
    {
        return NextScenes.SceneChoices.Length;
    }
}
