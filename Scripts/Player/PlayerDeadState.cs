using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeadState : PlayerState
{
    public PlayerDeadState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        GameManager.instance.canPause = false;
        GameObject.Find("Canvas").GetComponent<UI>().ShowDeathScreen();
    }

    public override void Update()
    {
        base.Update();
        player.ZeroVelocity();
    }

    public override void Exit()
    {
        base.Exit();
      
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }
}
