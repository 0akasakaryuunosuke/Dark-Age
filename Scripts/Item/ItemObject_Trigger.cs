using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ItemObject_Trigger : MonoBehaviour
{
   private ItemObject myObject => GetComponentInParent<ItemObject>();
    private void OnTriggerEnter2D(Collider2D other)
    {
        
        if(other.GetComponent<ItemObject_Trigger>()!=null)
            return;
        if (other.GetComponent<Player>() != null)
        {    
            if(other.GetComponent<PlayerStat>().isDead)
                return;
           myObject.PickUpItem();
        }
        if(other.GetComponent<TilemapCollider2D>())
            myObject.StayZeroVelocity();
    }   
}
