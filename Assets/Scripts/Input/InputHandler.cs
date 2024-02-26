using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Unity.Mathematics;
using UnityEngine;

public class InputHandler : MonoBehaviour
{
    [Header("Movement Values")]
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    [Header("Player Actions")]
    [SerializeField] private bool sprintInput = false;


    private PlayerControls playerControls;
    private PlayerMovement playerMovement;

    private Vector2 movementInput;

    private void Awake() 
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void OnEnable() 
    {
        if(playerControls == null)
        {
            playerControls = new PlayerControls();

            playerControls.Movement.Move.performed += i => movementInput = i.ReadValue<Vector2>();

            playerControls.Actions.Sprint.performed += i => sprintInput = true;
            playerControls.Actions.Sprint.canceled += i => sprintInput = false;
        }

        playerControls.Enable();
    }

    private void OnDisable() 
    {
        playerControls.Disable();
    }

    private void Update() 
    {
        HandleMovementInput();
    }

    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        movementInput.Normalize();

        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + MathF.Abs(horizontalInput));
    }

    public bool HandleSprintingInput()
    {
        return sprintInput;
    }

}
