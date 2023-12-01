using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetFollowTarget : MonoBehaviour
{
    //This will be the players transform. We can refernce this in case we make the camera 
    //focus on something else for a little, then head back to the player.
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

        target = playerObj.transform;
        setTarget();

        SetCameraBodyDamping();
    }

    private void setTarget()
    {
        cam.Follow = target;
        cam.LookAt = target;
    }


    private void SetCameraBodyDamping()
    {
        cam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0;
        cam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0;
        cam.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0;
    }

}
