using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{
    public float cooldown;
    protected float cooldownTimer;
    protected Player player;

    protected  virtual void Start()
    {
        player = PlayerManager.instance.player;
        // LoadUnlocked();
       StartCoroutine(DelayWith(0.1f));
    }

    protected virtual void Update()
    {
        cooldownTimer -= Time.deltaTime;
    }

    public virtual bool CanUseSkill()
    {
        if (cooldownTimer < 0)
        {
            UseSkill();
            cooldownTimer = cooldown;
            return true;
        }
        player.fx.CreatePopUpText("wait for cooldown");
        return false;
    }
    

    public bool CheckForCooldown()
    {
        if (cooldownTimer < 0)
        {
            cooldownTimer = cooldown;
            return true;
        }
        return false;
    }
    public virtual void UseSkill()
    {
        
    }

    protected  virtual  void LoadUnlocked()
    {
    }
    public virtual Transform FindClosest(Transform _transform)
    {
        Transform closestEnemy = null;
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(_transform.position, 10);
        float closestDistance = Mathf.Infinity;
        foreach (var hit in collider2Ds)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(_transform.position, hit.transform.position);
                if (distanceToEnemy < closestDistance)
                {
                    closestEnemy = hit.transform;
                    closestDistance = distanceToEnemy;
                }
            }
        }
        return closestEnemy;
    }

    private IEnumerator DelayWith(float _seconds)
    {
        yield return new WaitForSeconds(_seconds);
        LoadUnlocked();
    }
}
