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

    [SerializeField] private float X_Offset;
    [SerializeField] private float Y_Offset;
    [SerializeField] private float Z_Offset;

    [SerializeField] private float X_Damping;
    [SerializeField] private float Y_Damping;
    [SerializeField] private float Z_Damping;

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
        cam.AddCinemachineComponent<CinemachineTransposer>();
        cam.AddCinemachineComponent<CinemachineTransposer>().m_BindingMode = CinemachineTransposer.BindingMode.WorldSpace;

        cam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.x = X_Offset;
        cam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.y = Y_Offset;
        cam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset.z = Z_Offset;

        cam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = 0;
        cam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = 0;
        cam.GetCinemachineComponent<CinemachineTransposer>().m_ZDamping = 0;
    }

}
