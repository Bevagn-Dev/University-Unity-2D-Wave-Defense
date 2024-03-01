using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Player Interaction Parameters")]
    // Height offset for the interaction position
    [SerializeField] private float heightOffset = 1f;
    // Forward offset for the interaction position
    [SerializeField] private float forwardOffset = 1f;
    // Radius for interaction detection
    [SerializeField] private float interactRadius = 2f;

    // Reference to the PlayerMovement script
    private PlayerMovement playerMovement;
    // Reference to the InputHandler script
    private InputHandler inputHandler;
    // Position for interaction detection
    private Vector2 interactPosition;       

    private void Awake()
    {
        Init();
    }

    // Method to handle player interaction
    public void HandleInteract()
    {
        // Calculate the interaction position based on player movement
        interactPosition = new Vector2(transform.position.x, transform.position.y + heightOffset) + playerMovement.lastMoveDirection * forwardOffset;

        // Perform a circle cast to detect interactable objects
        RaycastHit2D hit = Physics2D.CircleCast(interactPosition, interactRadius, playerMovement.lastMoveDirection);

        // If an object is hit, check if it's interactable and invoke its interaction method
        if (hit.collider != null)
        {
            IInteractable interactable = hit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                interactable.Interact();
            }
        }
    }

    // Method for initializing references
    private void Init()
    {
        playerMovement = GetComponent<PlayerMovement>();
        inputHandler = GetComponent<InputHandler>();
    }

    // Draw gizmos to visualize the interaction radius
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(interactPosition, interactRadius);
    }
}
