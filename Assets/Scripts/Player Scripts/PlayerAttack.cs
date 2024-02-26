using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack parameters")]
    [SerializeField] private float attackForwardOffset = 1f; 
    [SerializeField] private float attackHeightOffset = 1f; 
    [SerializeField] private LayerMask enemyLayer;

    private Vector2 attackPosition;
    private PlayerMovement playerMovement;

    private float attackDelay;

    private void Awake() 
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update() 
    {
        if(Time.time >= attackDelay)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {
                HandleAttack();
                attackDelay = Time.time + 1f / playerMovement.playerValues.attackRate;
            }        
        }
    }

    private void HandleAttack()
    {
        playerMovement.playerAnimator.SetTrigger("Attack");

        attackPosition = new Vector2(transform.position.x, transform.position.y + attackHeightOffset) + playerMovement.lastMoveDirection * attackForwardOffset;

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPosition, playerMovement.playerValues.attackRadius, enemyLayer);

        foreach(Collider2D objects in hitObjects)
        {
            IDamageable damageable = objects.GetComponent<IDamageable>();

            if(damageable != null)
            {
                damageable.TakeDamage(playerMovement.playerValues.attackDamage);
            }
        }
    }    
}
