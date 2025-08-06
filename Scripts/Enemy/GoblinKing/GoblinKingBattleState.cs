using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinKingBattleState : EnemyState
{
    private Enemy_GoblinKing enemy;
    private Transform player;
    private int moveDir;
    public GoblinKingBattleState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_GoblinKing _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
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
        enemy.SetVelocity(enemy.moveSpeed*moveDir,rb.velocity.y);

        if (enemy.GetDistance()<25)
        {
            stateTimer = enemy.battleTime;
            AudioManager.instance.PlayBattleBGM();
             if (CanBomb())
            {
                enemy.anim.SetBool("Idle", false);
                stateMachine.ChangeState(enemy.bombThrowState);
                return;
            }
            if (enemy.GetDistance() < enemy.attackDistance)
            {
                if(enemy.anim.GetBool("Move"))
                {
                    enemy.anim.SetBool("Move", false);
                    enemy.anim.SetBool("Idle", true);
                }
                
                if(CanAttack())
                {
                    if(enemy.anim.GetBool("Idle")){
                        enemy.anim.SetBool("Idle", false);
                    }
                    stateMachine.ChangeState(enemy.attackState);
                }
              
                else
                {
                    enemy.ZeroVelocity();
                }
            }
            else
            {
                if (!enemy.anim.GetBool("Move"))
                {
                    enemy.anim.SetBool("Move", true);
                    enemy.anim.SetBool("Idle", false);
                }
            }
        }
        else
        {
            if (stateTimer < 0||Vector2.Distance(player.transform.position,enemy.transform.position)>30)
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

    private bool CanBomb()
    {
        return Time.time - enemy.lastBombAttack > enemy.bombCooldown && enemy.canThrowBomb;
    }

}
