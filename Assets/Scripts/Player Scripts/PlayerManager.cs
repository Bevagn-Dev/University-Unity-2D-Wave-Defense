using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour, IDamageable
{
    // Singleton instance
    public static PlayerManager Instance { get; private set; }

    // Player values
    [Header("Player Values")]
    public PlayerValues playerValues;

    // Initial money
    public int startingMoney = 10;

    // Safety period after taking damage
    [SerializeField] private float safetyPeriod = 5f;

    // Events
    [HideInInspector]
    public UnityEvent playerDeath;

    // Current health and money
    [HideInInspector]
    public int currentHealth;
    [HideInInspector]
    public int currentMoney;

    // Components
    [HideInInspector]
    public PlayerMovement playerMovement;

    private PlayerUI playerUI;
    private PlayerHealthBar healthBar;

    private float lastDamageTime;

    private Coroutine regenerateCoroutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        // Initialize player
        Init();
    }

    private void Update()
    {
        // Update health bar
        healthBar.SetHealth(currentHealth);

        // Check safety period for health regeneration
        if (Time.time - lastDamageTime >= safetyPeriod && regenerateCoroutine == null)
        {
            regenerateCoroutine = StartCoroutine(RegenHealth());
        }
    }

    // Method to handle taking damage
    public void TakeDamage(int Damage)
    {
        currentHealth -= Damage;

        lastDamageTime = Time.time;

        if (currentHealth < 0)
        {
            currentHealth = 0;
            playerDeath.Invoke();

            Destroy(gameObject, 1);
        }

        // Stop health regeneration coroutine if running
        if (regenerateCoroutine != null)
        {
            StopCoroutine(regenerateCoroutine);
            regenerateCoroutine = null;
        }
    }

    // Method to increment money
    public void IncrementMoney(int money)
    {
        currentMoney += money;
    }

    // Coroutine for health regeneration
    IEnumerator RegenHealth()
    {
        yield return new WaitForSeconds(playerValues.regenDelay);

        while (currentHealth < playerValues.maxHealth)
        {
            currentHealth += playerValues.regenAmount;
            yield return new WaitForSeconds(1f);
        }

        regenerateCoroutine = null;
    }

    // Initialization method
    private void Init()
    {
        playerUI = FindObjectOfType<PlayerUI>();
        healthBar = FindObjectOfType<PlayerHealthBar>();
        playerDeath.AddListener(() => playerUI.SetPlayerDeathScreen("YOU DIED!"));

        playerMovement = GetComponent<PlayerMovement>();

        currentMoney = startingMoney;
        currentHealth = playerValues.maxHealth;
        healthBar.SetMaxHealth(playerValues.maxHealth);
    }

}
