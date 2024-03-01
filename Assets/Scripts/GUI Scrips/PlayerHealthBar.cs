using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBar : MonoBehaviour
{
    public Slider healthBar; // Reference to the health bar UI element
    public Slider staminaBar; // Reference to the stamina bar UI element

    // Method to set the maximum health value and current health value of the health bar
    public void SetMaxHealth(int health)
    {
        healthBar.maxValue = health; // Set the maximum value of the health bar
        healthBar.value = health; // Set the current value of the health bar to the maximum
    }

    // Method to update the current health value of the health bar
    public void SetHealth(int health)
    {
        healthBar.value = health; // Update the current value of the health bar
    }
}
