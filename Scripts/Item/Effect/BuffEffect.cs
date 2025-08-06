using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Buff Effect",menuName = "Data/Effect/Buff Effect")]
public class BuffEffect : ItemEffect
{
   private PlayerStat playerStat;
   [SerializeField] private int buffAmount;
   [SerializeField] private float buffDuration;
   [SerializeField] private StatType statType;
   public override void ExecuteEffect(Transform _transform)
   {
      playerStat = PlayerManager.instance.player.GetComponent<PlayerStat>();
      playerStat.IncreaseStatByAmount(buffAmount,buffDuration,playerStat.GetStat(statType));
   }

 
}
