using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int maxHealth;


    private int currentHealth;

    private void Awake()
    {
        currentHealth = maxHealth;
    }

    private void Update()
    {
        if(currentHealth <0 )
        {
            currentHealth = 0;
        }
    }
}
