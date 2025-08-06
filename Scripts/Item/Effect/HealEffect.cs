using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Heal Effect",menuName = "Data/Effect/HealEffect")]
public class HealEffect : ItemEffect
{
   [SerializeField] private int healAmount;
   [Range(0,1f)]
   [SerializeField] private float healPercentage;

   public override void ExecuteEffect(Transform _transform)
   {
    PlayerStat playerStat=PlayerManager.instance.player.GetComponent<PlayerStat>();
    playerStat.IncreaseHPByAmount(healAmount);
    playerStat.IncreaseHPByPercentage(healPercentage);
   }
}
