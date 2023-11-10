using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddToManager : MonoBehaviour
{
    //This script is for adding cameras on scene change into the cameraManger script

    private void Awake()
    {
        CameraSwitcher.AddCamera(GetComponent<CinemachineVirtualCamera>());
    }
    void Start()
    {
      //  CameraSwitcher.AddCamera(GetComponent<CinemachineVirtualCamera>());
    }
}
