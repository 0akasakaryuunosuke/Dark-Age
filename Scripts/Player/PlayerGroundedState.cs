using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerGroundedState : PlayerState
{
    // Start is called before the first frame update
    public PlayerGroundedState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Mouse1)&&SkillManager.instance.parry.canParry&&SkillManager.instance.parry.CheckForCooldown())
        {
            stateMachine.changeState(player.counterAttackState);
        }
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            stateMachine.changeState(player.primaryAttackState);
        }
        if (!player.IsGroundDetected())
        {
            stateMachine.changeState(player.airState);
        }
        if (Input.GetKeyDown(KeyCode.Space)&& player.IsGroundDetected())
        {
            stateMachine.changeState(player.jumpState);
        }

        if (Input.GetKeyDown(KeyCode.F)&&HasNoSword()&&SkillManager.instance.sword.canThrow)
        {
            stateMachine.changeState(player.aimSword);
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            stateMachine.changeState(player.blackHole);
        }
    }

    public override void Exit()
    {
        base.Exit();
    }

    private bool HasNoSword()
    {
        if (!player.sword)
        {
            return true;
        }
        player.sword.GetComponent<SwordSkillController>().ReturnSword();
        return false;
    }
}
