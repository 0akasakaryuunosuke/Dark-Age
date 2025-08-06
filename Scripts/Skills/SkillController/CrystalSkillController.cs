using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalSkillController : MonoBehaviour
{
    private Animator anim;
    private float crystalTimer;
    private bool canExplode;
    private bool canMove;
    private float moveSpeed;
    private bool canIncrease = false;
    private float increaseSpeed = 5;
    private Transform closestEnemy;
    private CircleCollider2D cd=>GetComponent<CircleCollider2D>();
    private Player player;
    

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void SetCrystal(float _crystalDuration, bool _canExplode, bool _canMove, float _moveSpeed,Transform _closestEnemy,Player _player)
    {
        player = _player;
        crystalTimer = _crystalDuration;
        canExplode = _canExplode;
        canMove = _canMove;
        moveSpeed = _moveSpeed;
        closestEnemy = _closestEnemy;
    }

    private void Update()
    {
        crystalTimer -= Time.deltaTime;
       
        if (crystalTimer < 0)
        {
            CrystalRelease();
        }

        
        if (canMove&&closestEnemy)
        {
            transform.position =
                Vector2.MoveTowards(transform.position, closestEnemy.position, moveSpeed * Time.deltaTime);
            if (Vector2.Distance(transform.position, closestEnemy.position) < 1)
            {
                canMove = false;
                CrystalRelease();
            }
        }
        if (canIncrease)
        {
            transform.localScale =
                Vector2.Lerp(transform.localScale, new Vector2(2.5f, 2.5f), increaseSpeed * Time.deltaTime);
        }
    }

    public void CrystalRelease()
    {
        if (canExplode)
        {
            canIncrease = true;
            anim.SetTrigger("Explosive");
        }
        else
        {
            DestroySelf();
        }
    }

    public void DestroySelf()=> Destroy(gameObject);

    private void DamageTrigger()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, cd.radius);
        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
               player.stat.DoMagicalDamage(hit.GetComponent<EnemyStat>());
               Inventory.instance.GetEquipmentByType(EquipmentType.Amulet)?.CallEffects(hit.transform);
            }
        }
    }
    
}
