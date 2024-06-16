using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class EnemyState
{
    protected EnemyController enemy;
    protected Animator animator;

    public EnemyState (EnemyController _enemy, Animator _animator)
    {
        this.enemy = _enemy;
        this.animator = _animator;
    }

    public abstract void OnStateEnter();
    public abstract void OnStateUpdate();
    public abstract void OnStateExit();
}