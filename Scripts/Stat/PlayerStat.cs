using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum StatType
{
    Strength,
    Agility,
    Intelligence,
    Vitality,
    Damage,
    CriticalChance,
    CriticalPower,
    MaxHp,
    Evasion,
    Armor,
    MagicalArmor,
    FireDamage,
    IceDamage,
    LightingDamage
}
public class PlayerStat : EntityStat
{
    private Player player;
    
    protected override void Start()
    {
        base.Start();
        criticalPower.SetDefaultValue(150);
        player=GetComponent<Player>();
    }

    public override void TakeDamage(int _damage)
    {
        base.TakeDamage(_damage);
        if (_damage > maxHP.GetValue() * 0.3f)
        {
            player.fx.ScreenShake(_damage*1f/ maxHP.GetValue());
        }
        if (isDead)
            return;
        player.DamageEffect();
    }

    public override void Die()
    {
        base.Die();
        player.Die();
        GetComponent<PlayerItemDropController>().GenerateDrop();
    }

    public override void DecreaseHP(int _damage)
    {
        base.DecreaseHP(_damage);
        Inventory.instance.GetEquipmentByType(EquipmentType.Armon)?.CallEffects(player.transform);
    }
    
    public Stat GetStat(StatType _type)
    {
        return _type switch
        {
            StatType.Strength => strength,
            StatType.Agility => agility,
            StatType.Intelligence => intelligence,
            StatType.Vitality => vitality,
            StatType.Damage => damage,
            StatType.CriticalChance =>criticalChance,
            StatType.CriticalPower =>criticalPower,
            StatType.MaxHp => maxHP,
            StatType.Evasion => evasion,
            StatType.Armor => armor,
            StatType.MagicalArmor => magicalArmor,
            StatType.FireDamage => fireDamage,
            StatType.IceDamage => iceDamage,
            _ => _type == StatType.LightingDamage ? lightingDamage : null
        };
    }
}
