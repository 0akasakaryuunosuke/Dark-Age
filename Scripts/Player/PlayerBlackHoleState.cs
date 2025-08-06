using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBlackHoleState : PlayerState
{
    private float flyTime;
    private bool skillUsed;
    private float defaultGravity;


    public PlayerBlackHoleState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName, float _flyTime) : base(_player, _stateMachine, _animBoolName)
    {
        this.flyTime = _flyTime;
    }

    public override void Enter()
    {
        base.Enter();
        defaultGravity = rb.gravityScale;
        rb.gravityScale = 0;
        stateTimer = flyTime;
        skillUsed = false;
    }

    public override void Update()
    {
        base.Update();
        if (stateTimer > 0)
        {
            player.SetVelocity(0,10);
        }
        else
        {
            player.ZeroVelocity();
            rb.gravityScale = .1f;
            if (!skillUsed)
            {
                if (player.skill.blackHole.CanUseSkill()) skillUsed = true;
            }
        }
        if(player.skill.blackHole.canExit())player.stateMachine.changeState(player.idleState);
    }

    public override void Exit()
    {
        base.Exit();
        rb.gravityScale = defaultGravity;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
