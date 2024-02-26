using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerManager : MonoBehaviour , IDamageable
{    
    public static PlayerManager Instance{get; private set;}
    public PlayerMovement playerMovement;
    

    public int maxHealth;
    public int startingMoney;

    public PlayerHealthBar healthBar;
    public UnityEvent updateMoney;
    public UnityEvent playerDeath;

    public int currentHealth;
    public int currentMoney;


    private void Awake() 
    {
       if(Instance != null && Instance != this)
       {
            Destroy(this);
       }
       else
       {
            Instance = this;
       }
        
        updateMoney.AddListener(GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<PlayerUI>().UpdateMoneyText);
        playerDeath.AddListener(GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<PlayerUI>().SetPlayerDeathScreen);
        playerMovement = GetComponent<PlayerMovement>();

        currentMoney = startingMoney;
        currentHealth = maxHealth;
        healthBar.SetMaxHealth(maxHealth);
    }

    private void Update()
    {
        healthBar.setHealth(currentHealth);
    }

    public void TakeDamage(int Damage)
    {

        Debug.Log("Hit");

        currentHealth -= Damage;

        if(currentHealth <0 )
        {
            currentHealth = 0;
            playerDeath.Invoke();
            Destroy(gameObject, 1);
        }
    }

    public void AddMoney()
    {
        currentMoney += 10;
        updateMoney.Invoke();
    }

}
