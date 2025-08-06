using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoblinKingDeadState : EnemyState
{
    private Enemy_GoblinKing enemy;

    public GoblinKingDeadState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName, Enemy_GoblinKing _enemy) : base(_enemyBase, _stateMachine, _animBoolName)
    {
        enemy = _enemy;
    }
    public override void Enter()
    {
        base.Enter();
        //enemy.anim.SetBool(enemy.lastAnimBoolName,true);
       // enemy.cd.enabled = false;
        enemy.Invoke("DestroyMe",5);
        
        AudioManager.instance.PlayerSoundEffect(5,enemy.transform);
        PlayerManager.instance.player.stat.SetInvincible(true);
        GameObject.Find("Canvas").GetComponent<UI>().ShowClearScreen();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
