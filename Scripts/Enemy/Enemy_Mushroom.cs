using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Mushroom : Enemy
{
    public float sporeDistance;
    public float sporeCooldown;
    public GameObject sporePrefab;
    [HideInInspector]public float lastSporeAttack;
     public float lifeTime;
     public float chasingSpeed;
     public float popSpeed;

    #region 状态
    public MushroomIdleState idleState { get; private set; }
    public MushroomMoveState moveState{ get; private set; }
    public MushroomBattleState battleState{ get; private set; }
    public MushroomAttackState attackState { get; private set; }
    public MushroomStunnedState StunnedState  { get; private set; }
    public MushroomDeadState deadState { get; private set; }
    public MushroomSporeAttackState sporeState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new MushroomIdleState(this,stateMachine,"Idle",this);
        moveState = new MushroomMoveState(this, stateMachine, "Move", this);
        battleState = new MushroomBattleState(this, stateMachine, "Move", this);
        attackState = new MushroomAttackState(this, stateMachine, "Attack", this);
        StunnedState = new MushroomStunnedState(this, stateMachine, "Stunned", this);
        deadState = new MushroomDeadState(this, stateMachine, "Idle", this);
        sporeState = new MushroomSporeAttackState(this, stateMachine, "SporeAttack", this);
    }

    protected override void Update()
    {
        base.Update();
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
    }

    public override bool CanBeStunned()
    {
        if (base.CanBeStunned())
        {
            stateMachine.ChangeState(StunnedState);
            return true;
        }
        return false;
    }

    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

  
}
