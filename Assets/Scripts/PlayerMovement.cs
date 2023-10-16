using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{

    Vector2 moveInput;
    PlayerController _playerController;
    float speed;

    bool dash = false;

    float dashCooldown;

    Animator movementAnimator;
    Vector3 playerMovement;

    private void Awake()
    {
        _playerController = new PlayerController();

    }



    private void Start()
    {
        dashCooldown = 0;
        movementAnimator = GetComponent<Animator>();
    }



    private void Update()
    {
        speed = PlayerInfo.instance.movementSpeed;

        //stores the player's WASD input as a vector2, with AD as the x-axis and WS as the y-axis
        moveInput = _playerController.PlayerMovement.Movement.ReadValue<Vector2>();


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

            dashCooldown = PlayerInfo.instance.dashCooldown;
        }
        
        //basic player movement
        //moves the game object this script is attached to based on WASD input 

        playerMovement = new Vector3(moveInput.x, 0, moveInput.y) * speed;

        movementAnimator.SetFloat("MovementSpeed", playerMovement.magnitude);

        this.gameObject.GetComponent<Rigidbody>().velocity = playerMovement;
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
