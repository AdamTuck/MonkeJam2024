using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackState : EnemyState
{
    float distanceToPlayer;

    Health playerHealth;
    float damagePerSecond = 20f;

    public EnemyAttackState(EnemyController _enemy) : base(_enemy) 
    {
        playerHealth = _enemy.player.GetComponentInParent<Health>();
    }

    public override void OnStateEnter()
    {
        Debug.Log("Enemy Attacking");
    }

    public override void OnStateExit()
    {
        Debug.Log("Enemy stops attacking");
    }

    public override void OnStateUpdate()
    {
        Attack();

        if (enemy.player)
        {
            distanceToPlayer = Vector3.Distance(enemy.transform.position, enemy.player.position);

            if (distanceToPlayer > enemy.enemyAttackRange)
            {
                enemy.ChangeState(new EnemyFollowState(enemy));
            }

            enemy.agent.destination = enemy.player.position;
        }
        else
        {
            enemy.ChangeState(new EnemyPatrolState(enemy));
        }
    }

    void Attack ()
    {
        //Debug.Log("Try Attacking... " + playerHealth.name);
        if (playerHealth)
        {
            //Debug.Log("Reducing health: " + damagePerSecond * Time.deltaTime);
            playerHealth.DeductHealth(damagePerSecond * Time.deltaTime);
        }
    }
}
