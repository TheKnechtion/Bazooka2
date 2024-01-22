using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpObject : MonoBehaviour
{

    PlayerController _playerControllerRef;
    Collider playerCollider;
    bool canPickUp = false;
    bool isHoldingThisObject = false;
    float dragObjectSpeed = 1.0f;
    [SerializeField] float objSize;
    [SerializeField] private string objName;

    [SerializeField] private Collider[] colliders;
    //private Collider[] colliders;

    GameObject playerAttachPoint;
    GameObject objAttachPoint;

    float mass;

    Vector3 localObjAttachPointPosition;
    Rigidbody objectRbCopy;


    private void Awake()
    {
        _playerControllerRef = new PlayerController();
        _playerControllerRef.PlayerInteract.Activate.performed += HandlePickUp;
        _playerControllerRef.PlayerInteract.Activate.canceled -= HandlePickUp;
        GameManager.OnSceneChange += DestroyThis;
    }


    // Start is called before the first frame update
    void Start()
    {
        objAttachPoint = this.transform.Find("AttachPoint").transform.gameObject;

        localObjAttachPointPosition = objAttachPoint.transform.localPosition;

        objectRbCopy = this.GetComponent<Rigidbody>();
        mass = objectRbCopy.mass;

        //colliders = this.gameObject.GetComponents<Collider>();

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            UI_Manager.Show_InteractUI($"Pick Up {objName}");
        }
    }



    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player" && other.transform.gameObject.GetComponent<PlayerManager>().CanCarryObjectOnBack)
        {
            UI_Manager.Show_InteractUI($"Pick Up {objName}");
            other.transform.gameObject.GetComponent<PlayerManager>().CanCarryObjectOnBack = false;
            canPickUp = true;
            playerCollider = other;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Player")
        {
            other.transform.gameObject.GetComponent<PlayerManager>().CanCarryObjectOnBack = true;
            canPickUp = false;
            isHoldingThisObject = false;
            //StopHolding();
            UI_Manager.StopShow_InteractUI();
        }
    }


    void HandlePickUp(InputAction.CallbackContext e)
    {
        if (canPickUp && isHoldingThisObject)
        {
            StopHolding();
            transform.position = playerAttachPoint.transform.position - playerAttachPoint.transform.up*objSize;
            UI_Manager.Show_InteractUI($"Pick Up {objName}");
        }
        else if (canPickUp)
        {
            StartHolding();
            UI_Manager.StopShow_InteractUI();
        }
    }

    Transform playerSpine;
    Transform playerDetachPoint;
    void StartHolding()
    {
        DeactivateColliders();

        //Debug.Log(playerCollider.transform.Find("Bip001").Find("Bip001 Pelvis").Find("Bip001 Spine").Find("PlayerAttachPoint").name);

        playerSpine = playerCollider.transform.Find("Bip001").Find("Bip001 Pelvis").Find("Bip001 Spine");

        playerAttachPoint = playerSpine.transform.Find("PlayerAttachPoint").gameObject;

        playerCollider.transform.GetComponent<PlayerManager>().carriedObject = this.gameObject;

        playerCollider.transform.gameObject.GetComponent<PlayerManager>().isCarryingObjectOnBack = true;

        /*
        foreach(BoxCollider bc in this.GetComponents<BoxCollider>())
        {
            bc.isTrigger = true;
            
        }
        */

        Destroy(this.GetComponent<Rigidbody>());

        objAttachPoint.transform.SetParent(null);

        this.transform.SetParent(objAttachPoint.transform,true);

        //playerAttachPoint.transform.rotation = Quaternion.Euler();

        objAttachPoint.transform.SetParent(playerSpine,true);

        objAttachPoint.transform.localPosition = playerAttachPoint.transform.localPosition;

        objAttachPoint.transform.localRotation = Quaternion.Euler(-90f, 0f, 90f);

        //transform.localPosition = ;
        isHoldingThisObject = true;
        PlayerMovement.dragObjectSpeed = dragObjectSpeed;


    }

    public void StopHolding()
    {


        playerCollider.transform.GetComponent<PlayerManager>().carriedObject = this.gameObject;


        playerCollider.transform.gameObject.GetComponent<PlayerManager>().isCarryingObjectOnBack = false;

        transform.localPosition = localObjAttachPointPosition;

        objAttachPoint.transform.SetParent(null);

        transform.SetParent(null);

        objAttachPoint.transform.SetParent(this.transform,true);

        objAttachPoint.transform.localPosition = localObjAttachPointPosition;




        Rigidbody temp = this.AddComponent<Rigidbody>();

        temp.mass = mass;


        isHoldingThisObject = false;
        PlayerMovement.dragObjectSpeed = 1.0f;


        ActivateColliders();
    }

    public void DeactivateColliders()
    {
        foreach(var collider in colliders)
        {
            collider.enabled = false;
        }
    }

    public void ActivateColliders()
    {
        foreach (var collider in colliders)
        {
            collider.enabled = true;
        }
    }


    void DestroyThis(object sender, EventArgs e)
    {
        GameManager.OnSceneChange -= DestroyThis;

        if(isHoldingThisObject)
        {
            Destroy(this.gameObject.transform.parent.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }


    private void OnEnable()
    {
        _playerControllerRef.PlayerInteract.Enable();
    }
    private void OnDisable()
    {
        _playerControllerRef.PlayerInteract.Disable();
    }

}
