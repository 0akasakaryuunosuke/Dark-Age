using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Entity : MonoBehaviour
{
    public Rigidbody2D rb { get; private set; }
    public CapsuleCollider2D cd { get; private set; }
    public Animator anim { get; private set; }
    public EntityFX fx { get; private set; }
    public int facingDir{ get; private set; } = 1;
    public EntityStat stat { get; private set; }
    protected bool facingRight = true;
    [Header("碰撞参数")] 
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;
    public Transform attackCheck;
    public float attackRadius;
    protected bool isWallDetected;
    protected bool isGrounded ;

    [FormerlySerializedAs("knockbackDirection")]
    [Header("击退参数")] 
    [SerializeField] protected Vector2 knockbackForce;
    [SerializeField] protected float knockbackDuration;
    protected bool isKnocked;
    public int knockbackDir=0;
    
    public System.Action onFlip;
    // Start is called before the first frame update
    protected  virtual void Awake()
    {
        
    }

    protected virtual void Start()
    {
        rb = GetComponentInChildren<Rigidbody2D>();
        cd = GetComponentInChildren<CapsuleCollider2D>();
        anim = GetComponentInChildren<Animator>();
        fx = GetComponentInChildren<EntityFX>();
        stat = GetComponent<EntityStat>();
        if (wallCheck == null) wallCheck = transform;
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        //CollisionChecks();
    }

    public virtual void DamageEffect()
    {
        fx.StartCoroutine("FlashFX");
        StartCoroutine("hitKnockback");
    }

    public virtual void SlowDownBy(float _percentage, float _duration)
    {
        anim.speed *= 1 - _percentage;
    }

    public virtual void ReturnNormalSpeed()
    {
        anim.speed = 1;
    }

    public void KnockbackFrom(Transform _transform)
    {
        if (transform.position.x > _transform.position.x)
            knockbackDir = 1;
        else if (transform.position.x < _transform.position.x)
            knockbackDir = -1;
    }
    protected virtual IEnumerator hitKnockback()
    {
        isKnocked = true;
        rb.velocity = new Vector2(knockbackForce.x * knockbackDir, knockbackForce.y);
        yield return new WaitForSeconds(knockbackDuration);
        isKnocked = false;
        ZeroVelocity();
    }

    public virtual void Die()
    {
        
    }
    #region 翻转控制

    public void Flip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0,180,0);
        onFlip();
    }

    public void FlipController(float _x)
    {
        if (_x >0 && !facingRight)
        {
            Flip();
        }
        else if(_x < 0 && facingRight)
        {
            Flip();
        }
    }
   
    #endregion
    #region 碰撞相关
    public virtual bool IsGroundDetected() =>
        Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
    public virtual bool IsWallDetected() =>
        Physics2D.Raycast(wallCheck.position, Vector2.right, wallCheckDistance*facingDir, whatIsGround);
    protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position,new Vector3(groundCheck.position.x,groundCheck.position.y-groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,new Vector3(wallCheck.position.x + wallCheckDistance*facingDir,wallCheck.position.y) );
        Gizmos.DrawWireSphere(attackCheck.position,attackRadius);
    }
    #endregion
    #region 速度控制
    public void ZeroVelocity() => SetVelocity(0, 0);
    public void SetVelocity(float xVelocity, float yVelocity)
    {
        if (isKnocked) return;
        FlipController(xVelocity);
        rb.velocity = new Vector2(xVelocity, yVelocity);
    }
   
    #endregion

}