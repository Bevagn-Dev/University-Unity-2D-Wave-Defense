using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{

    [SerializeField] private float heightOffset = 1f;
    [SerializeField] private float forwardOffset = 1f;
    [SerializeField] private float interactRadius = 2f;


    private PlayerMovement playerMovement;
    private Vector2 interactPosition;

    private void Start() 
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update() 
    {
        HandleInteract();
    }

    private void HandleInteract()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            interactPosition = new Vector2(transform.position.x, transform.position.y + heightOffset) + playerMovement.lastMoveDirection * forwardOffset;
            
            RaycastHit2D hit = Physics2D.CircleCast(interactPosition, interactRadius, playerMovement.lastMoveDirection);
            
                if(hit.collider != null)
                {
                    IInteractable interactable = hit.collider.GetComponent<IInteractable>();

                    if(interactable != null)
                    {
                        interactable.Interact();
                    }
                }
        }
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(interactPosition, interactRadius);
    }
}
