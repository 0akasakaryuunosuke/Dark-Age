using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerZone : MonoBehaviour
{
   [SerializeField]private Enemy enemy;

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.GetComponent<Player>() != null)
      {
         enemy.GetComponentInChildren<GoblinKingAnimationTrigger>()?.AnimationTrigger();
      }
   }
}
