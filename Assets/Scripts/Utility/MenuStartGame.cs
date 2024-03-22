using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuStartGame : MonoBehaviour
{
    public static event EventHandler OnRestart;
    public void StartGame()
    {
        DestroyCurrentUI();
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Application.Quit();
    }


    public void DestroyCurrentUI()
    {
        GameObject f = GameObject.Find("GameManager");
        Destroy(f);
        GameObject a = GameObject.Find("Evac_Exit");
        Destroy(a);
        GameObject b = GameObject.Find("Player");
        Destroy(b);
        GameObject v = GameObject.Find("Main Camera");
        Destroy(v);
        GameObject w = GameObject.Find("Follow Camera");
        Destroy(w);
        GameObject e = GameObject.Find("Canvas");
        Destroy(e);
        GameObject d = GameObject.Find("DecalPool");
        Destroy(d);

    }

    public void RestartGame()
    {

        DestroyCurrentUI();

        //We restart fully from the current scene
        OnRestart?.Invoke(this, EventArgs.Empty);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentSceneIndex, LoadSceneMode.Single);
    }

    public void CheckpointRestart()
    {
        if (PlayerInfo.instance != null)
        {
            PlayerInfo.instance.CheckpointRespawn();
        }
    }
}
