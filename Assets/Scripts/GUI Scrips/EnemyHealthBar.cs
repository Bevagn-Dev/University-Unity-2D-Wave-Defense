using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    // Reference to the health bar slider
    public Slider healthBar; 

    // Method to set the maximum health value for the enemy
    public void SetMaxHealth(int health)
    {
        // Set the maximum value of the health bar
        healthBar.maxValue = health;
        // Set the current value of the health bar to the maximum
        healthBar.value = health; 
    }

    // Method to update the current health value of the enemy
    public void SetHealth(int health)
    {
        // Set the current value of the health bar
        healthBar.value = health;
    }
}
