using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SporeController : MonoBehaviour
{
  private Animator anim;
  private CircleCollider2D cd;
  private Player player;
  private float liftTime;
  private float chasingSpeed;
  private float popSpeed;
  private float lifeTimer=1;
  private float popTimer=1;
  private bool canExplode;
  private Enemy_Mushroom enemy;
  
  public void SetUpSpore(float _lifeTime, float _chasingSpeed,float _popSpeed,Enemy_Mushroom _enemy)
  {
    liftTime = _lifeTime;
    chasingSpeed = _chasingSpeed;
    popSpeed = _popSpeed;
    player=PlayerManager.instance.player;
    lifeTimer = liftTime;
    popTimer = .5f;
    canExplode = false;
    enemy = _enemy;
  }
  private void Start()
  {
    anim = GetComponent<Animator>();
    cd = GetComponent<CircleCollider2D>();
  }
  
  private void Update()
  {
    lifeTimer -= Time.deltaTime;
    popTimer -= Time.deltaTime;
    
    
    if (lifeTimer <= 0)
      canExplode = true;
    if (canExplode)
    {
      anim.SetTrigger("Exploded");
      Invoke(nameof(DestroySelf),2f);
    }
    else
    {
      if (popTimer > 0)
          transform.position=Vector2.MoveTowards(transform.position, transform.position + new Vector3(0, 1), popSpeed * Time.deltaTime);
      else
      {
        transform.position=Vector2.MoveTowards(transform.position, player.transform.position, chasingSpeed * Time.deltaTime);
      }
    }
  }
  private void DestroySelf()=> Destroy(gameObject);
  
  private void DamageTrigger()
  {
    AudioManager.instance.PlayerSoundEffect(14,transform);
    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);
    foreach (var hit in colliders)
    {
      if (hit.GetComponent<Player>() != null)
      {
        enemy.stat.DoMagicalDamage(hit.GetComponent<PlayerStat>());
      }
    }
  }

  private void OnTriggerEnter2D(Collider2D other)
  {
    if (other.GetComponent<Player>()!=null)
      canExplode = true;
  }
}
