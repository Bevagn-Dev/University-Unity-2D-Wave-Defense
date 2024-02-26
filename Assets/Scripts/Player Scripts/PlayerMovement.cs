using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;
using UnityEngine.AI;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Values")]
    public PlayerValues playerValues;

    private float playerSpeed;

    [HideInInspector]
    public Animator playerAnimator;
    private Rigidbody2D playerRigidbody;
    private InputHandler inputHandler;

    private Vector2 playerDirection;

    [HideInInspector]    
    public Vector2 lastMoveDirection;



    private void Awake()
    {
        init();
    }

    private void FixedUpdate()
    {
        HandleLocomotion();
    }

    private void Update()
    {
        HandleInput();
        HandleAnimations();
    }

    private void HandleInput()
    {
        playerDirection.x = inputHandler.horizontalInput;
        playerDirection.y = inputHandler.verticalInput;

        if(inputHandler.HandleSprintingInput() == true)
        {
            playerSpeed = playerValues.sprintSpeed;

        }
        else
        {
            playerSpeed = playerValues.walkSpeed;
        }
    }

    private void HandleLocomotion()
    {
        playerRigidbody.velocity = new Vector2(playerDirection.x * playerSpeed, playerDirection.y * playerSpeed);
    }


    private void HandleAnimations()
    {
        playerAnimator.SetFloat("Horizontal", lastMoveDirection.x);
        playerAnimator.SetFloat("Vertical", lastMoveDirection.y);
        playerAnimator.SetFloat("Speed", inputHandler.moveAmount);

        playerAnimator.SetBool("Sprinting", inputHandler.HandleSprintingInput());

        if (inputHandler.moveAmount > 0.01f)
        {
            lastMoveDirection = playerDirection;
        }
    }

    private void init()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        inputHandler = GetComponent<InputHandler>();
        playerAnimator = GetComponentInChildren<Animator>();
    }
}
