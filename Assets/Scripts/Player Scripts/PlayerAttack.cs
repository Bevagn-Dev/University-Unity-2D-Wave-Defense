using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // Attack Parameters
    [Header("Attack Parameters")]
    [SerializeField] private float attackForwardOffset = 1f;
    [SerializeField] private float attackHeightOffset = 1f;
    [SerializeField] private LayerMask enemyLayer;

    // Attack position
    private Vector2 attackPosition;

    // References to other scripts
    private PlayerMovement playerMovement;
    private PlayerManager playerManager;

    private void Awake()
    {
        // Initialization
        Init();
    }

    // Method to handle player attack
    public void HandleAttack()
    {
        // Trigger attack animation
        playerMovement.playerAnimator.SetTrigger("Attack");

        // Plays sound from sound manager using string
        SoundManager.Instance.Play("AttackSound");

        // Calculate attack position
        attackPosition = new Vector2(transform.position.x, transform.position.y + attackHeightOffset) + playerMovement.lastMoveDirection * attackForwardOffset;

        // Detect enemies in attack range
        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPosition, playerManager.playerValues.attackRadius, enemyLayer);

        // Deal damage to detected enemies
        foreach (Collider2D objects in hitObjects)
        {
            IDamageable damageable = objects.GetComponent<IDamageable>();

            if (damageable != null)
            {
                damageable.TakeDamage(playerManager.playerValues.attackDamage);
            }
        }
    }

    // Initialization method
    private void Init()
    {
        // Get references to other scripts
        playerManager = GetComponent<PlayerManager>();
        playerMovement = GetComponent<PlayerMovement>();
    }
}
