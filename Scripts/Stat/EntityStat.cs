using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class EntityStat : MonoBehaviour
{
    private EntityFX fx;
    [SerializeField] private GameObject thunderStrikePrefab;
    [Header("主要属性")]
    public Stat strength;
    public Stat agility;
    public Stat intelligence;
    public Stat vitality;
    
    [Header("攻击属性")]
    public Stat damage;
    public Stat criticalChance;
    public Stat criticalPower;
    [Header("防御属性")]
    public Stat maxHP;
    public Stat evasion;
    public Stat armor;
    public Stat magicalArmor;
    [Header("魔法属性")] 
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;
    public int thunderDamage;
    
    [Header("疾病参数")] 
    public float ailmentDuration;
    public bool isIgnited;
    public bool isChilled;
    public bool isShocked;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;
    private float ignitedDamageTimer;
    private float ignitedDamageCooldown;
    
    
    
    public  int currentHP;
    public System.Action onHealthUpdate;
    public bool isDead { get; private set; }
    public bool isInvincible { get; private set; }
    protected virtual void Start()
    {
        currentHP = maxHP.GetValue();
        fx = GetComponent<EntityFX>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        ignitedTimer -= Time.deltaTime;
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        ignitedDamageTimer -= Time.deltaTime;
        
        if (ignitedTimer < 0) isIgnited = false;
        if (chilledTimer < 0) isChilled = false;
        if (shockedTimer < 0) isShocked = false;

        if (ignitedDamageTimer < 0 && isIgnited)
        {
            ignitedDamageTimer = ignitedDamageCooldown;
            DecreaseHP(1+Mathf.RoundToInt(fireDamage.GetValue() * .2f));
            Debug.Log("get ignited damage");
            if(currentHP<0&&!isDead)
            {
                isIgnited = false;
                Die();
            }
        }
    }

    public virtual void DoDamage(EntityStat _stat)
    {
        if (EvasionCheck(_stat)) return;
        
        int totalDamage = damage.GetValue() + strength.GetValue();
        totalDamage = CriticalCheck(totalDamage,_stat);
        totalDamage = _stat.ArmorCheck(totalDamage);
        
        _stat.GetComponent<Entity>().KnockbackFrom(transform);
        _stat.TakeDamage(totalDamage);
    }

    public void  ApplyAilment(bool _chilled,bool _ignited,bool _shocked)
    {
        
        bool canIgnite = !isIgnited && !isChilled && !isShocked;
        bool canChill = !isIgnited && !isChilled && !isShocked;
        bool canShock = !isIgnited && !isChilled;
        if (_chilled&&canChill)
        {
            isChilled = _chilled;
            chilledTimer = ailmentDuration;
            fx.ChillFor(ailmentDuration);
            GetComponent<Entity>().SlowDownBy(0.25f,ailmentDuration);
        }

        if (_ignited&&canIgnite)
        {
            isIgnited = _ignited;
            ignitedTimer = ailmentDuration;
            ignitedDamageCooldown = .3f;
            fx.IgniteFor(ailmentDuration);
        } 
        if(_shocked&&canShock)
        {
            if (!isShocked)
            {
                isShocked = _shocked;
                Debug.Log("im shocked");
                shockedTimer = ailmentDuration;
                fx.ShockFor(ailmentDuration);
            }
            else
            {
                ThunderStrikeLogic();
            }
        }
    }

    private void ThunderStrikeLogic()
    {
        if (GetComponent<Player>() != null) return;
        Transform closestEnemy = null;
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, 20);
        float closestDistance = Mathf.Infinity;
        foreach (var hit in collider2Ds)
        {
            if (hit.GetComponent<Enemy>() != null)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);
                
                if (distanceToEnemy < closestDistance&&distanceToEnemy>1)
                {
                    closestEnemy = hit.transform;
                    closestDistance = distanceToEnemy;
                }
            }
        }
        if (closestEnemy == null) closestEnemy = transform;
        if (closestEnemy != null)
        {
            GameObject thunderStrike= Instantiate(thunderStrikePrefab, transform.position, Quaternion.identity);
            thunderStrike.GetComponent<ThunderStrikeController>().SetThunderStrike(closestEnemy,thunderDamage);
        }
    }

    public virtual void DoMagicalDamage(EntityStat _stat)
    {
        float _fireDamage = fireDamage.GetValue() *( 1+intelligence.GetValue()*0.01f);
        float _iceDamage = iceDamage.GetValue() *( 1+intelligence.GetValue()*.01f);
        float _lightingDamage = lightingDamage.GetValue() *( 1+intelligence.GetValue()*.01f);
        float totalMagicalDamage = _iceDamage + _fireDamage + _lightingDamage;
        totalMagicalDamage -= _stat.magicalArmor.GetValue() * (1 + intelligence.GetValue()*.01f);
        totalMagicalDamage = Mathf.Clamp(0, totalMagicalDamage, int.MaxValue);
        int resultDamage = Mathf.RoundToInt(totalMagicalDamage);
        Debug.Log("魔法伤害:"+resultDamage);
        
        if (resultDamage == 0) return;
        _stat.GetComponent<Entity>().KnockbackFrom(transform);
        _stat.TakeDamage(resultDamage);

        bool canIgnited = _fireDamage >= _iceDamage && _fireDamage >= _lightingDamage;
        bool canChilled = _iceDamage >= _lightingDamage && _iceDamage >= _fireDamage;
        bool canShocked = _lightingDamage >= _fireDamage && _lightingDamage >= _iceDamage;
  
        _stat.ApplyAilment(canChilled,canIgnited,canShocked);
        
    }
    private int CriticalCheck(int totalDamage,EntityStat _stat)
    {
        int totalCriticalChance = criticalChance.GetValue() + vitality.GetValue();
        if (Random.Range(0, 100) < totalCriticalChance)
        {
            float totalCriticalPower = (criticalPower.GetValue() + strength.GetValue()) * .01f;
            totalDamage = Mathf.RoundToInt(totalDamage * totalCriticalPower);
            fx.CreateCriticalFX(_stat.transform,GetComponent<Entity>().facingDir);
        }

        return totalDamage;
    }

    private int ArmorCheck( int totalDamage)
    {
        if (isChilled)
            totalDamage -= Mathf.RoundToInt(armor.GetValue() * 0.8f);
        else 
            totalDamage -= armor.GetValue();
        totalDamage = Mathf.Clamp(0, totalDamage, int.MaxValue);
        return totalDamage;
    }

    private bool EvasionCheck(EntityStat _stat)
    {
        int totalEvasion = _stat.evasion.GetValue() + _stat.agility.GetValue();
        if (_stat.isShocked)
            totalEvasion += 20;
        if (Random.Range(0, 100) < totalEvasion)
        {
            Debug.Log("I miss the attack");
            return true;
        }
        return false;
    }

    public virtual void TakeDamage(int _damage)
    {
        if (isInvincible)
            return;
        DecreaseHP(_damage);
        fx.CreatePopUpText("take damage:"+_damage);
        if (currentHP <= 0&&!isDead) 
            Die();
    }

    public virtual void Die()
    {
        isDead = true;
    }

    public virtual void DecreaseHP(int _damage)
    {
        currentHP -= _damage;
        onHealthUpdate();
    }

    public virtual void IncreaseHPByAmount(int amount)
    {
        currentHP = Mathf.Min(maxHP.GetValue(), currentHP + amount);
        onHealthUpdate();
    }
    public virtual void IncreaseHPByPercentage(float percentage)
    {
        currentHP = (int)Mathf.Min(maxHP.GetValue(), currentHP * (1 + percentage));
        onHealthUpdate();
    }

    public virtual void IncreaseStatByAmount(int amount, float duration, Stat _stat)
    {

        StartCoroutine(StatToModify(amount, duration, _stat));
    }

    private IEnumerator StatToModify(int amount, float duration, Stat _stat)
    {
        _stat.AddModifier(amount);
        Debug.Log(_stat.GetValue());
        yield return new WaitForSeconds(duration);
        _stat.RemoveModifier(amount);
        Debug.Log(_stat.GetValue());
    }
    public void  SetInvincible(bool _invincible)=> isInvincible = _invincible;
    
}
