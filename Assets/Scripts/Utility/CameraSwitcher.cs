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

    [SerializeField] private static List<CinemachineVirtualCamera> sceneCameras = new List<CinemachineVirtualCamera>();

    private CinemachineVirtualCamera currentOBJCamera;

    [SerializeField] private float CameraDuration = 3.5f;
    [SerializeField] private float CamDurationCutscene = 8.0f;

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
    }

    private void changedScene(Scene arg0, Scene arg1)
    {
        //sceneCameras.Clear();
    }

    #region Transition Invokers
    /// <summary>
    /// This transitions camera with cusdtom time for duration
    /// </summary>
    /// <param name="incomingCamera"></param>
    /// <param name="duration"></param>
    public void SwitchToCamera(CinemachineVirtualCamera incomingCamera, float duration)
    {
        OnCameraEnable?.Invoke(this, EventArgs.Empty);
        StartCoroutine(switchCamera(incomingCamera, duration));
    }
    

    /// <summary>
    /// This transitions camera with default time 3.5f duration
    /// </summary>
    /// <param name="incomingCamera"></param>
    public void SwitchToCamera(CinemachineVirtualCamera incomingCamera)
    {
        OnCameraEnable?.Invoke(this, EventArgs.Empty);
        StartCoroutine(switchCamera(incomingCamera));
    }
    #endregion

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
    private IEnumerator switchCamera(CinemachineVirtualCamera cameraToActivate, float duration)
    {
        currentOBJCamera = findCamera(cameraToActivate);
        currentOBJCamera.Priority = activePriority;


        yield return new WaitForSeconds(duration);


        currentOBJCamera.Priority = disablePriority;
        currentOBJCamera = null;
        OnCameraDisable?.Invoke(this, EventArgs.Empty);

        yield return null;
    }
    private CinemachineVirtualCamera findCamera(CinemachineVirtualCamera cameraIWant)
    {
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
