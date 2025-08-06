using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDashState : PlayerState
{
    private float dashImageCooldown;
    private float dashImageTimer;
    public PlayerDashState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
        dashImageCooldown = 0.08f;
        dashImageTimer = dashImageCooldown;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = player.dashDuration;
        player.stat.SetInvincible(true);
        if(SkillManager.instance.dash.canCreateCloneBegin)
             player.skill.clone.CreateOnDashStart();
    }

    public override void Update()
    {
        base.Update();
        dashImageTimer -= Time.deltaTime;
        if (dashImageTimer < 0)
        {
            player.fx.CreateDashImage(player.facingDir);
            dashImageTimer = dashImageCooldown;
        }
        
        player.SetVelocity(player.dashSpeed*player.dashDir,0);
        if (stateTimer < 0)
        {
            stateMachine.changeState(player.idleState);
        }

        if (!player.IsGroundDetected()&&player.IsWallDetected())
        {
            stateMachine.changeState(player.wallSlideState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        player.stat.SetInvincible(false);
        player.SetVelocity(player.moveSpeed,rb.velocity.y);
        if(SkillManager.instance.dash.canCreateCloneEnd)
          player.skill.clone.CreateOnDashOver();
    }
}
