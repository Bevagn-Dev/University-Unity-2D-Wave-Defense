using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InputHandler : MonoBehaviour
{
    // Movement values
    [Header("Movement Values")]
    public float verticalInput;
    public float horizontalInput;
    public float moveAmount;

    // Player actions
    [Header("Player Actions")]
    [SerializeField] private bool sprintInput = false;
    [SerializeField] private bool interactInput = false;
    [SerializeField] private bool attackInput = false;

    // Events for player actions
    [HideInInspector] public UnityEvent OnInteract;
    [HideInInspector] public UnityEvent OnAttack;

    // References to other player components
    private PlayerControls playerControls;
    private PlayerMovement playerMovement;
    private PlayerManager playerManager;
    private PlayerInteraction playerInteraction;
    private PlayerAttack playerAttack;

    // Movement input vector
    private Vector2 movementInput;

    private void Awake()
    {
        // Get references to other player components
        playerMovement = GetComponent<PlayerMovement>();
        playerInteraction = GetComponent<PlayerInteraction>();
        playerAttack = GetComponent<PlayerAttack>();
        playerManager = GetComponent<PlayerManager>();

        // Add listeners to player action events
        OnInteract.AddListener(playerInteraction.HandleInteract);
        OnAttack.AddListener(playerAttack.HandleAttack);
    }

    private void OnEnable()
    {
        if (playerControls == null)
        {
            // Initialize player controls
            playerControls = new PlayerControls();

            // Set up movement input action
            playerControls.Movement.Move.performed += i => movementInput = i.ReadValue<Vector2>();

            // Set up sprint input actions
            playerControls.Actions.Sprint.performed += i => sprintInput = true;
            playerControls.Actions.Sprint.canceled += i => sprintInput = false;

            // Set up interact and attack input actions
            playerControls.Actions.Interact.performed += i => Interact();
            playerControls.Actions.Attack.performed += i => Attack();
        }

        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    private void Update()
    {
        // Handle movement input
        HandleMovementInput();
    }

    // Process movement input
    private void HandleMovementInput()
    {
        verticalInput = movementInput.y;
        horizontalInput = movementInput.x;
        movementInput.Normalize();

        moveAmount = Mathf.Clamp01(Mathf.Abs(verticalInput) + MathF.Abs(horizontalInput));
    }

    // Check if sprinting input is active
    public bool HandleSprintingInput()
    {
        return sprintInput;
    }

    // Process interact input
    private void Interact()
    {
        if (!interactInput)
        {
            // Invoke interact action and set interactInput to true
            interactInput = true;
            OnInteract.Invoke();
            Invoke("ResetInteractBool", .1f);
        }
    }

    // Process attack input
    private void Attack()
    {
        if (!attackInput && moveAmount == 0)
        {
            // Invoke attack action and set attackInput to true
            attackInput = true;
            OnAttack.Invoke();

            // Start coroutine to reset attack input after specified delay
            StartCoroutine(ResetAttackAfterDelay());
        }
    }

    // Reset interact input after a short delay
    private void ResetInteractBool()
    {
        interactInput = false;
    }

    // Coroutine to reset attack input after a delay determined by attack rate
    private IEnumerator ResetAttackAfterDelay()
    {
        yield return new WaitForSeconds(playerManager.playerValues.attackRate);
        attackInput = false;
    }
}
