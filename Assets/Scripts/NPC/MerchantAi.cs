using UnityEngine;
using UnityEngine.Events;

public class MerchantAi : MonoBehaviour, IInteractable
{
    [HideInInspector]
    public UnityEvent interactShop; // Event invoked when interacting with the merchant

    private PlayerUI playerUI; // Reference to the PlayerUI script

    private void Awake()
    {
        // Find and assign the PlayerUI component
        playerUI = FindObjectOfType<PlayerUI>();

        // Add a listener to the interactShop event to handle shop interaction in the PlayerUI
        interactShop.AddListener(playerUI.HandleShop);
    }

    // Method called when interacting with the merchant
    public void Interact()
    {
        // Invoke the interactShop event to trigger shop interaction
        interactShop.Invoke();
    }
}
