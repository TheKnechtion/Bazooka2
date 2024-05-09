using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.Events;
using UnityEngine.Playables;

public class CamCutscene : MonoBehaviour
{

    //New addition (for camera)
    [SerializeField] private CinemachineVirtualCamera connectedCamera;
    private CameraSwitcher camManager;
    [SerializeField] private bool usesCamera;

    [SerializeField] private float CameraDuration = 3.5f;

    [SerializeField] private bool StopsUI = true;

    private void Awake()
    {
      
        if (connectedCamera)
        {
            camManager = GameObject.Find("CameraManager").GetComponent<CameraSwitcher>();
            usesCamera = true;
        }
        else { usesCamera = false; }
    }


    public void Activate()
    {
        if (usesCamera)
            camManager.SwitchToCamera(connectedCamera, CameraDuration, StopsUI);
    }
        
}
