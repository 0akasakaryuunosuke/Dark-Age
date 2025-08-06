using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{
    [SerializeField]protected LayerMask whatIsPlayer;
    [Header("眩晕参数")] 
    public float stunnedTime;
    public Vector2 stunnedKnockback;
    [SerializeField] protected GameObject counterImage;
    protected bool canBeStunned;
    [Header("移动参数")]
    public float moveSpeed;
    public float idleTime;
    protected float defaultSpeed;
    [Header("攻击参数")] 
    public float attackDistance;
    public float attackCooldown;
    [HideInInspector]public float lastAttack;
    public float battleTime;
    protected EnemyStateMachine stateMachine;
    public string lastAnimBoolName;

    protected override void Awake()
    {
        base.Awake();
        stateMachine = new EnemyStateMachine();
    }

    protected override void Start()
    {
        base.Start();
        defaultSpeed = moveSpeed;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        stateMachine.currentState.Update();
       
    }

    public override void SlowDownBy(float _percentage, float _duration)
    {
        base.SlowDownBy(_percentage, _duration);
        moveSpeed *= 1 - _percentage;
        Invoke("ReturnNormalSpeed",_duration);
    }

    public override void ReturnNormalSpeed()
    {
        base.ReturnNormalSpeed();
        moveSpeed = defaultSpeed;
    }

    public virtual void OpenCounterAttackWindow()
    {
        canBeStunned = true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow()
    {
        canBeStunned = false;
        counterImage.SetActive(false);
    }

    public virtual bool CanBeStunned()
    {
        if (canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }
        return false;
    }
    public virtual RaycastHit2D IsPlayerDetected() =>
        Physics2D.Raycast(wallCheck.position, Vector2.right * facingDir, 12, whatIsPlayer);

    public virtual void AnimationFinishTrigger() => stateMachine.currentState.AnimationFinishTrigger();
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position,new Vector3(transform.position.x+attackDistance*facingDir,transform.position.y));
    }
    public virtual void FreezeTime(bool isFreeze)
    {
        if (isFreeze)
        {
            moveSpeed = 0 ;
            anim.speed = 0;
        }
        else
        {
            moveSpeed = defaultSpeed ;
            anim.speed = 1;
        }
    }

    public IEnumerator FreezeTimeFor(float _seconds)
    {
        FreezeTime(true);
        yield return new WaitForSeconds(_seconds);
        FreezeTime(false);
    }

    public void DestroyMe() => Destroy(gameObject);
    
    
}
