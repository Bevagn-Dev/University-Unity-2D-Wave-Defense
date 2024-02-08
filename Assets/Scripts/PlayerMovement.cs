using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement parameters")]
    [SerializeField] private float playerSpeed = 1f;


    private Animator playerAnimator;
    private Rigidbody2D playerRigidbody;

    private Vector2 playerDirection;
    private Vector2 lastMoveDirection;

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
        playerDirection.x = Input.GetAxis("Horizontal");
        playerDirection.y = Input.GetAxis("Vertical");
        playerDirection.Normalize();

        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            HandleAttack();
        }
    }

    private void HandleLocomotion()
    {
        playerRigidbody.velocity = new Vector2(playerDirection.x * playerSpeed, playerDirection.y * playerSpeed);

        if(playerDirection.x == 0 && playerDirection.y == 0)
        {
            playerRigidbody.velocity = Vector2.zero;
        }
    }

    private void HandleAnimations()
    {
        playerAnimator.SetFloat("Horizontal", lastMoveDirection.x);
        playerAnimator.SetFloat("Vertical", lastMoveDirection.y);
        playerAnimator.SetFloat("Speed", playerDirection.sqrMagnitude);

        if (playerDirection.magnitude > 0.01f)
        {
            lastMoveDirection = playerDirection;
        }
    }

    private void HandleAttack()
    {
        playerAnimator.SetTrigger("Attack");


    }

    private void init()
    {
        playerRigidbody = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }
}
