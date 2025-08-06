using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomSporeAttackState : EnemyState
{
    private Enemy_Mushroom enemy;
    public MushroomSporeAttackState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName,Enemy_Mushroom _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        AudioManager.instance.PlayerSoundEffect(13,enemy.transform);
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
        enemy.lastSporeAttack = Time.time;
    }
}
