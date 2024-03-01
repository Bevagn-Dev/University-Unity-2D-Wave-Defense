using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D playerRigidbody;

    // References to other scripts
    private PlayerManager playerManager;
    private InputHandler inputHandler;

    // Animator component for player animations
    [HideInInspector]
    public Animator playerAnimator;

    // Last movement direction
    [HideInInspector]
    public Vector2 lastMoveDirection;

    private Vector2 playerDirection;

    // Player movement speed
    private float playerSpeed;

    private void Awake()
    {
        // Initialize components
        Initialize();
    }

    private void FixedUpdate()
    {
        // Handle player movement
        HandleLocomotion();
    }

    private void Update()
    {
        // Handle player input and animations
        HandleInput();
        HandleAnimations();
    }

    // Method to handle player input
    private void HandleInput()
    {
        // Get player movement input
        playerDirection.x = inputHandler.horizontalInput;
        playerDirection.y = inputHandler.verticalInput;

        // Set player speed based on input (walking or sprinting)
        if (inputHandler.HandleSprintingInput())
        {
            playerSpeed = playerManager.playerValues.sprintSpeed;
        }
        else
        {
            playerSpeed = playerManager.playerValues.walkSpeed;
        }
    }

    // Method to handle player locomotion
    private void HandleLocomotion()
    {
        // Move the player rigidbody based on input direction and speed
        playerRigidbody.velocity = new Vector2(playerDirection.x * playerSpeed, playerDirection.y * playerSpeed);
    }

    // Method to handle player animations
    private void HandleAnimations()
    {
        // Set animator parameters based on player input and movement
        playerAnimator.SetFloat("Horizontal", lastMoveDirection.x);
        playerAnimator.SetFloat("Vertical", lastMoveDirection.y);
        playerAnimator.SetFloat("Speed", inputHandler.moveAmount);
        playerAnimator.SetBool("Sprinting", inputHandler.HandleSprintingInput());

        // Update last movement direction
        if (inputHandler.moveAmount > 0.01f)
        {
            lastMoveDirection = playerDirection;
        }
    }

    // Method for initialization
    private void Initialize()
    {
        // Get components and references
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponentInChildren<Animator>();

        inputHandler = GetComponent<InputHandler>();
        playerManager = FindObjectOfType<PlayerManager>();
    }
}
