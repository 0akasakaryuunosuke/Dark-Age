using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinKingAttackState : EnemyState
{
    private Enemy_GoblinKing enemy;
  
    public GoblinKingAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_GoblinKing _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlayerSoundEffect(3,enemy.transform);
    }

    public override void Update()
    {
        base.Update();
        enemy.ZeroVelocity();
        if (triggerCalled)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    
        enemy.lastAttack = Time.time;
    }
}
