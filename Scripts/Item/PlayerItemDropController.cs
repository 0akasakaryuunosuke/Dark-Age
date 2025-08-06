using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerItemDropController : ItemDropController
{
    [SerializeField] private int loosePercentage;
    
    public override void GenerateDrop()
    {
        Inventory inventory = Inventory.instance;
        CheckList(inventory.inventoryItems);
        CheckList(inventory.stashItems);
        CheckList(inventory.equipmentItems);
       
    }

    private void CheckList(List<InventoryItem> _list)
    {
        Inventory inventory = Inventory.instance;
        List<InventoryItem> itemsToLoose = new List<InventoryItem>(); 
        foreach (var item in _list)
        {
            if (Random.Range(0, 100) < loosePercentage)
            {
                DropItem(item.itemData);
                itemsToLoose.Add(item);
            }
        }
        if(_list==inventory.equipmentItems)
        {
            foreach (var item in itemsToLoose)
            {
                inventory.UnequipItem(item.itemData as ItemData_Equipment);
            }
        }

        foreach (var item in itemsToLoose)
        {
            inventory.RemoveItem(item.itemData);
        }
    }
}
