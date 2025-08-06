using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadBody : MonoBehaviour
{
   public long currency;

   private void OnTriggerEnter2D(Collider2D other)
   {
      if (other.GetComponent<Player>() != null)
      {
         Invoke("DestroySelf",.5f);
         PlayerManager.instance.currency += currency;
      }
   }

   private void DestroySelf() => Destroy(gameObject);

   public void SetUpDeadBody(float _x, float _y, long _currency)
   {
      transform.position = new Vector2(_x, _y);
      currency = _currency;
      GameManager.instance.lostCurrency = 0;
   }
}
