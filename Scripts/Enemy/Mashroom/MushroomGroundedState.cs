using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MushroomGroundedState : EnemyState
{
    protected Enemy_Mushroom enemy;
    protected Transform player;
    public MushroomGroundedState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_Mushroom enemy) : base(_enemyBase, _stateMachine, _animBoolName)
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
        if ((enemy.IsPlayerDetected() || Vector2.Distance(player.transform.position, enemy.transform.position) < 4)
            &&!PlayerManager.instance.player.GetComponent<PlayerStat>().isDead)
        {
            stateMachine.ChangeState(enemy.battleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }
}
