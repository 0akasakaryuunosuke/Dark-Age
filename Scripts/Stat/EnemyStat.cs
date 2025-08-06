using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStat : EntityStat
{
    private Enemy enemy;
    protected ItemDropController dropController;
    
    [Header("等级细节")]
    public int level;
    [Range(0f, 1f)] [SerializeField] private float levelPercentage;

    [SerializeField] private Stat dropCurrency;
    private void ApplyLevelModifier()
    {
        LevelModify(strength);
        LevelModify(agility);
        LevelModify(intelligence);
        LevelModify(vitality);
        LevelModify(damage);
        LevelModify(criticalChance);
        LevelModify(criticalPower);
        LevelModify(maxHP);
        LevelModify(evasion);
        LevelModify(armor);
        LevelModify(magicalArmor);
    }

    private void LevelModify(Stat _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * levelPercentage;
            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
        
    }
    protected override void Start()
    {
        dropCurrency.SetDefaultValue(100);
        ApplyLevelModifier();
        base.Start();
        enemy = GetComponent<Enemy>();
        dropController = GetComponent<ItemDropController>();
        
    }
    

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        enemy.DamageEffect();
    }

    public override void Die()
    {
        base.Die();
        enemy.Die();
        dropController.GenerateDrop();
        PlayerManager.instance.currency += dropCurrency.GetValue();
    }
}
