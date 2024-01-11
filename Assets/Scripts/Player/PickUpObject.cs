using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickUpObject : MonoBehaviour
{

    PlayerController _playerController;
    Collider playerCollider;
    bool canPickUp = false;
    bool isHoldingThisObject = false;
    float dragObjectSpeed = 1.0f;
    [SerializeField] float objSize;

    GameObject playerAttachPoint;
    GameObject objAttachPoint;

    float mass;

    Vector3 localObjAttachPointPosition;
    Rigidbody objectRbCopy;


    private void Awake()
    {
        _playerController = new PlayerController();
        _playerController.PlayerInteract.Activate.performed += HandlePickUp;
        _playerController.PlayerInteract.Activate.canceled -= HandlePickUp;
    }


    // Start is called before the first frame update
    void Start()
    {
        objAttachPoint = this.transform.Find("AttachPoint").transform.gameObject;

        localObjAttachPointPosition = objAttachPoint.transform.localPosition;

        objectRbCopy = this.GetComponent<Rigidbody>();
        mass = objectRbCopy.mass;
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
        if (other.transform.tag == "Player" && other.transform.gameObject.GetComponent<PlayerManager>().CanCarryObjectOnBack)
        {
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
            StopHolding();
        }
    }


    void HandlePickUp(InputAction.CallbackContext e)
    {
        if (canPickUp && isHoldingThisObject)
        {
            StopHolding();
            transform.position = playerAttachPoint.transform.position - playerAttachPoint.transform.forward*objSize;
        }
        else if (canPickUp)
        {
            StartHolding();
        }
    }

    void StartHolding()
    {
        playerAttachPoint = playerCollider.transform.Find("PlayerAttachPoint").gameObject;

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

        objAttachPoint.transform.SetParent(playerCollider.transform,true);

        objAttachPoint.transform.localPosition = playerAttachPoint.transform.localPosition;

        objAttachPoint.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        //transform.localPosition = ;
        isHoldingThisObject = true;
        PlayerMovement.dragObjectSpeed = dragObjectSpeed;
    }

    public void StopHolding()
    {
        playerCollider.transform.GetComponent<PlayerManager>().carriedObject = this.gameObject;

        playerCollider.transform.gameObject.GetComponent<PlayerManager>().isCarryingObjectOnBack = false;

        Rigidbody temp = this.AddComponent<Rigidbody>();

        temp.mass = mass;

        transform.SetParent(null);


        objAttachPoint.transform.SetParent(this.transform,true);

        objAttachPoint.transform.localPosition = localObjAttachPointPosition;


        isHoldingThisObject = false;
        PlayerMovement.dragObjectSpeed = 1.0f;
    }

    private void OnEnable()
    {
        _playerController.PlayerInteract.Enable();
    }
    private void OnDisable()
    {
        _playerController.PlayerInteract.Disable();
    }

}
