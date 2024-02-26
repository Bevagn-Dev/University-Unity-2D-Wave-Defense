using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class TownHealth : MonoBehaviour, IDamageable
{
    public static TownHealth Instance{get; private set;}

    public UnityEvent townDeath;

    public int maxHealth;
    public int currentHealth;

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
        townDeath.AddListener(GameObject.FindGameObjectWithTag("PlayerUI").GetComponent<PlayerUI>().SetTownDeathScreen);
        currentHealth = maxHealth;
    }


    public void TakeDamage(int Damage)
    {

        currentHealth -= Damage;

        if(currentHealth <0 )
        {
            currentHealth = 0;
            townDeath.Invoke();
        }
    }
}
