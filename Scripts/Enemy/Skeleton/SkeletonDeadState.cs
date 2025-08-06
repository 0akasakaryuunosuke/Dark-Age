using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeadState : EnemyState
{
    private Enemy_Skeleton enemy;

    public SkeletonDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        enemy.anim.SetBool(enemy.lastAnimBoolName,true);
        enemy.cd.enabled = false;
        stateTimer = .2f;
        enemy.anim.speed = 0;
        enemy.rb.gravityScale = 6;
        enemy.Invoke("DestroyMe",5);
        
        AudioManager.instance.PlayerSoundEffect(6,enemy.transform);
    }

    public override void Update()
    {
        base.Update();
        if(stateTimer>0)
            enemy.SetVelocity(0,10);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
