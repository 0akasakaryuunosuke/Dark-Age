using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
   protected Enemy enemyBase { get; private set; }
   protected EnemyStateMachine stateMachine{ get; private set; }
   protected Rigidbody2D rb;
   private string animBoolName;
   protected float stateTimer;
   protected bool triggerCalled;
   public EnemyState(Enemy _enemyBase, EnemyStateMachine _stateMachine, string _animBoolName)
   {
      enemyBase = _enemyBase;
      stateMachine = _stateMachine;
      animBoolName = _animBoolName;
   }
   public virtual void Enter()
   {
      triggerCalled = false;
      rb = enemyBase.rb;
      enemyBase.anim.SetBool(animBoolName,true);
   }

   public virtual void Update()
   {
      stateTimer -= Time.deltaTime;
   }

   public virtual void Exit()
   {
    
      enemyBase.anim.SetBool(animBoolName, false);
      enemyBase.lastAnimBoolName = animBoolName;
     
   }

   public virtual void AnimationFinishTrigger() => triggerCalled = true;
}
