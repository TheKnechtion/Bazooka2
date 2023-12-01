using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.PlayerLoop;
using UnityEngine.InputSystem;

public class AimCursor : MonoBehaviour
{
    Vector2 mouseDelta;
    PlayerController _playerController;

    public float cursorSensitivity;

    public Vector3 cursorLocation;

    static Vector3 screenCenter;

    public Vector3 location;

    GameObject playerObj;
    public GameObject centerObj;


    LineRenderer lineRenderer;

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        _playerController = new PlayerController();

        PlayerManager.OnPlayerSpawn += InitializeCursorControls;

        lineRenderer = this.GetComponent<LineRenderer>();
        
    }

    Camera cam;

    private void Start()
    {

        playerObj = GameObject.Find("Player");

        cam = Camera.main;
    }


    void InitializeCursorControls(object sender, EventArgs e)
    {
        _playerController.PlayerActions.MousePosition.performed += mouseMove => mouseDelta = mouseMove.ReadValue<Vector2>();     
        _playerController.PlayerActions.MousePosition.canceled += mouseMove => mouseDelta = Vector2.zero;

    }

    Event currentEvent;

    public static Vector3 cursorVector;

    Vector2 mousePos;
    Vector2 screenCen;


    private void FixedUpdate()
    {

        
        gameObject.GetComponent<RectTransform>().localPosition += new Vector3(mouseDelta.x, mouseDelta.y, 0) * cursorSensitivity;

        screenCen.x = Screen.width / 2;
        screenCen.y = Screen.height / 2;

        screenCenter = centerObj.transform.position;

        cursorLocation = gameObject.GetComponent<RectTransform>().position;




        //Debug.Log(mouseDelta);

        cursorVector = cursorLocation - screenCenter;
    }

    private void LateUpdate()
    {

        lineRenderer.SetPosition(0, screenCenter);
        lineRenderer.SetPosition(1, cursorLocation);


    }




    Vector3 mousePosition;





    private void OnEnable()
    {
        _playerController.PlayerActions.Enable();
        _playerController.PlayerMovement.Enable();
    }

    private void OnDisable()
    {
        _playerController.PlayerActions.Disable();
        _playerController.PlayerMovement.Disable();
    }


}
