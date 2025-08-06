using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightingAttackController : MonoBehaviour
{
   
   protected virtual void OnTriggerEnter2D(Collider2D other)
   {
      if (other.GetComponent<Enemy>() != null)
      {
         PlayerStat playerStat = PlayerManager.instance.player.GetComponent<PlayerStat>();
         playerStat.DoMagicalDamage(other.GetComponent<EnemyStat>());
      }
   }
}
