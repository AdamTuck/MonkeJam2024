using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private EnemyState currentState;
    public Animator animator;

    [Header("Navigation")]
    public Transform destinationPoint;
    public Transform[] patrolPoints;
    public Transform enemyEye;
    public float playerCheckDistance;
    public float checkRadius = 0.8f;
    public float enemyAggroRange, enemyAttackRange;
    [HideInInspector] public NavMeshAgent agent;
    [HideInInspector] public Transform player;

    // Roaming
    [Header("Searching")]
    public float idleTime;
    public float roamTime;
    public float roamRadius;
    public float searchTimeout;
     public float roamTimer, searchTimer, idleTimer;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Default state (Patrolling)
        currentState = new EnemySearchState(this, animator);
        currentState.OnStateEnter();
    }

    void Update()
    {
        currentState.OnStateUpdate();
    }

    private void OnEnable()
    {
        gameObject.GetComponent<Health>().OnHealthUpdated += OnDamaged;
        gameObject.GetComponent<Health>().OnDeath += OnDeath;
    }

    private void OnDamaged(float damage)
    {

    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }


    public void ChangeState (EnemyState state)
    {
        currentState.OnStateExit();
        currentState = state;
        currentState.OnStateEnter();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(enemyEye.position, checkRadius);
        Gizmos.DrawWireSphere(enemyEye.position + enemyEye.forward * playerCheckDistance, checkRadius);

        Gizmos.DrawLine(enemyEye.position, enemyEye.position + enemyEye.forward * playerCheckDistance);
    }
}