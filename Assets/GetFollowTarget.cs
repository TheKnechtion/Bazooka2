using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFollowTarget : MonoBehaviour
{
    Transform target;
    GameObject playerObj;
    CinemachineVirtualCamera cam;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        cam = GetComponent<CinemachineVirtualCamera>();
        playerObj = GameObject.Find("Player");
        setTarget();
    }

    private void setTarget()
    {
        cam.Follow = playerObj.transform;
        cam.LookAt = playerObj.transform;
    }
}
