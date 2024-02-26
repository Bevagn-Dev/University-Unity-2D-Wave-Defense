using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerUI : MonoBehaviour
{
    public UnityEvent startWaves;
    public GameObject shopUI;
    public GameObject deathScreen;

    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI waveText;
    [SerializeField] private TextMeshProUGUI finalWaveText;
    [SerializeField] private TextMeshProUGUI deathText;
    [SerializeField] private Slider townHealthBar;

    private PlayerManager playerManager;
    private TownHealth townHealth;

    public PlayerValues[] playerValues;

    private int numberOfWaves;

    private void Start() 
    {
        playerManager = PlayerManager.Instance;
        townHealth = TownHealth.Instance;

        UpdateMoneyText();
        UpdateWaveText();
        SetMaxHealth(townHealth.maxHealth);

        deathScreen.SetActive(false);

        startWaves.AddListener(GameObject.FindGameObjectWithTag("EnemySpawner").GetComponent<EnemySpawn>().StartEnemySpawning);
    }

    private void Update() 
    {
        UpdateWaveText();
        setHealth(townHealth.currentHealth);
    }

    public void StartWaveButton()
    {
        startWaves.Invoke();
    }

    public void ShowShop()
    {
        if(!shopUI.activeSelf)
        {
            Time.timeScale = 0;
            shopUI.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            shopUI.SetActive(false);
        }
    }

    public void QuitShop()
    {
        if(shopUI.activeSelf)
        {
            Time.timeScale = 1;
            shopUI.SetActive(false);
        }
    }

    public void UpdateMoneyText()
    {
        moneyText.SetText(playerManager.currentMoney.ToString());
    }
    public void UpdateWaveText()
    {
        waveText.SetText(numberOfWaves.ToString());
    }

    public void IncreaseWaveCount()
    {
        numberOfWaves++;
    }

    public void UpgradeOne()
    {
        if(playerManager.currentMoney >= 80)
        {
            playerManager.playerMovement.playerValues = playerValues[0];
        }
    }

    public void UpgradeTwo()
    {
        if(playerManager.currentMoney >= 120)
        {
            playerManager.playerMovement.playerValues = playerValues[1];
        }
    }

    public void UpgradeThree()
    {
        if(playerManager.currentMoney >= 180)
        {
            playerManager.playerMovement.playerValues = playerValues[2];
        }
    }

    public void RegenerateHealth()
    {
        if(playerManager.currentMoney >= 30)
        {
            playerManager.currentHealth = 100;
        }
    }

    public void HandleReturn(int sceneIndex)
    {
        SceneManager.LoadScene(sceneIndex);
    }

    public void HandleRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void SetMaxHealth(int health)
    {
        townHealthBar.maxValue = health;
        townHealthBar.value = health;
    }
    
    public void setHealth(int health)
    {
        townHealthBar.value = health;
    }

    public void SetPlayerDeathScreen()
    {
        if(!deathScreen.activeSelf)
        {
            deathText.SetText("YOU DIED!");
            finalWaveText.SetText(numberOfWaves.ToString());
            deathScreen.SetActive(true);
        }
    }

    public void SetTownDeathScreen()
    {
        if(!deathScreen.activeSelf)
        {
            deathText.SetText("THEY BROKE IN!");
            finalWaveText.SetText(numberOfWaves.ToString());
            deathScreen.SetActive(true);
        }
    }

}
