using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Items", menuName = "Player Values")]
public class PlayerValues : ScriptableObject
{
    [Header("Movement parameters")]
    public float walkSpeed = 1f; 
    public float sprintSpeed = 3f; 

    [Header("Attack parameters")]
    public int attackDamage = 5; 
    public float attackRadius = 1f; 
    public float attackRate = 2f;

}
