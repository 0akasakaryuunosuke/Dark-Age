using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Player : Entity
{
   public bool isBusy { get; private set; }

   [Header("移动参数")]
   public float moveSpeed ;
   public float jumpForce ;
   public float swordCatchForce;
   private float defaultMoveSpeed;
   private float defaultJumpForce;
   [Header("冲刺参数")] 
   [SerializeField] private float dashCooldown;
   private float dashCooldownTimer;
   public float dashSpeed;
   public float dashDuration;
   private float defaultDashSpeed;
   public float dashDir { get; private set; }
   [Header("攻击参数")] 
   [SerializeField] public Vector2[] attackMovement;
   public float counterAttackDuration;
   //[SerializeField] protected LayerMask whatIsWall;
   [Header("黑洞参数")] 
   [SerializeField] private float flyTime;
   #region 全局

   public  SkillManager skill{ get; private set; }
   public GameObject sword{ get; private set; }
   
   #endregion
   #region 状态
   public PlayerStateMachine stateMachine { get; private set; }
   public PlayerIdleState idleState{ get; private set; }
   public PlayerMoveState moveState{ get; private set; }
   public PlayerJumpState jumpState{ get; private set; }
   public PlayerAirState  airState { get; private set; }
   public PlayerDashState  dashState { get; private set; }
   public PlayerWallSlideState  wallSlideState { get; private set; }
   public PlayerWallJumpState  wallJumpState  { get; private set; }
   
   public PlayerPrimaryAttackState primaryAttackState { get; private set; }
   public PlayerCounterAttackState counterAttackState { get; private set; }
   public PlayerAimState aimSword { get; private set; }
   public PlayerCatchSwordState catchSword { get; private set; }
   public PlayerBlackHoleState blackHole { get; private set; }
   public PlayerDeadState deadState { get; private set; }
   
   #endregion
   protected  override void Awake()
   {
      base.Awake();
      stateMachine = new PlayerStateMachine();
      idleState = new PlayerIdleState(this,stateMachine,"Idle");
      moveState = new PlayerMoveState(this, stateMachine, "Move");
      jumpState = new PlayerJumpState(this, stateMachine, "Jump");
      airState = new PlayerAirState(this, stateMachine, "Jump");
      dashState = new PlayerDashState(this, stateMachine, "Dash");
      wallSlideState = new PlayerWallSlideState(this, stateMachine, "WallSlide");
      wallJumpState = new PlayerWallJumpState(this, stateMachine, "Jump");
      primaryAttackState = new PlayerPrimaryAttackState(this, stateMachine, "Attack");
      counterAttackState = new PlayerCounterAttackState(this, stateMachine, "CounterAttack");
      aimSword = new PlayerAimState(this, stateMachine, "AimSword");
      catchSword = new PlayerCatchSwordState(this, stateMachine, "CatchSword");
      blackHole = new PlayerBlackHoleState(this, stateMachine, "Jump",flyTime);
      deadState = new PlayerDeadState(this, stateMachine, "Die");

   }
   
   protected  override void Start()
   {
      base.Start();
      stateMachine.Initialize(idleState);
      skill = SkillManager.instance;
      defaultMoveSpeed = moveSpeed;
      defaultJumpForce = jumpForce;
      defaultDashSpeed = dashSpeed;
   }

   protected  override void Update()
   {
      if (Time.timeScale == 0)
         return;
      base.Update();
      stateMachine.currentState.Update();
      CheckDashInput();
      if (Input.GetKeyDown(KeyCode.Q))
      {
         skill.crystal.UseSkill();
      }
   }

   public override void SlowDownBy(float _percentage, float _duration)
   {
      base.SlowDownBy(_percentage, _duration);
      moveSpeed *= 1 - _percentage;
      jumpForce *= 1 - _percentage;
      dashSpeed *= 1 - _percentage;
      Invoke("ReturnNormalSpeed",_duration);
   }

   public override void ReturnNormalSpeed()
   {
      base.ReturnNormalSpeed();
      moveSpeed = defaultMoveSpeed;
      jumpForce = defaultJumpForce;
      dashSpeed = defaultDashSpeed;
   }

   public IEnumerator BusyFor(float _seconds)
   {
      isBusy = true;
      yield return new WaitForSeconds(_seconds);
      isBusy = false;

   }

   public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();
   private void CheckDashInput()
   {
      if (IsWallDetected()) return;
      dashCooldownTimer -= Time.deltaTime;
      if (Input.GetKeyDown(KeyCode.LeftShift)&&dashCooldownTimer<0&&SkillManager.instance.dash.canUseDash)
      {
         dashCooldownTimer = dashCooldown;
         dashDir = Input.GetAxisRaw("Horizontal")!=0?Input.GetAxisRaw("Horizontal"):facingDir;
         stateMachine.changeState(dashState);
      }
      else if(Input.GetKeyDown(KeyCode.LeftShift)&&dashCooldownTimer>0&&SkillManager.instance.dash.canUseDash)
         fx.CreatePopUpText("wait for cooldown");
   }


   public void AssignSword(GameObject _newSword)
   {
      sword = _newSword;
   }

   public void CatchTheSword()
   {
      stateMachine.changeState(catchSword);
      Destroy(sword);
   }

   public override void Die()
   {
      base.Die();
      long lostCurrency = Mathf.RoundToInt(PlayerManager.instance.currency * 0.3f);
      PlayerManager.instance.currency -= lostCurrency;
      GameManager.instance.SetDeadBody(transform.position.x, transform.position.y, lostCurrency);
      GameManager.instance.canPause = false;
      SaveManager.instance.SaveData();
      stateMachine.changeState(deadState);
   }
}
