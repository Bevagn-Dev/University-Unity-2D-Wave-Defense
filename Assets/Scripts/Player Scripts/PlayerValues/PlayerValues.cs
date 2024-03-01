using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Player Values")]
public class PlayerValues : ScriptableObject
{
    [Header("Health Parameters")]
    public int maxHealth = 100;
    public int regenAmount = 10;
    public int regenDelay = 1;

    [Header("Movement Parameters")]
    public float walkSpeed = 1f; 
    public float sprintSpeed = 3f; 

    [Header("Attack Parameters")]
    public int attackDamage = 5; 
    public float attackRadius = 1f; 
    public float attackRate = 2f;
}
