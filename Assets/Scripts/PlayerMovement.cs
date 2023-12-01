using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System;

public class PlayerMovement : MonoBehaviour
{

    Vector2 moveInput;
    PlayerController _playerController;
    float speed;

    bool dash = false;

    float dashCooldown;

    Animator movementAnimator;
    public static Vector3 playerMovement;

    public static Vector3 currentPosition;

    public static event EventHandler OnPlayerMoved;


    Rigidbody player_rb;

    private void Awake()
    {
        _playerController = new PlayerController();
        player_rb = gameObject.GetComponent<Rigidbody>();
    }



    private void Start()
    {
        dashCooldown = 0;
        movementAnimator = GetComponent<Animator>();
        _playerController.PlayerMovement.Movement.performed += UpdateWhenMoved;
        _playerController.PlayerMovement.Movement.canceled += UpdateWhenMoved;
    }



    private void Update()
    {


        //dashCooldown = (dashCooldown > 0) ? dashCooldown-=Time.deltaTime:dashCooldown;

        //dash = _playerController.PlayerMovement.Dash.IsPressed();



    }




    // Update is called once per frame
    void FixedUpdate()
    {


        //dash
        if (dash &&  dashCooldown<=0)
        {
            //moves the game object this script is attached to based on WASD input
            transform.Translate(new Vector3(moveInput.x, 0, moveInput.y) * 5.0f);

            //player_rb.AddForce(new Vector3(moveInput.x, 0, moveInput.y) * 5.0f);
            

            dashCooldown = PlayerInfo.instance.dashCooldown;
        }


        //basic player movement
        //moves the game object this script is attached to based on WASD input 

        movementAnimator.SetFloat("MovementSpeed", playerMovement.magnitude);

        this.gameObject.GetComponent<Rigidbody>().velocity = playerMovement;


        currentPosition = transform.position;
    }


    public void UpdateWhenMoved(InputAction.CallbackContext e)
    {


        //stores the player's WASD input as a vector2, with AD as the x-axis and WS as the y-axis


        moveInput = _playerController.PlayerMovement.Movement.ReadValue<Vector2>();

        speed = PlayerInfo.instance.movementSpeed;

        playerMovement = new Vector3(moveInput.x, 0, moveInput.y) * speed;


        //player_rb.AddForce(playerMovement);

    }

    public void UpdateWhenStopMoved(InputAction.CallbackContext e)
    {

        /*
        //stores the player's WASD input as a vector2, with AD as the x-axis and WS as the y-axis


        moveInput = _playerController.PlayerMovement.Movement.ReadValue<Vector2>();

        speed = PlayerInfo.instance.movementSpeed;

        playerMovement = new Vector3(moveInput.x, 0, moveInput.y) * speed;

        //player_rb.AddForce(playerMovement);
        */
    }

    private void OnEnable()
    {
        //begins player movement functions
        _playerController.PlayerMovement.Enable();
    }


    private void OnDisable()
    {
        //ends player movement functions
        _playerController.PlayerMovement.Disable();
    }

}
