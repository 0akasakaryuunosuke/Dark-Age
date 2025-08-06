using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;
   public  Rigidbody2D rb;
   
   private void OnValidate()
   {
       SetUpVisual();
   }

   // ReSharper disable Unity.PerformanceAnalysis
   private void SetUpVisual()
   {
       if (!itemData)
           return;
       GetComponent<SpriteRenderer>().sprite=itemData.icon;
       gameObject.name = "ItemObject`name:" + itemData.itemName;
   }

   public void PickUpItem()
   {
       if (!Inventory.instance.CanPickUp(itemData.itemType))
       {
           rb.velocity = new Vector2(Random.Range(-5, 5), 10);
           return;
       }
       Inventory.instance.AddItem(itemData);
       Destroy(gameObject);
   }

   public void SetUpItem(ItemData _itemData,Vector2 _velocity)
   {
       itemData = _itemData;
       rb.velocity = _velocity;
       SetUpVisual();
   }
   private void Update()
   {
   }

   public void StayZeroVelocity() => rb.velocity = Vector2.zero;
}
