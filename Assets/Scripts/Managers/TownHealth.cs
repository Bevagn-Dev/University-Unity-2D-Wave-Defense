using UnityEngine;
using UnityEngine.Events;

public class TownHealth : MonoBehaviour, IDamageable
{
    public static TownHealth Instance { get; private set; } // Singleton instance reference

    public int maxHealth; // Maximum health of the town

    [HideInInspector]
    public int currentHealth; // Current health of the town
    [HideInInspector]
    public UnityEvent townDeath; // Event invoked when the town is destroyed
    private PlayerUI playerUI; // Reference to the PlayerUI script

    private void Awake()
    {
        // Singleton pattern implementation
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        // Find and assign the PlayerUI component
        playerUI = FindObjectOfType<PlayerUI>();

        // Add a listener to the townDeath event to display the player death screen when the town is destroyed
        townDeath.AddListener(() => playerUI.SetPlayerDeathScreen("THE TOWN WAS DESTROYED!"));

        // Initialize the current health to the maximum health
        currentHealth = maxHealth;
    }

    // Method to handle damage taken by the town
    public void TakeDamage(int damage)
    {
        // Decrease the current health by the amount of damage taken
        currentHealth -= damage;

        // Ensure that health doesn't go below zero
        if (currentHealth < 0)
        {
            currentHealth = 0;
            // Invoke the townDeath event when the town's health reaches zero
            townDeath.Invoke();
        }
    }
}
