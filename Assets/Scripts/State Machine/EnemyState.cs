using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    protected EnemyBase enemy;
    protected EnemyStateMachine enemyStateMachine;
    
    public EnemyState(EnemyBase Enemy, EnemyStateMachine EnemyStateMachine)
    {
        enemy = Enemy;
        enemyStateMachine = EnemyStateMachine;
    }

    public virtual void EnterState() {}
    public virtual void ExitState() {}
    public virtual void UpdateState() {}
    public virtual void PhysicsUpdate() {}
    public virtual void AnimationTriggerEvent() {}

}
