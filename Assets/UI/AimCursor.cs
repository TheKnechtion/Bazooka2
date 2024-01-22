using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System;
using UnityEngine.PlayerLoop;
using UnityEngine.InputSystem;
using static Cinemachine.CinemachineTargetGroup;
using UnityEngine.UI;
using UnityEngine.Rendering;

public class AimCursor : MonoBehaviour
{
    Vector2 mouseDelta;

    public float cursorSensitivity;

    public static Vector3 cursorLocation;
    public static Vector3 cursorForward;

    public Vector3 cursorLocationView;
    public Vector3 cursorForwardView;


    public Vector3 location;

    public GameObject centerObj;

    public static Image cursorImage;

    public static Vector3 cursorVector;

    Camera cam;


    Color darkGreen = new Color(.03f, 0.69f, 0.0f);
    Color lightGreen = new Color(.00f, 1.00f, 0.0f);

    Color darkRed = new Color(0.77f, 0.00f, 0.00f);
    Color lightRed = new Color(1.00f, 0.42f, 0.42f);

    bool isAiming;

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        PlayerManager.OnPlayerAim += StartAiming;
        PlayerManager.OnPlayerStopAim += StopAiming;

        PauseManager.OnPause += MakeCursorVisisble;


        cursorImage = gameObject.GetComponent<Image>();
        tempColor = Color.white;
        tempColor.a = 0;
        cursorImage.color = tempColor;
    }

    private void Start()
    {
        cam = Camera.main;
    }

    Vector3 mouseMovementVector;
    Vector3 mousePosition;
    Vector3 mouseMovementInWorldSpaceVector;

    private void Update()
    {
        mouseDelta = PlayerManager.mouseDelta;

        mouseMovementVector = new Vector3(mouseDelta.x, mouseDelta.y, 0) * cursorSensitivity;

        mousePosition = gameObject.GetComponent<RectTransform>().localPosition;

        mousePosition = mousePosition + mouseMovementVector;

        mousePosition.x = Mathf.Clamp(mousePosition.x, -960, 960);

        mousePosition.y = Mathf.Clamp(mousePosition.y, -540, 540);

        gameObject.GetComponent<RectTransform>().localPosition = mousePosition;



        cursorLocation = gameObject.transform.position;

        cursorVector = cursorLocation - cam.transform.position;

        cursorImage.color = lightGreen;
    }


    private void FixedUpdate()
    {

    }

    Color tempColor;

    void StartAiming(object sender, EventArgs e)
    {
        isAiming = true;
    }
    void StopAiming(object sender, EventArgs e)
    {
        tempColor = Color.white;
        tempColor.a = 0;
        cursorImage.color = tempColor;
        isAiming = false;
    }


    bool isPaused = false;
    public void MakeCursorVisisble(object sender, EventArgs e)
    {
        if(!isPaused)
        {
            tempColor = lightGreen;
            tempColor.a = 1;

            isPaused = true;
        }
        else
        {
            tempColor = Color.white;
            tempColor.a = 0;
            cursorImage.color = tempColor;
            isPaused = false;
        }


    }



}
