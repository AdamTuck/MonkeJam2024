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
    [SerializeField] public float idleTime;
    [SerializeField] public float roamTime;
    [SerializeField] public float roamRadius;
    [SerializeField] public float searchTimeout;
    [HideInInspector] public float roamTimer, searchTimer, idleTimer;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private List<AudioClip> sounds;
    [SerializeField] private float audioTime;
    private float audioTimer = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Default state (Patrolling)
        currentState = new EnemySearchState(this, animator);
        currentState.OnStateEnter();
    }

    void Update()
    {
        audioTimer += Time.deltaTime;
        if (audioTimer > audioTime)
        {
            int random = Random.Range(0, sounds.Count-1);
            audioSource.clip = sounds[random];
            audioSource.Play();
            audioTimer = 0;
        }
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