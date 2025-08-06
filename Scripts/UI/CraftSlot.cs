using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class CraftSlot : ItemSlot
{
    [SerializeField]private Image craftIcon;
    [SerializeField] private TextMeshProUGUI craftName;
    private void OnEnable()
    {
            //UpdateItemOnly(item);
    }

    public void SetUpCraftSlot(ItemData_Equipment _equipment)
    {
        item.itemData = _equipment;
        craftIcon.sprite = _equipment.icon;
        craftName.text = _equipment.itemName;
        if (craftName.text.Length > 12)
            craftName.fontSize *= .7f;
        else
            craftName.fontSize = 24;
    }

    private void OnValidate()
    {
            gameObject.name = "Craft Slot";
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            ui.craftUI.GetComponentInChildren<CraftWindow>().SetUpCraftWindow(item.itemData as ItemData_Equipment);
        }
    }
}
