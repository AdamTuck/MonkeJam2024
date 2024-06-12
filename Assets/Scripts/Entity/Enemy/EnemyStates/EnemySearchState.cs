using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySearchState : EnemyState
{
    public EnemySearchState(EnemyController _enemy) : base(_enemy)
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
        enemy.roamTimer += Time.deltaTime;
        enemy.searchTimer += Time.deltaTime;

        if (enemy.searchTimer >= enemy.searchTimeout)
        {
            enemy.roamTimer = 0;
            enemy.searchTimer = 0;
            enemy.ChangeState(new EnemyPatrolState(enemy));
            return;
        }

        if (enemy.roamTimer >= enemy.roamFrequency)
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
        }

        if (Physics.SphereCast(enemy.enemyEye.position, enemy.checkRadius, enemy.transform.forward, out RaycastHit hit, enemy.playerCheckDistance))
        {
            if (hit.transform.CompareTag("Player"))
            {
                Debug.Log("PLAYER SPOTTED");
                enemy.player = hit.transform;
                enemy.agent.destination = enemy.player.position;

                enemy.ChangeState(new EnemyFollowState(enemy));
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
