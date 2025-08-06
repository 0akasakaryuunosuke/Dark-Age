using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public PlayerMoveState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        //AudioManager.instance.PlayerSoundEffect(7,null);
    }

    public override void Update()
    {
        base.Update();
       
        if (xInput == 0||player.isBusy)
        {
            stateMachine.changeState(player.idleState);
            return;
        }
        player.SetVelocity(xInput*player.moveSpeed,rb.velocity.y);
    }

    public override void Exit()
    {
        base.Exit();
        //AudioManager.instance.StopSoundEffect(7);
    }
}
