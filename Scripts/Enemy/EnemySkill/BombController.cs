using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
   private Animator anim;
  [SerializeField] private Rigidbody2D rb;
   private CircleCollider2D cd;
   private Enemy_GoblinKing enemy;
   [SerializeField] protected Transform groundCheck;
   [SerializeField] protected float groundCheckDistance;
   [SerializeField] protected LayerMask whatIsGround;
   private void Start()
   {
      anim = GetComponent<Animator>();
      //rb = GetComponent<Rigidbody2D>();
      cd = GetComponent<CircleCollider2D>();
   }

   public void SetUpBomb(float _xVelocity, float _yVelocity,Enemy_GoblinKing _enemy)
   {
      Debug.Log(_xVelocity+" "+ _yVelocity);
      rb.velocity = new Vector2(_xVelocity, _yVelocity);
      enemy = _enemy;
   }

   private void DamageTrigger()
   {
      AudioManager.instance.PlayerSoundEffect(10,enemy.transform);
      Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);
      foreach (var hit in colliders)
      {
         if (hit.GetComponent<Player>() != null)
         {
            enemy.stat.DoDamage(hit.GetComponent<PlayerStat>());
         }
      }
   }
   private void Update()
   {
      if (IsGroundDetected())
      {
         anim.SetTrigger("Exploded");
         rb.velocity=Vector2.zero;
         Destroy(gameObject, 2f);
      }
   }
   public virtual bool IsGroundDetected() =>
      Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGround);
}
