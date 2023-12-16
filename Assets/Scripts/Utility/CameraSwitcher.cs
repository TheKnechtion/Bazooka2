using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraSwitcher : MonoBehaviour
{
    /*
     * Lets grab all scene-specific cameras
     * Add these cameras to an array for iterating.
     * 
     * When we want to switch to camera:
     *      - find it in array
     *      - set high priority for a few seconds
     *          - raise event to make [Player invincible |or| disable enemies and Player movement]
     *      - return to lower priotirty
     *          - raise event to return to playing state
     */


    //[SerializeField] private CinemachineVirtualCamera[] sceneCameras;
    [SerializeField] private static List<CinemachineVirtualCamera> sceneCameras = new List<CinemachineVirtualCamera>();

    private CinemachineVirtualCamera currentOBJCamera;

    [SerializeField] private float CameraDuration = 3.5f;

    private int disablePriority, activePriority;

    public static event EventHandler OnCameraEnable;
    public static event EventHandler OnCameraDisable;


    

    void Start()
    {
        currentOBJCamera = null;
        disablePriority = 5;
        activePriority = 11;

        SceneManager.activeSceneChanged += changedScene;

        //We gather all cameras in scene and set priority
        foreach (CinemachineVirtualCamera cam in sceneCameras)
        {
            cam.Priority = disablePriority;
        }

        //Array
        //for (int i = 0; i < sceneCameras.Length; i++)
        //{
        //    sceneCameras[i].Priority = disablePriority;
        //}   

        //Debug.Log("My camera count " + sceneCameras.Count);
    }

    private void changedScene(Scene arg0, Scene arg1)
    {
        //sceneCameras.Clear();
    }

    public void SwitchToCamera(CinemachineVirtualCamera incomingCamera)
    {
        OnCameraEnable?.Invoke(this, EventArgs.Empty);
        StartCoroutine(switchCamera(incomingCamera));
    }

    private IEnumerator switchCamera(CinemachineVirtualCamera cameraToActivate) 
    {
        currentOBJCamera = findCamera(cameraToActivate);
        currentOBJCamera.Priority = activePriority;

        yield return new WaitForSeconds(CameraDuration);
        currentOBJCamera.Priority = disablePriority;
        currentOBJCamera = null;
        OnCameraDisable?.Invoke(this, EventArgs.Empty);

        yield return null;
    }

    private CinemachineVirtualCamera findCamera(CinemachineVirtualCamera cameraIWant)
    {
        //Array
        //for (int i = 0; i < sceneCameras.Length; i++)
        //{
        //    if (ReferenceEquals(cameraIWant, sceneCameras[i]))
        //    {
        //        return sceneCameras[i];
        //    }
        //}

        //List
        foreach (CinemachineVirtualCamera cam in sceneCameras)
        {
            if (ReferenceEquals(cameraIWant, cam))
            {
                return cam;
            }
        }
        
        return null;
    }

    public static void AddCamera(CinemachineVirtualCamera cameraToAdd)
    {
        sceneCameras.Add(cameraToAdd);   
    }
}
