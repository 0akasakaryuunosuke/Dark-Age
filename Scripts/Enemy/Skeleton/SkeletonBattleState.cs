using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonBattleState : EnemyState
{
    private Enemy_Skeleton enemy;
    private Transform player;

    private int moveDir;
    // Start is called before the first frame update

    public SkeletonBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Skeleton enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        this.enemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        player = PlayerManager.instance.player.transform;
    }

    public override void Update()
    {
        base.Update();
        if(player.GetComponent<PlayerStat>().isDead)
            stateMachine.ChangeState(enemy.moveState);
        if (player.position.x > enemy.transform.position.x)
        {
            moveDir = 1;
        }
        else
        {
            moveDir = -1;
        }
        enemy.SetVelocity(enemy.moveSpeed*1.5f*moveDir,rb.velocity.y);

        if (enemy.IsPlayerDetected())
        {
            stateTimer = enemy.battleTime;
            AudioManager.instance.PlayBattleBGM();
            if (enemy.IsPlayerDetected().distance < enemy.attackDistance)
            {
                if(  CanAttack())
                {
                    stateMachine.ChangeState(enemy.attackState);
                }
                else
                {
                    enemy.ZeroVelocity();
                }
            }
        }
        else
        {
            if (stateTimer < 0||Vector2.Distance(player.transform.position,enemy.transform.position)>10)
            {
                stateMachine.ChangeState(enemy.idleState);
            }
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool CanAttack()
    {
        return Time.time - enemy.lastAttack > enemy.attackCooldown;
    }

 
}
