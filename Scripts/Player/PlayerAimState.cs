using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAimState : PlayerState
{
    public PlayerAimState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        player.skill.sword.SetDotsActive(true);
    }

    public override void Update()
    {
        base.Update();
        player.ZeroVelocity();
        if(Input.GetKeyUp(KeyCode.F))
        {
            stateMachine.changeState(player.idleState);
        }
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        if(mousePosition.x>player.transform.position.x&&player.facingDir==-1)player.Flip();
        else if(mousePosition.x<player.transform.position.x&&player.facingDir==1)player.Flip();
    }

    public override void Exit()
    {
        base.Exit();
        player.StartCoroutine("BusyFor", .2f);
        player.skill.sword.SetDotsActive(false);
    }
}
