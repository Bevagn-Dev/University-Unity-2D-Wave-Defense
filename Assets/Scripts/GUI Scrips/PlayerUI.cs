using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    // Singleton instance
    public static PlayerUI Instance { get; private set; }

    // Event to start waves
    [HideInInspector]
    public UnityEvent startWaves;

    // Text elements
    [Header("Text Elements")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI enemiesLeftText;
    [SerializeField] private TextMeshProUGUI survivedWavesText;
    [SerializeField] private TextMeshProUGUI deathText;

    // UI elements
    [Space(5)]
    [Header("UI Elements")]
    public GameObject deathScreen;
    [SerializeField] private Slider townHealthBar;

    // References to various components
    [HideInInspector]
    public PlayerManager playerManager;
    [HideInInspector]
    public PlayerHealthBar playerHealthBar;
    private TownHealth townHealth;
    private EnemySpawn enemySpawn;
    private ShopManager shopManager;

    private int enemiesLeft;

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

        Init();
    }

    private void Update()
    {
        // Update GUI elements
        UpdateGUIValues();
    }

    // Method to start waves
    public void StartWaveButton()
    {
        startWaves.Invoke();
    }

    // Method to handle shop interaction
    public void HandleShop()
    {
        // Toggle shop UI
        if (!shopManager.shopParent.activeSelf)
        {
            Time.timeScale = 0;
            shopManager.shopParent.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            shopManager.shopParent.SetActive(false);
        }
    }

    // Method to quit shop
    public void QuitShop()
    {
        if (shopManager.shopParent.activeSelf)
        {
            Time.timeScale = 1;
            shopManager.shopParent.SetActive(false);
        }
    }

    // Method to update GUI values
    public void UpdateGUIValues()
    {
        moneyText.SetText(playerManager.currentMoney.ToString());
        waveText.SetText(enemySpawn.currentWave.ToString());

        // Calculates the amount of enemies Left
        enemiesLeft = enemySpawn.enemiesToSpawn.Count + enemySpawn.spawnedEnemies.Count;

        enemiesLeftText.SetText(enemiesLeft.ToString());
        setHealth(townHealth.currentHealth);
    }

    // Method to handle return to main menu
    public void HandleReturn(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    // Method to handle game restart
    public void HandleRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Method to set max health for town
    public void SetMaxHealth(int health)
    {
        townHealthBar.maxValue = health;
        townHealthBar.value = health;
    }

    // Method to set health for town
    public void setHealth(int health)
    {
        townHealthBar.value = health;
    }

    // Method to display player death screen
    public void SetPlayerDeathScreen(string deathMessage)
    {
        if (!deathScreen.activeSelf)
        {
            deathText.SetText(deathMessage);
            survivedWavesText.SetText(enemySpawn.currentWave.ToString());
            deathScreen.SetActive(true);
        }
    }

    private void Init()
    {
        // Get references to various components
        playerManager = FindObjectOfType<PlayerManager>();
        townHealth = FindObjectOfType<TownHealth>();
        enemySpawn = FindObjectOfType<EnemySpawn>();
        playerHealthBar = GetComponentInChildren<PlayerHealthBar>();
        shopManager = GetComponentInChildren<ShopManager>();

        // Set max health for town
        SetMaxHealth(townHealth.maxHealth);

        // Disable death screen initially
        deathScreen.SetActive(false);

        // Subscribe to startWaves event
        startWaves.AddListener(enemySpawn.StartEnemySpawning);
    }
}
