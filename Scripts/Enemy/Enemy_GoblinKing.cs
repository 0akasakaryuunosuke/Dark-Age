using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_GoblinKing : Enemy
{

    public Player player;
    public bool canThrowBomb;
    public float bombCooldown;
    public GameObject bombPrefab;
    [HideInInspector]public float lastBombAttack;
    #region 状态
    public GoblinKingIdleState idleState { get; private set; }
    public GoblinKingMoveState moveState{ get; private set; }
    public GoblinKingBattleState battleState{ get; private set; }
    public GoblinKingAttackState attackState { get; private set; }
    public GoblinKingDeadState deadState { get; private set; }
    public GoblinKingBombThrowState bombThrowState { get; private set; }
    #endregion
    protected override void Awake()
    {
        base.Awake();
        idleState = new GoblinKingIdleState(this,stateMachine,"Idle",this);
        moveState = new GoblinKingMoveState(this, stateMachine, "Move", this);
        battleState = new GoblinKingBattleState(this, stateMachine, "Move", this);
        attackState = new GoblinKingAttackState(this, stateMachine, "Attack", this);
        deadState = new GoblinKingDeadState(this, stateMachine, "Dead", this);
        bombThrowState = new GoblinKingBombThrowState(this, stateMachine, "Bomb", this);
    }

    protected override void Update()
    {
        base.Update();
        if (stat.currentHP < stat.maxHP.GetValue() * 0.6f)
            canThrowBomb = true;
    }

    protected override void Start()
    {
        base.Start();
        stateMachine.Initialize(idleState);
        player = PlayerManager.instance.player;
    }

 
    public override void Die()
    {
        base.Die();
        stateMachine.ChangeState(deadState);
    }

    public float GetDistance()
    {
        return Vector2.Distance(player.transform.position, transform.position);
    }
}
