using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomStunnedState : EnemyState
{
    private Enemy_Mushroom enemy;
    public MushroomStunnedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Mushroom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        stateTimer = enemy.stunnedTime;
        enemy.fx.InvokeRepeating("RedColorBlink",0,.1f);
        rb.velocity= new Vector2(-enemy.facingDir*enemy.stunnedKnockback.x,enemy.stunnedKnockback.y);
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer < .5f)
        {
            rb.velocity =Vector2.zero;
        }
        if (stateTimer < 0)
        {
            stateMachine.ChangeState(enemy.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        enemy.fx.Invoke("CancelColorBlink",0);
    }
}
