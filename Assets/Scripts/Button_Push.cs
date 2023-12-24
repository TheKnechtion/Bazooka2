using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using System;
using UnityEngine.Events;

public class Button_Push : MonoBehaviour
{
  
    bool canActivate = false;

    Material activatedMat;
    MeshRenderer buttonRenderer;


    [SerializeField] private CinemachineVirtualCamera connectedCamera;
    private CameraSwitcher camManager;
    [SerializeField] private bool usesCamera;


    public static event EventHandler OnPlayerInRange;
    public static event EventHandler OnPlayerOutOfRange;


    public UnityEvent OnActivated;

    public GameObject targetObject;



    private void Awake()                                    
    {
        PlayerManager.OnPlayerActivatePress += Activate;

        activatedMat = (Material)Resources.Load("Activated");
        buttonRenderer = gameObject.GetComponent<MeshRenderer>();



        //uiManager = GameObject.Find("Canvas").GetComponent<UI_Manager>();


        

    }

    //activate the "press space" UI
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            canActivate = true;
            OnPlayerInRange?.Invoke(this, EventArgs.Empty);
        }
    }


    //de-activate the "press space" UI
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

            OnActivated.Invoke();

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
