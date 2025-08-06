using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public enum EquipmentType
{
    Weapon,
    Armon,
    Amulet,
    Flask
}

[CreateAssetMenu(fileName = "New Equipment",menuName = "Data/Equipment")]
public class ItemData_Equipment : ItemData
{
    public EquipmentType equipmentType;
    
    [Header("主要属性")]
    public int strength;
    public int agility;
    public int intelligence;
    public int vitality;
    
    [Header("攻击属性")]
    public int damage;
    public int criticalChance;
    public int criticalPower;
    [Header("防御属性")]
    public int maxHP;
    public int evasion;
    public int armor;
    public int magicalArmor;

    [Header("魔法属性")] 
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;
    
    [Header("其他细节")]
    public List<InventoryItem> craftMaterials;
    public ItemEffect[] itemEffects;
    public float itemCooldown;
    
    public void AddToModifiers()
    {
        PlayerStat playerStat = PlayerManager.instance.player.GetComponent<PlayerStat>();
        playerStat.strength.AddModifier(strength);
        playerStat.agility.AddModifier(agility);
        playerStat.intelligence.AddModifier(intelligence);
        playerStat.vitality.AddModifier(vitality);
        playerStat.damage.AddModifier(damage);
        playerStat.criticalChance.AddModifier(criticalChance);
        playerStat.criticalPower.AddModifier(criticalPower);
        playerStat.maxHP.AddModifier(maxHP); 
        playerStat.evasion.AddModifier(evasion);
        playerStat.armor.AddModifier(armor);
        playerStat.magicalArmor.AddModifier(magicalArmor);
        playerStat.fireDamage.AddModifier(fireDamage);
        playerStat.iceDamage.AddModifier(iceDamage);
        playerStat.lightingDamage.AddModifier(lightingDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStat playerStat = PlayerManager.instance.player.GetComponent<PlayerStat>();
        playerStat.strength.RemoveModifier(strength);
        playerStat.agility.RemoveModifier(agility);
        playerStat.intelligence.RemoveModifier(intelligence);
        playerStat.vitality.RemoveModifier(vitality);
        playerStat.damage.RemoveModifier(damage);
        playerStat.criticalChance.RemoveModifier(criticalChance);
        playerStat.criticalPower.RemoveModifier(criticalPower);
        playerStat.maxHP.RemoveModifier(maxHP); 
        playerStat.evasion.RemoveModifier(evasion);
        playerStat.armor.RemoveModifier(armor);
        playerStat.magicalArmor.RemoveModifier(magicalArmor);
        playerStat.fireDamage.RemoveModifier(fireDamage);
        playerStat.iceDamage.RemoveModifier(iceDamage);
        playerStat.lightingDamage.RemoveModifier(lightingDamage);
    }

    public void CallEffects(Transform _transform)
    {
        foreach (var effect in itemEffects)
        {
            effect.ExecuteEffect(_transform);
        }
    }

    public override StringBuilder GetDescription()
    {
        base.GetDescription();
        description.Append(AppendStat(strength, "strength"));
        description.Append(AppendStat(agility, "agility"));
        description.Append(AppendStat(intelligence, "intelligence"));
        description.Append(AppendStat(vitality, "vitality"));
        description.Append(AppendStat(damage, "damage"));
        description.Append(AppendStat(criticalChance, "criticalChance"));
        description.Append(AppendStat(criticalPower, "criticalPower"));
        description.Append(AppendStat(maxHP, "maxHP"));
        description.Append(AppendStat(evasion, "evasion"));
        description.Append(AppendStat(armor, "armor"));
        description.Append(AppendStat(magicalArmor, "magicalArmor"));
        description.Append(AppendStat(fireDamage, "fireDamage"));
        description.Append(AppendStat(iceDamage, "iceDamage"));
        description.Append(AppendStat(lightingDamage, "lightingDamage"));
        return description;
    }

    private string AppendStat(int _value, string _stat)
    {
        if (_value > 0)
            return _stat + ":" + _value+"\n";
        return "";
    }
}
