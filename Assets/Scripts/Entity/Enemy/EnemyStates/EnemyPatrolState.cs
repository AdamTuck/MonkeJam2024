using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolState : EnemyState
{
    int currentTarget = 0;

    public EnemyPatrolState (EnemyController _enemy) : base(_enemy)
    {

    }

    public override void OnStateEnter()
    {
        enemy.agent.destination = enemy.patrolPoints[currentTarget].position;
        Debug.Log("Enemy is patrolling");
    }

    public override void OnStateExit()
    {
        Debug.Log("Enemy stops patrolling");
    }

    public override void OnStateUpdate()
    {
        if (enemy.agent.remainingDistance <= 0.2f)
        {
            currentTarget++;

            if (currentTarget >= enemy.patrolPoints.Length)
                currentTarget = 0;

            enemy.agent.destination = enemy.patrolPoints[currentTarget].position;
        }

        if (Physics.SphereCast(enemy.enemyEye.position, enemy.checkRadius, enemy.transform.forward, out RaycastHit hit, enemy.playerCheckDistance))
        {
            if (hit.transform.CompareTag("Player"))
            {
                Debug.Log("PLAYER SPOTTED");
                enemy.player = hit.transform;
                enemy.agent.destination = enemy.player.position;

                enemy.ChangeState(new EnemyFollowState(enemy));
                return;
            }
        }
    }
}