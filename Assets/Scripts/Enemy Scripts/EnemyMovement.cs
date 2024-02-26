using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum AIStates
{
    RunningState,
    ChasingState,
    AttackState,
}

public class EnemyMovement : MonoBehaviour
{

    [Header("Debug")]
    [SerializeField] private Transform currentTarget;
    [SerializeField] private AIStates states = AIStates.RunningState;

    [Header("Detection Values")]
    [SerializeField] private float detectionRadius = 5f;
    [SerializeField] private float attackRange = 2f;
    [SerializeField] private LayerMask detectionLayer;

    [Header("Attack parameters")]
    [SerializeField] private float attackForwardOffset = 1.4f; 
    [SerializeField] private float attackHeightOffset = 1f; 
    [SerializeField] private float attackRadius = 1.2f;
    [SerializeField] private float attackRate = 2f;
    [SerializeField] private int attackDamage = 10;
    [SerializeField] private LayerMask attackLayer;

    private EnemyManager enemyManager;
    private NavMeshAgent enemyNavMesh;
    private Animator enemyAnimator;

    private float distanceToTarget;

    private Vector2 moveDirection;
    private Vector2 attackPosition;


    void Start()
    {
        Init();
        currentTarget = TownHealth.Instance.transform;
        StartCoroutine(HandleStates());
    }


    void Update()
    {
        attackPosition = new Vector2(transform.position.x, transform.position.y + attackHeightOffset) + moveDirection * attackForwardOffset;

        HandleAnimations();

        distanceToTarget = Vector2.Distance(currentTarget.position, transform.position);
        moveDirection = GetMoveDirection();
        enemyNavMesh.SetDestination(currentTarget.position);
    }

private IEnumerator HandleStates()
{
    while (true)
    {
        switch (states)
        {
            case AIStates.RunningState:

                currentTarget = TownHealth.Instance.transform;

                if(PlayerDetection() == true)
                {
                    states = AIStates.ChasingState;
                    break;
                }
                    
                if (!PlayerDetection())
                {
                    enemyNavMesh.SetDestination(currentTarget.position);

                    if (Vector2.Distance(transform.position, currentTarget.position) <= enemyNavMesh.stoppingDistance)
                    {
                        states = AIStates.AttackState;
                    }
                }

                break;

            case AIStates.ChasingState:
                currentTarget = PlayerManager.Instance.transform;

                if (distanceToTarget < attackRange)
                {
                    states = AIStates.AttackState;
                }

                break;

            case AIStates.AttackState:

                if (distanceToTarget < attackRange)
                {
                    HandleAttack();
                    yield return new WaitForSeconds(attackRate);
                }
                else if(PlayerDetection())
                {
                    states = AIStates.ChasingState;
                }
                else
                {
                    states = AIStates.RunningState;
                }

                break;

            default:
                break;
        }

        yield return null;
    }
}

    public bool PlayerDetection()
    {
        Collider2D[] detectedObjects = Physics2D.OverlapCircleAll(transform.position, detectionRadius, detectionLayer);

        foreach (Collider2D objects in detectedObjects)
        {
            if (objects.CompareTag("Player"))
            {
                return true;
            }
        }

        return false;
    }

    private void HandleAnimations()
    {
        enemyAnimator.SetFloat("Horizontal", moveDirection.x);
        enemyAnimator.SetFloat("Vertical", moveDirection.y);

        if (enemyNavMesh.remainingDistance > enemyNavMesh.stoppingDistance)
        {
            enemyAnimator.SetFloat("Speed", 1f);
        }
        else
        {
            enemyAnimator.SetFloat("Speed", 0f);
        }
    }

    private void HandleAttack()
    {
        enemyAnimator.SetTrigger("Attack");

        attackPosition = new Vector2(transform.position.x, transform.position.y + attackHeightOffset) + moveDirection * attackForwardOffset;

        Collider2D[] hitObjects = Physics2D.OverlapCircleAll(attackPosition, attackRadius, attackLayer);

        foreach(Collider2D objects in hitObjects)
        {
            IDamageable damageable = objects.GetComponent<IDamageable>();

            if(damageable != null)
            {
                damageable.TakeDamage(attackDamage);
            }
        }
    }        

    private Vector2 GetMoveDirection()
    {
        if(currentTarget == null)
            return Vector2.zero;

        Vector2 moveDir = currentTarget.position - transform.position;
        moveDir.Normalize();

        float horizontal = Mathf.Clamp(moveDir.x, -1, 1);
        float vertical = Mathf.Clamp(moveDir.y, -1, 1);

        return new Vector2(horizontal, vertical);
    }

    private void OnDrawGizmosSelected() 
    {
        Gizmos.DrawWireSphere(attackPosition, attackRange);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPosition, attackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

    private void Init()
    {
        enemyNavMesh = GetComponent<NavMeshAgent>();
        enemyAnimator = GetComponentInChildren<Animator>();
        enemyManager = GetComponent<EnemyManager>();
    }
     
}
