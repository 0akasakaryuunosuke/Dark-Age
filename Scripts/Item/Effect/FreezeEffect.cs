using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "FreezeAround",menuName = "Data/Effect/FreezeAround")]
public class FreezeEffect : ItemEffect
{
  [SerializeField] private float duration;
  [SerializeField] private int radius;
  [Range(0,1f)][SerializeField] private float condition;
  
  public override void ExecuteEffect(Transform _transform)
  {
    PlayerStat player = PlayerManager.instance.player.GetComponent<PlayerStat>();
    if (player.currentHP > condition * player.maxHP.GetValue())
      return;
    if (!Inventory.instance.CanUseArmon())
      return;
    
    Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(_transform.position, radius);
    foreach (var collider in collider2Ds)
    {
      if (collider.GetComponent<Enemy>() != null)
      {
        Enemy enemy = collider.GetComponent<Enemy>();
        enemy.StartCoroutine(enemy.FreezeTimeFor(duration));
      }
    }
  }
}
