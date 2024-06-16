using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollowState : EnemyState
{
    float distanceToPlayer;

    public EnemyFollowState(EnemyController _enemy, Animator _animator) : base(_enemy, _animator)
    {

    }

    public override void OnStateEnter()
    {
        Debug.Log("Begin Following");
        animator.SetBool("Running", true);
        animator.SetBool("Idle", false);
    }

    public override void OnStateExit()
    {
        Debug.Log("Enemy stops following");
    }

    public override void OnStateUpdate()
    {
        if (enemy.player)
        {
            distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.position);

            if (distanceToPlayer > enemy.enemyAggroRange)
            {
                enemy.ChangeState(new EnemySearchState(enemy, animator));
                return;
            }

            if (distanceToPlayer < enemy.enemyAttackRange)
            {
                enemy.ChangeState(new EnemyAttackState(enemy, animator));
                return;
            }

            enemy.agent.destination = enemy.player.position;
        }
        else
        {
            enemy.ChangeState(new EnemySearchState(enemy, animator));
            return;
        }
    }
}
