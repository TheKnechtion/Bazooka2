using System;
using System.Collections;
using System.Collections.Generic;
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

        SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
    }
}
