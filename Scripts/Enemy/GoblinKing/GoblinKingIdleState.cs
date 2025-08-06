using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinKingIdleState : GoblinKingGroundedState
{
    public GoblinKingIdleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_GoblinKing _enemy) : base(_enemyBase, _stateMachine, _animBoolName, _enemy)
    {
    }
    public override void Enter()
    {
        base.Enter();
        enemy.SetVelocity(0,enemy.rb.velocity.y);
        stateTimer = enemy.idleTime;
    }

    public override void Update()
    {
        base.Update();
        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.moveState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
