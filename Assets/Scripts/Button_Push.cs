using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using System;

public class Button_Push : MonoBehaviour
{
    [SerializeField] GameObject activatedObject;


    public float distanceFromPlayer;
    bool canActivate = false;

    Material activatedMat;
    MeshRenderer buttonRenderer;


    [SerializeField] private CinemachineVirtualCamera connectedCamera;
    private CameraSwitcher camManager;
    [SerializeField] private bool usesCamera;


    public static event EventHandler OnPlayerInRange;
    public static event EventHandler OnPlayerOutOfRange;

    UI_Manager uiManager;


    private void Awake()                                    
    {
        PlayerManager.OnPlayerActivatePress += Activate;

        activatedMat = (Material)Resources.Load("Activated");
        buttonRenderer = gameObject.GetComponent<MeshRenderer>();

        OnPlayerOutOfRange?.Invoke(this, EventArgs.Empty);

        //uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();

    }


    // Update is called once per frame
    void Update()
    {
        distanceFromPlayer = (gameObject.transform.position - PlayerInfo.instance.playerPosition).magnitude;

        /*
        if (distanceFromPlayer <= 3.0f)
        {
            canActivate = true;
            OnPlayerInRange?.Invoke(this, EventArgs.Empty);
        }
        else
        {
            canActivate = false;
            OnPlayerInRange?.Invoke(this, EventArgs.Empty);
        }
        */


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            canActivate = true;
            OnPlayerInRange?.Invoke(this, EventArgs.Empty);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            canActivate = false;
            OnPlayerOutOfRange?.Invoke(this, EventArgs.Empty);
        }
    }


    public void Activate(object sender, System.EventArgs e)
    {
        if(canActivate)
        {
            buttonRenderer.material = activatedMat;

            activatedObject.GetComponent<Act>().Activate();

            CheckForVirtualCamera();
            DeleteOnActivate();
        }
    }

    void CheckForVirtualCamera()
    {
        /*
        if (connectedCamera)
        {
            camManager = GameObject.Find("CameraManager").GetComponent<CameraSwitcher>();
            usesCamera = true;
            //camManager.SwitchToCamera(connectedCamera);
        }
        else { usesCamera = false; }
        */
    }

    void DeleteOnActivate()
    {
        OnPlayerOutOfRange?.Invoke(this, EventArgs.Empty);
        PlayerManager.OnPlayerActivatePress -= Activate;
        Destroy(this);
    }




}
