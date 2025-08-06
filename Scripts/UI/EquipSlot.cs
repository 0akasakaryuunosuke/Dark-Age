using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipSlot : ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment Slot -" + slotType;
    }
    

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            Inventory.instance.UnequipItem(item.itemData as ItemData_Equipment);
            Inventory.instance.AddItem(item.itemData as ItemData_Equipment);
            CleanUpSlot();
        }
    }

   
}
