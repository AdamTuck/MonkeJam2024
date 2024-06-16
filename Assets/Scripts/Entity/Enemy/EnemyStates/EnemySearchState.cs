using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySearchState : EnemyState
{
    public EnemySearchState(EnemyController _enemy, Animator _animator) : base(_enemy, _animator)
    {

    }

    public override void OnStateEnter()
    {
        Debug.Log("Enemy searching");
    }

    public override void OnStateExit()
    {
        Debug.Log("Enemy stops searching");
    }

    public override void OnStateUpdate()
    {


        /*enemy.searchTimer += Time.deltaTime;
        if (enemy.searchTimer >= enemy.searchTimeout)
        {
            enemy.roamTimer = 0;
            enemy.searchTimer = 0;
            enemy.ChangeState(new EnemyPatrolState(enemy));
            return;
        }*/

        enemy.idleTimer += Time.deltaTime;
        if (enemy.idleTimer < enemy.idleTime)
        {
            //Idle
            animator.SetBool("Idle", true);
            enemy.idleTimer += Time.deltaTime;
            enemy.agent.SetDestination(enemy.transform.position);
        }
        else if (enemy.roamTimer < enemy.roamTime)
        {
            enemy.roamTimer += Time.deltaTime;
            animator.SetBool("Idle", false);
            animator.SetBool("Running", true);
            if (!enemy.agent.pathPending)
            {
                if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance)
                {
                    if (!enemy.agent.hasPath || enemy.agent.velocity.sqrMagnitude == 0f)
                    {
                        enemy.agent.SetDestination(RandomNavmeshLocation(enemy.roamRadius));
                    }
                }
            }
            //Else it just walks
        }
        else
        {
            //Reset timers and start idle again
            animator.SetBool("Idle", true);
            animator.SetBool("Running", false);
            enemy.roamTimer = 0;
            enemy.idleTimer = 0;
        }

        /*
        enemy.roamTimer += Time.deltaTime;
        if (enemy.roamTimer >= enemy.roamTime)
        {
            enemy.agent.SetDestination(RandomNavmeshLocation(enemy.roamRadius));
            enemy.roamTimer = 0;
        }

        if (!enemy.agent.pathPending)
        {
            if (enemy.agent.remainingDistance <= enemy.agent.stoppingDistance)
            {
                if (!enemy.agent.hasPath || enemy.agent.velocity.sqrMagnitude == 0f)
                {
                    enemy.agent.SetDestination(RandomNavmeshLocation(enemy.roamRadius));
                    enemy.roamTimer = 0;
                }
            }
        }*/

        if (Physics.SphereCast(enemy.enemyEye.position, enemy.checkRadius, enemy.transform.forward, out RaycastHit hit, enemy.playerCheckDistance))
        {
            if (hit.transform.CompareTag("Player"))
            {
                Debug.Log("PLAYER SPOTTED");
                enemy.player = hit.transform;
                enemy.agent.destination = enemy.player.position;

                enemy.ChangeState(new EnemyFollowState(enemy, animator));
            }
        }
    }

    private Vector3 RandomNavmeshLocation(float radius)
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += enemy.transform.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, 1))
        {
            finalPosition = hit.position;
        }
        return finalPosition;
    }
}
